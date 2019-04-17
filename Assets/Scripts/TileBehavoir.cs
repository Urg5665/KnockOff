﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavoir : MonoBehaviour
{
    public bool destroyed;
    public int destroyTimer;

    public MeshRenderer mesh;
    public MeshCollider col;

    public int player1Score;
    public int player2Score;

    public DeathPlane deathPlane;

    // Start is called before the first frame update
    void Start()
    {
        mesh = this.GetComponent<MeshRenderer>();
        col = this.GetComponentInChildren<MeshCollider>();
        destroyed = false;
        destroyTimer = 0;
        player1Score = 3;
        player2Score = 3;
        deathPlane = GameObject.Find("DeathPlane").GetComponent<DeathPlane>();
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
        if(player1Score != deathPlane.player1Score || player2Score != deathPlane.player2Score)
        {
            //Debug.Log(player2Score + "  " + deathPlane.player2Score);

            //destroyed = false;
            //this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            destroyTimer += 3;
        }

        if (Input.GetKey(KeyCode.H))
        {
            destroyed = true;
        }

        if (destroyed)
        {
            col.enabled = false;
            destroyTimer++;
            double destroyTimerFloat = (double)destroyTimer;
            if (destroyTimer > 0 && destroyTimer < 200)
            {
                
                this.transform.position = new Vector3(transform.position.x,  - destroyTimer/2, transform.position.z);
            }
            else if (destroyTimer >= 200)
            {
                //Debug.Log("Rising");
                this.transform.position = new Vector3(transform.position.x, 1.04f + (- 250 + destroyTimer), transform.position.z);
            }
        }
        if (destroyTimer > 250)
        {
            destroyed = false;
            destroyTimer = 0;
            mesh.enabled = true;
            col.enabled = true;
            this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        if (!destroyed)
        {
            player1Score = deathPlane.player1Score;
            player2Score = deathPlane.player2Score;
        }
    }
}
