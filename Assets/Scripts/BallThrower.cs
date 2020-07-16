using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BallThrower : MonoBehaviour
{
    public LayerMask raycastLayer;
    public Color pickedColor;

    private Color[] colors;

    private float pressDuration;
    private float dragOldX;

    void Update()
    {
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
                Fire();
            }
            pressDuration = 0;
            dragOldX = 0;
        }
    }

    public void SpawnNewBall()
    {
        pickedColor = colors[Random.Range(0, colors.Length)];
    }

    public void Fire()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
 
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastLayer))
        {
            TowerBlock block = hit.collider.gameObject.GetComponent<TowerBlock>();
            if (block.color == pickedColor)
            {
                StartCoroutine(block.PropagateDestroy());
                
            }
            else
            {
                StartCoroutine(block.PropagateColor(pickedColor));
            }
        }

        SpawnNewBall();
    }

    public void Setup(Color[] cols)
    {
        colors = cols;
        SpawnNewBall();
    }
}
