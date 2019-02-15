﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardThrow : MonoBehaviour
{
    public float rotSpeed;
    public float throwSpeed;
    public int playerInt;
    public int cardNum;

    public GameObject player1;
    public GameObject player1Aim;
    public PlayerControl playerControl;

    public string resType;

    public bool toRes;
    public bool toPlayer;
    public int i;

    private void Start()
    {
        toRes = true;
        toPlayer = false;
        i = 0;
        player1 = GameObject.Find("Player1");
        player1Aim = GameObject.Find("Player1Aim");
        transform.LookAt(player1Aim.transform);
        playerControl = player1.GetComponent<PlayerControl>();
        cardNum = playerControl.spellSelected;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "fireRes")
        {
            toRes = false;
            toPlayer = true;
            resType = "Fire";
            i = 101;
        }
        if (collision.gameObject.tag == "windRes")
        {
            toRes = false;
            toPlayer = true;
            resType = "Wind";
            i = 101;
        }
        if (collision.gameObject.tag == "Target")
        {
            toRes = false;
            toPlayer = true;
            i = 101;
        }

        if (collision.gameObject.tag == "Player1" && i > 100)
        {
            playerControl.spellPrimary[cardNum] = resType;
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        if (toRes)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * throwSpeed, Space.Self);
            i++;
        }

        if (toPlayer)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed, Space.World);
            transform.position = Vector3.MoveTowards(transform.position, player1.transform.position, throwSpeed * Time.deltaTime);
        }
        if (i > 100)
        {
            toRes = false;
            toPlayer = true;
        }
    }

}
/*        if (Input.GetMouseButtonDown(0))
        {
            GameObject targetSave = Instantiate(cardTarget);
            targetSave.transform.position = this.transform.position;
            allTargets[cardsThrown] = targetSave;
            cardsThrown++;

        }

 * 
 */

