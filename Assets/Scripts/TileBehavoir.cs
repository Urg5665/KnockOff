﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavoir : MonoBehaviour
{
    public bool destroyed;
    public int destroyTimer;

    public MeshRenderer mesh;
    public MeshCollider col;

    // Start is called before the first frame update
    void Start()
    {
        mesh = this.GetComponent<MeshRenderer>();
        col = this.GetComponentInChildren<MeshCollider>();
        destroyed = false;
        destroyTimer = 0;
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "earthQuake")
        {
            Debug.Log(this.gameObject.name + " was Destroyed");
            destroyed = true;
        }
    }
    

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.H))
        {
            destroyed = true;
        }

        if (destroyed)
        {
            //mesh.enabled = false;
            col.enabled = false;
            destroyTimer++;
            double destroyTimerFloat = (double)destroyTimer;
            if (destroyTimer > 0 && destroyTimer < 150)
            {
                
                this.transform.position = new Vector3(transform.position.x,  - destroyTimer/2, transform.position.z);
            }
            else if (destroyTimer >= 150)
            {
                //Debug.Log("Rising");
                this.transform.position = new Vector3(transform.position.x, 1.04f + (- 200 + destroyTimer), transform.position.z);
            }
        }
        if (destroyTimer > 200)
        {
            destroyed = false;
            destroyTimer = 0;
            mesh.enabled = true;
            col.enabled = true;
            this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }



    }
}
