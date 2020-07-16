﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public int playZoneLength;
    public List<TowerLine> lines;
    public int numberOfColors;

    private int currentLowestActivatedLine;
    private Color[] colors;

    private float timeCpt = 0;

    // Start is called before the first frame update
    void Start()
    {
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

        currentLowestActivatedLine = lines.Count - playZoneLength;

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
