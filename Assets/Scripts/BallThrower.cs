using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [Header("Throw physics")]
    public float height;

    [Space]
    public float speed;
    public Transform ballPosition;
    public GameObject ballPrefab;
    public LayerMask raycastLayer;

    private GameObject currentBall;

    private Color pickedColor;

    private Color[] colors;

    private float targetElevation;

    private float pressDuration;
    private float dragOldX;
    public void Setup(Color[] cols)
    {
        colors = cols;
        SpawnNewBall();
    }

    void Update()
    {
        //Keep the Camera at play zone level
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetElevation, transform.position.z), speed * Time.deltaTime);

        //----------------Handle Player Input-------------------
        if (Input.GetMouseButton(0))
        {
            pressDuration += Time.deltaTime;

            if (pressDuration >= 0.2f) //the user pressed long enough to trigger the camera drag move
            {
                if (dragOldX == 0)
                {
                    dragOldX = Input.mousePosition.x;
                }
                else
                {
                    transform.RotateAround(Vector3.zero, Vector3.up, Input.mousePosition.x - dragOldX);
                    dragOldX = Input.mousePosition.x;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (pressDuration <= 0.2f) //user taped
            {
                Launch();
            }
            pressDuration = 0;
            dragOldX = 0;
        }
        //---------------------------------------------------------
    }

    public void SetCameraElevation(float f)
    {
        targetElevation = f;
    }

    public void SpawnNewBall()
    {
        pickedColor = colors[Random.Range(0, colors.Length)];

        currentBall = Instantiate(ballPrefab, ballPosition.position, Quaternion.identity, transform);
        currentBall.GetComponent<Ball>().SetColor(pickedColor);
    }

    public void Launch()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        float gravity = ballPrefab.GetComponent<Ball>().gravity;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastLayer))
        {
            Vector3 endPoint = hit.point;

            float displacementY = endPoint.y - ballPosition.position.y;
            Vector3 displacementXZ = new Vector3(endPoint.x - ballPosition.position.x, 0, endPoint.z - ballPosition.position.z);

            float time = (Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity));

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
            Vector3 velocityXZ = displacementXZ / time;

            currentBall.GetComponent<Rigidbody>().isKinematic = false;
            currentBall.GetComponent<Rigidbody>().velocity = velocityXZ + velocityY;

            currentBall.transform.parent = transform.parent;

            SpawnNewBall();
        }
    }
}
