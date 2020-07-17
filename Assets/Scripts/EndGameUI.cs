using System.Collections;
using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [HideInInspector]
    public int ballsLeft = 0;
    public int[] starsRequierement = new int[3];
    public GameObject[] starsSlots = new GameObject[3];

    public TextMeshProUGUI ballsLeftText;
    public TextMeshProUGUI firstStarReqText;
    public TextMeshProUGUI secondStarReqText;
    public TextMeshProUGUI thirdStarReqText;

    private int nextReq = 0;
    // Start is called before the first frame update
    void Start()
    {
        firstStarReqText.text = starsRequierement[0].ToString();
        secondStarReqText.text = starsRequierement[1].ToString();
        thirdStarReqText.text = starsRequierement[2].ToString();

        StartCoroutine(CountBallsLeft());
    }

    public IEnumerator CountBallsLeft()
    {
        yield return new WaitForSeconds(1);
        for (int i = 1; i <= ballsLeft; i++)
        {
            ballsLeftText.text = "Balls left : " + i.ToString();
            if (i >= starsRequierement[nextReq])
            {
                Instantiate(Resources.Load("star") ,starsSlots[nextReq].transform.position, Quaternion.identity, transform);
                Mathf.Clamp(++nextReq, 0, starsRequierement.Length-1);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
