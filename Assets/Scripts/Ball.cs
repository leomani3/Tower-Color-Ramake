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
        if(!GetComponent<Rigidbody>().isKinematic)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, gravity, 0));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == target && collision.gameObject.GetComponent<TowerBlock>())
        {
            SoftDestroy();
            TowerBlock block = collision.gameObject.GetComponent<TowerBlock>();
            if (block.IsEnable)
            {
                if (block.color == color)
                {
                    StartCoroutine(block.PropagateDestroy());

                }
                else
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
