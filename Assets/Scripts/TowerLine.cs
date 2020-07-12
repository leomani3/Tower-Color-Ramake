using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLine : MonoBehaviour
{
    private int towerBlockInside = 0;
    private TowerManager towerManager;
    private bool triggered = false;

    private void Awake()
    {
        towerManager = FindObjectOfType<TowerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        towerBlockInside++;
    }

    private void OnTriggerExit(Collider other)
    {
        towerBlockInside--;
        if (towerBlockInside == 0 && !triggered)
        {
            Debug.Log(name + " : empty");
            triggered = true;
            towerManager.LowerPlayZone();
        }
    }
}
