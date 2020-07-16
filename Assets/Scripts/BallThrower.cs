using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BallThrower : MonoBehaviour
{
    public LayerMask raycastLayer;
    public Color pickedColor;

    private Color[] colors;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
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
