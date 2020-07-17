using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBlock : MonoBehaviour
{
    public Color color;
    private GameObject child;
    private bool isEnabled;
    private GameObject cameraHolder;
    private void Awake()
    {
        child = transform.GetChild(0).gameObject; //I created a clone of the TowerBlock as a child without the collider, rb, etc... just to animate without breaking the physic
    }

    private void Start()
    {
        cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder");
    }

    public IEnumerator PropagateColor(Color c)
    {
        //save the current color of the object
        Color currentColor = GetColor();
        //scan for neighbors
        Collider[] cols = Physics.OverlapSphere(transform.position, 0.9f, LayerMask.GetMask("TowerBlock"));
        //Animation
        child.GetComponent<Animator>().SetTrigger("ChangeColor");
        //Set the new color of the current object 
        SetColor (c);

        yield return new WaitForSeconds(0.05f);

        //recursively call the propagation on all neighbors of the same color
        foreach (Collider col in cols)
        {
            TowerBlock neighbor = col.GetComponent<TowerBlock>();
            if (neighbor.GetColor() == currentColor && col.gameObject != gameObject)
            {
                StartCoroutine(neighbor.PropagateColor(c));
            }
        }
    }

    public IEnumerator PropagateDestroy()
    {
        cameraHolder.GetComponent<Animator>().SetBool("Shake", true);
        //save the current color of the object
        Color currentColor = color;
        //scan for neighbors
        Collider[] cols = Physics.OverlapSphere(transform.position, 1, LayerMask.GetMask("TowerBlock"));

        //GameObject vfx = Instantiate(Resources.Load("TowerBlockDestroy"), transform.position, transform.rotation) as GameObject; //laggyyyyyyyy
        //vfx.GetComponent<ParticleSystem>().startColor = GetColor();

        SoftDestroy(); //visually ans physically destroy the object

        yield return new WaitForSeconds(0.03f);

        //recursively call the propagation on all neighbors of the same color
        foreach (Collider col in cols)
        {
            TowerBlock neighbor = col.GetComponent<TowerBlock>();
            if (neighbor.GetColor() == currentColor && col.gameObject != gameObject)
            {
                StartCoroutine(neighbor.PropagateDestroy());
            }
        }
        cameraHolder.GetComponent<Animator>().SetBool("Shake", false);
    }

    /// <summary>
    /// Allows to visually and physically destroy the object but it is not really destroy in the scene in case we still need the script to run (need to do that because of the potential Coroutines still runing).
    /// </summary>
    public void SoftDestroy()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public Color GetColor()
    {
        return GetComponent<MeshRenderer>().material.color;
    }

    public void SetColor(Color c)
    {
        color = c;
        Colorize(c);
    }

    public void Colorize(Color newColor)
    {
        GetComponent<MeshRenderer>().material.color = newColor;
        child.GetComponent<MeshRenderer>().material.color = newColor;
    }

    /// <summary>
    /// enabling the block by re-activating is rb and his color so it can be part of the play zone
    /// </summary>
    public void Enable()
    {
        isEnabled = true;
        Colorize(color);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    /// <summary>
    /// Disable a block by turning it black and freezing is rb so it doesn't interact with anything.
    /// </summary>
    public void Disable()
    {
        isEnabled = false;
        Colorize(Color.black);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public bool IsEnable
    {
        get { return isEnabled; }
    }
}
