using System.Collections;
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
    public int boomSpellCounter; // TEST try to get three things to return in a curve
    public int boomCurveWidth;

    public int rangeCounter;
    public int maxRange;
    public int bombRange;

    public float fireForce;
    public float fireKnockUp;
    public Vector3 spellDir;

    public CameraMove cameraMove;

    public Vector3 dashTarget;

    public static bool hitPlayer;

    public GameObject hitEffect;
    public GameObject hitEffectInGame;

    public int hitSlow; // For Effects i guess?

    public AudioClip audioClip;
    public AudioSource audioSource;
    public bool AOEspell; // check for audio source
    
    public int hoverDur;

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
            playerAim = player.transform.GetChild(0).gameObject;
            playerControlXbox = player.GetComponent<PlayerControlXbox>();
            spellNum = playerControlXbox.spellSelected;
            dashTarget = GameObject.Find("Player1").transform.position;
        }

        maxRange = 10;
        transform.LookAt(playerAim.transform);
        spellDir = this.gameObject.transform.forward;
        fireForce = 700;
        fireKnockUp = 200;
        throwSpeed = 60; // 30
        rangeCounter = 0;
        cameraMove = GameObject.Find("MainCamera").GetComponent<CameraMove>();
        hitSlow = 101;
        audioClip = this.GetComponent<AudioSource>().clip;
        audioSource = this.GetComponent<AudioSource>();
        bombRange = 20;
        //bombSpell = false;
        hitPlayer = false;
        boomSpell = false;
        boomReturn = false;
        boomHover = false;
        hoverDur = 0;
        boomCurveWidth = 150;
        if (AOEspell)
        {
            audioSource.volume = 0.2f;
        }

    }

     void OnTriggerEnter(Collider collision)
    {
        if (playerInt == 1 && collision.gameObject.tag == "Cliffs")
        { 
            playerControl.canCast[spellNum] = true;
            playerControl.spellPrimary[spellNum] = "";
            playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty

            hitEffectInGame = Instantiate(hitEffect);
            hitEffectInGame.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
            Destroy(this.gameObject);
        }
        if (playerInt == 2 && collision.gameObject.tag == "Cliffs")
        {
            playerControlXbox.canCast[spellNum] = true;
            playerControlXbox.spellPrimary[spellNum] = "";
            playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty

            hitEffectInGame = Instantiate(hitEffect);
            hitEffectInGame.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
            Destroy(this.gameObject);
        }

        if (playerInt == 1 && collision.gameObject.tag == "Player1" && boomReturn)
        {
            Destroy(this.gameObject);
        }

        if (playerInt == 2 && collision.gameObject.tag == "Player2" && boomReturn)
        {
            Destroy(this.gameObject);
        }

        if (!hitPlayer && playerInt == 1 && collision.gameObject.tag == "Player2")
        {
            collision.gameObject.GetComponent<PlayerControlXbox>().finishDash();
            collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * fireForce); // Knock Back
            Debug.Log(this.gameObject.transform.forward); 
            if ((collision.gameObject.GetComponent<PlayerControlXbox>().stunLength > 0))
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * fireForce / 2); // Double If Stuned
            }
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * fireKnockUp); // Knock Up
            collision.GetComponent<BoxCollider>().isTrigger = true;
            //Destroy(this.gameObject);
            if (!boomReturn)
            {
                playerControl.canCast[spellNum] = true;
                playerControl.spellPrimary[spellNum] = "";
                playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
            }
            hitPlayer = true;
            hitSlow = 0;
            StartCoroutine(cameraMove.Shake(.15f, .5f));
            //cameraMove.player2Hit = true;
            hitEffectInGame = Instantiate(hitEffect);
            //hitEffectInGame.transform.position = this.transform.position;
            hitEffectInGame.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);

        }
        if (!hitPlayer && playerInt == 2 && collision.gameObject.tag == "Player1")
        {
            collision.gameObject.GetComponent<PlayerControl>().finishDash();
            collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * fireForce); // Knock Back
            if ((collision.gameObject.GetComponent<PlayerControl>().stunLength > 0))
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * fireForce / 2); // Double If Stuned
            }
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * fireKnockUp); // Knock Up
            collision.GetComponent<BoxCollider>().isTrigger = true;
            hitPlayer = true;
            if (!boomReturn)
            {
                playerControlXbox.canCast[spellNum] = true;
                playerControlXbox.spellPrimary[spellNum] = "";
                playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
            }
            hitSlow = 0;
            StartCoroutine(cameraMove.Shake(.15f, .5f));
            //cameraMove.player1Hit = true;
            hitEffectInGame = Instantiate(hitEffect);
            //hitEffectInGame.transform.position = this.transform.position;
            hitEffectInGame.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);

        }

    }
    private void Start()
    {
        if (AOEspell)
        {
            audioSource.volume = 0.2f;
        }
    }

    void FixedUpdate()
    {
        if (AOEspell)
        {
            audioSource.volume = 0.2f;
        }
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
        if (playerInt == 1)
        {
            dashTarget = GameObject.Find("Player2").transform.position;
        }
        if (playerInt == 2)
        {
            dashTarget = GameObject.Find("Player1").transform.position;
        }

        if (dashSpell)
        {
            transform.LookAt(dashTarget);
        }
        if (!boomHover)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * throwSpeed, Space.Self);
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
            if (boomSpellCounter == 1)
            {
                transform.RotateAround(player.transform.position, Vector3.up, boomCurveWidth * Time.deltaTime);
            }
            if (boomSpellCounter == 2)
            {
                transform.RotateAround(player.transform.position, Vector3.up, -boomCurveWidth * Time.deltaTime);
            }
            //transform.LookAt(dashTarget); If you want to go to  opposing player // run this
        }

        if (rangeCounter == maxRange + 1)
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
                    //playerControl.canCast[spellNum] = true;
                    //playerControl.spellPrimary[spellNum] = "";
                    //playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
                }
                playerControl.canCast[spellNum] = true;
                playerControl.spellPrimary[spellNum] = "";
                playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
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
                    //playerControlXbox.canCast[spellNum] = true;
                    //playerControlXbox.spellPrimary[spellNum] = "";
                    //playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
                }
                playerControlXbox.canCast[spellNum] = true;
                playerControlXbox.spellPrimary[spellNum] = "";
                playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
            }
        }
        if (boomHover)
        {
            //print("hovering for:  " + hoverDur);
            hoverDur++;
            if (hoverDur > 15 && hoverDur < 40)
            {
                this.transform.position += new Vector3(0, .2f, 0);
            }
            if(hoverDur > 95)
            {
                this.transform.position -= new Vector3(0, 1f, 0);
            }
            
        }

        if (rangeCounter == (maxRange * 6))
        {
            if (boomSpell)
            {
                boomHover = false;
                boomReturn = true;
                hitPlayer = false;
                audioSource.Play();
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

