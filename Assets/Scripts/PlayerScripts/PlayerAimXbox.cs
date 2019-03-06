﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimXbox : MonoBehaviour
{
    public Transform parent;
    public GameObject player;
    public PlayerControlXbox playerControlXbox;

    public float xDif;
    public float zDif;

    public float xPos;
    public float zPos;

    public float hypo;

    public float angle;

    public float speed;
    public float playerSpeed;

    public int spellSelected;

    public int snapOffset; // Change for AOE so that it doesnt fuck up in that direction
    // Start is called before the first frame update
    void Start()
    {
        speed = 20;
        playerSpeed = 10;
    }

    // Update is called once per frame
    void Update()
    {

        xPos = this.transform.position.x - parent.position.x;
        zPos = this.transform.position.z - parent.position.z;
        xDif = Mathf.Abs(this.transform.position.x - parent.position.x);
        zDif = Mathf.Abs(this.transform.position.z - parent.position.z);

        hypo = Mathf.Sqrt((xDif * xDif + zDif * zDif));

        angle = Mathf.Rad2Deg * (Mathf.Asin(zDif / hypo));


        if ( angle > 45) // north south
        {
            if(zPos > 0)
            {
                spellSelected = 0;
            }
            if (zPos <= 0)
            {
                spellSelected = 2;
            }
        }
        else if ( angle <= 45) // east west
        {
            if (xPos > 0)
            {
                spellSelected = 1;
            }
            if (xPos <= 0)
            {
                spellSelected = 3;
            }
        }

        Vector3 privousPos = this.transform.position;
        Vector3 nextPos = new Vector3(parent.transform.position.x + (Input.GetAxis("HorAim") * 10), parent.transform.position.y, parent.transform.position.z - (Input.GetAxis("VerAim") * 10));
        transform.position = Vector3.Lerp(privousPos, nextPos, Time.deltaTime * 20);

    }
}
