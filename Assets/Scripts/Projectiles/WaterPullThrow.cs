using System.Collections;
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
    public GameObject playerHit;


    public bool dashSpell; // This will tell the spell to seek out the oppoentafter a dash// to hard to cast after dashing
    public bool boomSpell;
    public bool boomHover;
    public bool boomReturn;

    public float waterForce;
    public float waterKnockUp;
    public Vector3 spellDir;

    public int rangeCounter;
    public int maxRange;
    public int bombRange;
    public Vector3 dashTarget;

    public static bool hitPlayer; // so physics doesnt freek out, this means the first time the player is hit by this is does not fucking anhiliate them - Mark

    public int hitSlow; // For Effects i guess?

    public CameraMove cameraMove;
    public GameObject hitEffect;
    public GameObject hitEffectInGame;

    public AudioSource audioSource;
    public bool AOEspell; // check for audio source

    public int hoverDur;
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
        waterForce = 700;
        waterKnockUp = 250;
        hitPlayer = false;
        throwSpeed = 60;
        rangeCounter = 0;
        hitSlow = 101;
        cameraMove = GameObject.Find("MainCamera").GetComponent<CameraMove>();
        boomSpell = false;
        boomReturn = false;
        boomHover = false;
        hoverDur = 0;
        if (AOEspell)
        {
            audioSource.volume = 0.2f;
        }
    }

    private void OnTriggerEnter(Collider collision)
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
            //playerControl.canCast[spellNum] = true;
            //playerControl.spellPrimary[spellNum] = "";
            //playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
        }
        if (playerInt == 2 && collision.gameObject.tag == "Player2" && boomReturn)
        {
            Destroy(this.gameObject);
            //playerControlXbox.canCast[spellNum] = true;
            //playerControlXbox.spellPrimary[spellNum] = "";
            //playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
        }

        if (!hitPlayer && playerInt == 1 && collision.gameObject.tag == "Player2" )
        {
            collision.gameObject.GetComponent<PlayerControlXbox>().finishDash();
            collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * waterForce * -1); // Knock Back
            if ((collision.gameObject.GetComponent<PlayerControlXbox>().stunLength > 0))
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * waterForce * -1); // Double If Stuned
            }
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * waterKnockUp); // Knock Up
            playerHit = collision.gameObject;                                                                               
            collision.GetComponent<BoxCollider>().isTrigger = true;
            hitPlayer = true;
            if (!boomReturn)
            {
                playerControl.canCast[spellNum] = true;
                playerControl.spellPrimary[spellNum] = "";
                playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
            }

            hitSlow = 0;
            StartCoroutine(cameraMove.Shake(.15f, .5f));
            cameraMove.player2Hit = true;
            hitEffectInGame = Instantiate(hitEffect);
            //hitEffectInGame.transform.position = this.transform.position;
            hitEffectInGame.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        }
        if (!hitPlayer && playerInt == 2 && collision.gameObject.tag == "Player1")
        {
            collision.gameObject.GetComponent<PlayerControl>().finishDash();
            collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * waterForce * -1); // Knock Back
            if (collision.gameObject.GetComponent<PlayerControl>().stunLength > 0)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * waterForce * -1); // Double If Stuned
            }
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * waterKnockUp); // Knock Up
            playerHit = collision.gameObject;                                                                               
            collision.GetComponent<BoxCollider>().isTrigger = true;
            //Destroy(this.gameObject);
            hitPlayer = true;
            if (!boomReturn)
            {
                playerControlXbox.canCast[spellNum] = true;
                playerControlXbox.spellPrimary[spellNum] = "";
                playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
            }
            hitSlow = 0;
            StartCoroutine(cameraMove.Shake(.15f, .5f));
            cameraMove.player1Hit = true;
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
            if (!boomSpell)
            {
                Destroy(this.gameObject);
            }

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


        rangeCounter++;

        if (boomReturn)
        {
            transform.LookAt(player.transform.position);
        }

        if (rangeCounter == 1 + maxRange)
        {
            if (playerInt == 1)
            {
                if (boomSpell)
                {
                    boomHover = true;
                }
                else if (!boomSpell)
                {
                    Destroy(this.gameObject);
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
                }
                playerControlXbox.canCast[spellNum] = true;
                playerControlXbox.spellPrimary[spellNum] = "";
                playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
            }
        }
        if (boomHover)
        {
            hoverDur++;
            if (hoverDur > 15 && hoverDur < 40)
            {
                this.transform.position += new Vector3(0, .2f, 0);
            }
            if (hoverDur > 55)
            {
                this.transform.position -= new Vector3(0, 1f, 0);
            }

        }
        if (rangeCounter == (maxRange * 4))
        {
            if (boomSpell)
            {
                boomReturn = true;
                boomHover = false;
                hitPlayer = false;
                audioSource.Play();
            }
        }
    }
}
