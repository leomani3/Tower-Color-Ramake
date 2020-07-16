using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TowerBlock block;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(block.PropagateDestroy());
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(block.PropagateColor(Color.green));
        }
    }
}
