using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float gravity;
    private Color color;
    private GameObject target;

    private void FixedUpdate()
    {
        //I wanted to have better controle on the ball so I decided to make it use its own gravity
        if(!GetComponent<Rigidbody>().isKinematic)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, gravity, 0));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //The ball hits a block
        if (collision.gameObject.GetComponent<TowerBlock>())
        {
            //make it disapear
            SoftDestroy();
            TowerBlock block = target.GetComponent<TowerBlock>();
            if (block.IsEnable)
            {
                //Good color -> propagate destroy
                if (block.color == color)
                {
                    StartCoroutine(block.PropagateDestroy());
                }
                else //Wrong color -> propagate color
                {
                    StartCoroutine(block.PropagateColor(color));
                }
            }
            Destroy(gameObject, 2);
        }
    }

    public void SoftDestroy()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
    }

    public void SetColor(Color c)
    {
        color = c;
        GetComponent<MeshRenderer>().material.color = c;
    }

    public void SetTarget(GameObject go)
    {
        target = go;
    }
}
