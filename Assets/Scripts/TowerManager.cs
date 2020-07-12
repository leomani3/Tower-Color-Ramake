using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public int playZoneLength;
    public List<GameObject> lines;

    private int currentLowestActivatedLine;

    private float timeCpt = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentLowestActivatedLine = lines.Count - playZoneLength;

        for (int i = 0; i < lines.Count; i++)
        {
            //represents all the lines below the playzone. Disaable them.
            if (i < currentLowestActivatedLine) 
            {
                foreach (TowerBlock towerBlock in lines[i].GetComponentsInChildren<TowerBlock>())
                {
                    towerBlock.Disable();
                }
            }
            else //enable the lines that are part of the playzone.
            {
                foreach (TowerBlock towerBlock in lines[i].GetComponentsInChildren<TowerBlock>())
                {
                    towerBlock.Enable();
                }
            }
        }
    }

    public void LowerPlayZone()
    {
        if (currentLowestActivatedLine > 0)
        {
            currentLowestActivatedLine--;
            foreach (TowerBlock towerBlock in lines[currentLowestActivatedLine].GetComponentsInChildren<TowerBlock>())
            {
                towerBlock.Enable();
            }
        }
    }
}
