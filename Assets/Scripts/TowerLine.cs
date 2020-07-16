using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLine : MonoBehaviour
{
    public Action onEmpty;

    private int blockCount = 0;
    private TowerManager towerManager;
    private bool triggered = false;

    private void Start()
    {
        foreach (TowerBlock towerBlock in GetComponentsInChildren<TowerBlock>())
        {
            towerBlock.onDestroy += DecreaseBlockCount;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        blockCount++;
    }

    public void DecreaseBlockCount()
    {
        blockCount--;
        if (blockCount == 0 && !triggered)
        {
            triggered = true;
            onEmpty?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DecreaseBlockCount();
    }
}
