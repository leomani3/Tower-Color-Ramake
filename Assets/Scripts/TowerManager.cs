using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public int playZoneLength;
    public List<TowerLine> lines;
    public int numberOfColors;
    public List<GameObject> confettis;
    public int winLevel;
    public GameObject tryAgainText;
    public GameObject levelCompleteText;
    public GameObject tapToContinueText;
    public GameObject endGameUI;

    private int currentLowestActivatedLine;
    private int currentHighestActivatedLine;
    private Color[] colors;

    private bool won = false;
    private bool lost = false;

    private BallThrower ballThrower;

    private float timeCpt = 0;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        ballThrower = FindObjectOfType<BallThrower>();

        //pick numberOfColors random colors. Added some stuff to make sure every color is different enough for the others
        colors = new Color[numberOfColors];
        float h = Random.Range(0.0f, 1.0f);
        for (int i = 0; i < numberOfColors; i++)
        {
            h = Mathf.Repeat(h + (Random.Range(0.2f, 0.3f)), 1);
            colors[i] = Color.HSVToRGB(h, 1, 1);
        }

        //pass those colors reference to the ball thrower
        FindObjectOfType<BallThrower>().Setup(colors);

        currentLowestActivatedLine = lines.Count - 1 - playZoneLength;
        currentHighestActivatedLine = lines.Count - 1;

        ballThrower.SetCameraElevation(lines[currentLowestActivatedLine + playZoneLength - 1].transform.position.y);

        for (int i = 0; i < lines.Count; i++)
        {
            //represents all the lines below the playzone. Disaable them.
            if (i < currentLowestActivatedLine)
            {
                foreach (TowerBlock towerBlock in lines[i].GetComponentsInChildren<TowerBlock>())
                {
                    towerBlock.SetColor(colors[Random.Range(0, numberOfColors)]);
                    towerBlock.Disable();
                }
            }
            else //enable the lines that are part of the playzone.
            {
                foreach (TowerBlock towerBlock in lines[i].GetComponentsInChildren<TowerBlock>())
                {
                    towerBlock.SetColor(colors[Random.Range(0, numberOfColors)]);
                    towerBlock.Enable();
                }
            }

            //subscribe to the event fired by a line whenever it is empty in order to update the play zone
            lines[i].onEmpty += LowerPlayZone;
        }
    }

    private void Update()
    {
        if (won)
        {
            ballThrower.transform.RotateAround(Vector3.zero, Vector3.up, 10 * Time.deltaTime);
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if(lost && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void LowerPlayZone()
    {
        if (!won)
        {
            currentHighestActivatedLine--;

            //WIN CONDITION
            if (currentHighestActivatedLine <= winLevel)
            {
                //trigger end game UI
                endGameUI.SetActive(true);
                endGameUI.GetComponent<EndGameUI>().ballsLeft = ballThrower.ballCount;

                int levelIndex = PlayerPrefs.GetInt("LevelIndex");
                won = true;
                levelCompleteText.SetActive(true);
                levelCompleteText.GetComponent<TextMeshProUGUI>().text = "Level " + levelIndex + " completed !";
                tapToContinueText.SetActive(true);
                foreach (GameObject go in confettis)
                {
                    go.SetActive(true);
                }

                levelIndex++;
                PlayerPrefs.SetInt("LevelIndex", levelIndex);
            }

            if (currentLowestActivatedLine > 0)
            {
                currentLowestActivatedLine--;
                foreach (TowerBlock towerBlock in lines[currentLowestActivatedLine].GetComponentsInChildren<TowerBlock>())
                {
                    towerBlock.Enable();
                }
            }

            //prevents the camera from going too low down.
            /*if (currentLowestActivatedLine > winLevel)
            {
                ballThrower.SetCameraElevation(lines[currentLowestActivatedLine + playZoneLength - 1].transform.position.y);
            }*/
            ballThrower.SetCameraElevation(lines[currentLowestActivatedLine + playZoneLength - 1].transform.position.y);
        }
    }

    public IEnumerator WaitToSeeIfLoss(Image circle)
    {
        circle.gameObject.SetActive(true);
        float timeCount = 0;
        while (timeCount <= 5)
        {
            timeCount += Time.deltaTime;
            circle.fillAmount = 1 - (timeCount / 5);
            yield return null;
        }

        if (currentHighestActivatedLine >= winLevel)
        {
            //LOSS
            lost = true;
            tryAgainText.SetActive(true);
            tapToContinueText.SetActive(true);
        }
    }
}
