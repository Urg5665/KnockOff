﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPullThrow : MonoBehaviour
{
    public float throwSpeed;
    public int playerInt;
    public int spellNum;

    public GameObject player;
    public GameObject playerAim;
    public PlayerControl playerControl;
    public PlayerControlXbox playerControlXbox;


    public bool dashSpell; // This will tell the spell to seek out the oppoentafter a dash// to hard to cast after dashing

    public float waterForce;
    public float waterKnockUp;
    public Vector3 spellDir;

    public int rangeCounter;
    public int maxRange;
    public Vector3 dashTarget;

    public static bool hitPlayer; // so physics doesnt freek out, this means the first time the player is hit by this is does not fucking anhiliate them - Mark

    public int hitSlow; // For Effects i guess?

    public CameraMove cameraMove;
    public GameObject hitEffect;
    public GameObject hitEffectInGame;

    private void Awake()
    {
        maxRange = 10;
        if (playerInt == 1)
        {
            player = GameObject.Find("Player1");
            playerAim = player.transform.GetChild(0).gameObject;
            playerControl = player.GetComponent<PlayerControl>();
            spellNum = playerControl.spellSelected;
            dashTarget = GameObject.Find("Player2").transform.position;
        }
        if (playerInt == 2)
        {
            player = GameObject.Find("Player2");
            playerAim = GameObject.Find("Player2Aim");
            playerControlXbox = player.GetComponent<PlayerControlXbox>();
            spellNum = playerControlXbox.spellSelected;
            dashTarget = GameObject.Find("Player1").transform.position;
        }

        transform.LookAt(playerAim.transform);

        spellDir = this.gameObject.transform.forward;
        waterForce = 600;
        waterKnockUp = 400;
        hitPlayer = false;
        throwSpeed = 30;
        rangeCounter = 0;
        hitSlow = 101;
        cameraMove = GameObject.Find("MainCamera").GetComponent<CameraMove>();
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (!hitPlayer && playerInt == 1 && collision.gameObject.tag == "Player2" )
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * waterForce * -1); // Knock Back
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * waterKnockUp); // Knock Up
            //Destroy(this.gameObject);
            playerControl.canCast[spellNum] = true;
            hitPlayer = true;
            playerControl.spellPrimary[spellNum] = "";
            playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
            hitSlow = 0;
            StartCoroutine(cameraMove.Shake(.15f, .5f));
            cameraMove.player2Hit = true;
            hitEffectInGame = Instantiate(hitEffect);
            //hitEffectInGame.transform.position = this.transform.position;
            hitEffectInGame.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        }
        if (!hitPlayer && playerInt == 2 && collision.gameObject.tag == "Player1")
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * waterForce * -1); // Knock Back
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * waterKnockUp); // Knock Up
            //Destroy(this.gameObject);
            playerControlXbox.canCast[spellNum] = true;
            hitPlayer = true;
            playerControlXbox.spellPrimary[spellNum] = "";
            playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
            hitSlow = 0;
            StartCoroutine(cameraMove.Shake(.15f, .5f));
            cameraMove.player1Hit = true;
            hitEffectInGame = Instantiate(hitEffect);
            //hitEffectInGame.transform.position = this.transform.position;
            hitEffectInGame.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        }
    }

    void FixedUpdate()
    {

        
        if (hitSlow == 0)
        {
            Time.timeScale = 0.2f;
            hitSlow++;
        }
        if (hitSlow <= 10)
        {
            hitSlow++;
        }
        if (hitSlow == 10)
        {
            Time.timeScale = 1.0f;
            Destroy(this.gameObject);
        }


        if (dashSpell)
        {
            transform.LookAt(dashTarget);
        }
        transform.Translate(Vector3.forward * Time.deltaTime * throwSpeed, Space.Self);

        rangeCounter++;

        if (rangeCounter > maxRange)
        {
            if (playerInt == 1)
            {
                Destroy(this.gameObject);
                playerControl.canCast[spellNum] = true;
                playerControl.spellPrimary[spellNum] = "";
                playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
            }

            if (playerInt == 2)
            {
                Destroy(this.gameObject);
                playerControlXbox.canCast[spellNum] = true;
                playerControlXbox.spellPrimary[spellNum] = "";
                playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
            }
        }
    }
}
