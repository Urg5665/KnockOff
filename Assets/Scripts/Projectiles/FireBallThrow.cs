﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallThrow : MonoBehaviour
{
    public float throwSpeed;
    public int playerInt;
    public int spellNum;
    public int fireBallID;

    public GameObject player;
    public GameObject playerAim;
    public PlayerControl playerControl;
    public PlayerControlXbox playerControlXbox;

    public bool dashSpell; // This will tell the spell to seek out the oppoentafter a dash// to hard to cast after dashing
    //public bool bombSpell; // This will tell the spell to explode ( Isntaitate 8x) after destoyed;

    public bool boomSpell; // Code for Boomerang, comes back after
    public bool boomReturn;
    //public GameObject[] newSpellBomb; move this to playerControl so that it is not lost on this destroy
    public bool boomHover;

    public int rangeCounter;
    public int maxRange;
    public int bombRange;

    public CameraMove cameraMove;

    public Vector3 dashTarget;

    public static bool hitPlayer;

    public GameObject hitEffect;
    public GameObject hitEffectInGame;

    public int hitSlow; // For Effects i guess?

    public AudioClip audioClip;
    public AudioSource audioSource;

    private void Awake()
    {
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

        maxRange = 10;
        transform.LookAt(playerAim.transform);
        throwSpeed = 30;
        rangeCounter = 0;
        cameraMove = GameObject.Find("MainCamera").GetComponent<CameraMove>();
        hitSlow = 101;
        audioClip = this.GetComponent<AudioSource>().clip;
        audioSource = this.GetComponent<AudioSource>();
        bombRange = 20;
        //bombSpell = false;
        boomSpell = false;
        boomReturn = false;
        boomHover = false;

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (playerInt == 1 && collision.gameObject.tag == "Player1" && boomReturn)
        {
            Destroy(this.gameObject);
            playerControl.canCast[spellNum] = true;
            playerControl.spellPrimary[spellNum] = "";
            playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
        }

        if (playerInt == 2 && collision.gameObject.tag == "Player2" && boomReturn)
        {
            Destroy(this.gameObject);
            playerControlXbox.canCast[spellNum] = true;
            playerControlXbox.spellPrimary[spellNum] = "";
            playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
        }


        if (playerInt == 1 && collision.gameObject.tag == "Player2")
        {
            //collision.gameObject.GetComponent<PlayerControlXbox>().speed = 0;
            
            StartCoroutine(cameraMove.Shake(.3f, .5f));
            // Kill
            //collision.gameObject.transform.position = 
            //   new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y - 6, collision.gameObject.transform.position.z);
            // Stun w kill
            if (collision.gameObject.GetComponent<PlayerControlXbox>().stunLength > 0 && collision.gameObject.GetComponent<PlayerControlXbox>().stunID != fireBallID) // yes stuned
            {
                print("p2 Killed:" + fireBallID);
                collision.gameObject.transform.position =
                new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y - 6, collision.gameObject.transform.position.z);
            }
            if (collision.gameObject.GetComponent<PlayerControlXbox>().stunLength <= 0) // not Stuned
            {
                collision.gameObject.GetComponent<PlayerControlXbox>().speed = 0;
                collision.gameObject.GetComponent<PlayerControlXbox>().stunID = fireBallID;
                collision.gameObject.GetComponent<PlayerControlXbox>().stunLength = 100;
                collision.gameObject.GetComponent<PlayerControlXbox>().dirStun = spellNum;
                print("p2 Stuned:" + fireBallID);
            }


           playerControl.canCast[spellNum] = true;
           playerControl.spellPrimary[spellNum] = "";
           playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
           hitEffectInGame = Instantiate(hitEffect);
           //hitEffectInGame.transform.position = this.transform.position;
           hitEffectInGame.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
           hitSlow = 0;
           //Destroy(this.gameObject);
        }
        if (playerInt == 2 && collision.gameObject.tag == "Player1")
        {
            StartCoroutine(cameraMove.Shake(.3f, .5f));
            //collision.gameObject.transform.position =
            //    new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y - 6, collision.gameObject.transform.position.z);
            // Stun

            if (collision.gameObject.GetComponent<PlayerControl>().stunLength > 0 && collision.gameObject.GetComponent<PlayerControl>().stunID != fireBallID ) // yes stuned , ignore other aoe spells
            {
                print("p1 Killed:" + fireBallID);
                collision.gameObject.transform.position =
                new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y - 6, collision.gameObject.transform.position.z);
            }
            if (collision.gameObject.GetComponent<PlayerControl>().stunLength <= 0) // not Stuned
            {
                print("p1 Stuned:" + fireBallID);
                collision.gameObject.GetComponent<PlayerControl>().speed = 0;
                collision.gameObject.GetComponent<PlayerControl>().stunID = fireBallID; // this is what the player remebers as stun
                collision.gameObject.GetComponent<PlayerControl>().stunLength = 100;
                collision.gameObject.GetComponent<PlayerControl>().dirStun = spellNum;
            }
            playerControlXbox.canCast[spellNum] = true;
            playerControlXbox.spellPrimary[spellNum] = "";
            playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
            hitEffectInGame = Instantiate(hitEffect);
            //hitEffectInGame.transform.position = this.transform.position;
            hitEffectInGame.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
            hitSlow = 0;
            //Destroy(this.gameObject);

        }

    }

    void FixedUpdate()
    {
        if (hitSlow == 0)
        {
            //Time.timeScale = 0.2f;
            Time.timeScale = 1.0f;
            hitSlow++;
        }
        if (hitSlow <= 10)
        {
            hitSlow++;
        }
        if (hitSlow == 10 && !audioSource.isPlaying)
        {
            Time.timeScale = 1.0f;
            Destroy(this.gameObject);
        }


        if (!dashSpell)
        {
            if (!boomHover)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * throwSpeed, Space.Self);
            }

        }
        if (dashSpell)
        {
            if (playerInt == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, dashTarget, throwSpeed * Time.deltaTime);
            }
            if (playerInt == 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, dashTarget, throwSpeed * Time.deltaTime);
            }
        }
        rangeCounter++;
 
        if (boomReturn)
        {
            transform.LookAt(player.transform.position);
            //transform.LookAt(dashTarget); If you want to go to  opposing player // run this
        }

        if (rangeCounter > maxRange)
        {
            if (playerInt == 1)
            {
                if (boomSpell)
                {
                    boomHover = true;
                }
                else if(!boomSpell)
                {
                    Destroy(this.gameObject);
                    playerControl.canCast[spellNum] = true;
                    playerControl.spellPrimary[spellNum] = "";
                    playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
                }
            }

            if (playerInt == 2)
            {
                if (boomSpell)
                {
                    boomHover = true;
                }
                else if (!boomSpell)
                {
                    Destroy(this.gameObject);
                    playerControlXbox.canCast[spellNum] = true;
                    playerControlXbox.spellPrimary[spellNum] = "";
                    playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
                }
            }
        }

        if (rangeCounter > maxRange * 3.5)
        {
            if (boomSpell)
            {
                boomHover = false;
                boomReturn = true;
            }
        }

    }
}
/*
 *                 if (bombSpell)
                {
                    //GameObject clone = Instantiate(this.gameObject);
                    //clone.GetComponent<FireBallThrow>().bombSpell = false;
                   // clone.GetComponent<FireBallThrow>().maxRange = 100;


                    for (int i = 0; i < 8; i++)
                    {
                        playerControl.newSpellBomb[i] = Instantiate(this.gameObject, this.transform.position, this.gameObject.transform.rotation);
                        //newSpellAOE[i].transform.position = new Vector3(newSpellAOE[i].transform.position.x, this.gameObject.transform.position.y - .25f, newSpellAOE[i].transform.position.z);
                        playerControl.newSpellBomb[i].GetComponent<FireBallThrow>().spellNum = spellNum;
                        playerControl.newSpellBomb[i].GetComponent<FireBallThrow>().maxRange = bombRange;
                        //playerControl.aoeCone(i);
                        playerControl.bombCircle(this.gameObject,i);
                        playerControl.newSpellBomb[i].GetComponent<FireBallThrow>().transform.LookAt(playerControl.AOEpoint);
                        playerControl.newSpellBomb[i].GetComponent<FireBallThrow>().bombSpell = false;
                    }
                }*/

