using System;
using UnityEngine;

public class TowerLine : MonoBehaviour
{
    public LayerMask layerBlock;
    public Action onEmpty;

    private bool triggered = false;
    private float timeCount = 0;

    private void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount >= 0.5f) //prevents from polling every frame for nothing
        {
            //check for blocks inside
            if (Physics.OverlapBox(transform.position, new Vector3(10, 0.1f, 10), Quaternion.Euler(Vector3.up), layerBlock).Length <= 0 && !triggered)
            {
                onEmpty?.Invoke();
                triggered = true;
                timeCount = 0;
            }
        }
    }
}
