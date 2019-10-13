using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuakeThrow : MonoBehaviour
{
    public float throwSpeed;
    public int playerInt;
    public int spellNum;

    public GameObject player;
    public GameObject playerAim;
    public PlayerControl playerControl;
    public PlayerControlXbox playerControlXbox;

    public bool dashSpell; // This will tell the spell to seek out the oppoentafter a dash// to hard to cast after dashing
    public int rangeCounter;
    public int maxRange;


    public bool boomSpell; // Code for Boomerang, comes back after
    public bool boomReturn;
    public bool boomHover;
    public int boomSpellCounter;
    public int boomCurveWidth;


    //public CameraMove cameraMove;

    public Vector3 dashTarget;

    public GameObject hitEffect;
    public GameObject hitEffectInGame;

    public int hitSlow; // For Effects i guess?

    public AudioClip audioClip;
    public AudioSource audioSource;
    public bool AOEspell; // check for audio source
    public int minReturnDistance;

    public GameObject earthParticle; // unique gameobject particles for earth
    public GameObject newPart;
    public int hoverDur;

    public float lobSpeed;
    public float lobDec;

    private void Awake()
    {
        lobSpeed = 20;
        lobDec = 1;
        if (playerInt == 1)
        {
            player = GameObject.Find("Player1");
            playerAim = player.transform.GetChild(0).gameObject;
            playerControl = player.GetComponent<PlayerControl>();
            spellNum = playerControl.spellSelected;
            dashTarget = GameObject.Find("Player2").transform.position;
            dashTarget = new Vector3(dashTarget.x , dashTarget.y - .5f, dashTarget.z );
            if (playerControl.spellSecondary[spellNum] == "Range")
            {
                lobSpeed = 60;
                lobDec = 5;
            }
        }
        if (playerInt == 2)
        {
            player = GameObject.Find("Player2");
            playerAim = GameObject.Find("Player2Aim");
            playerControlXbox = player.GetComponent<PlayerControlXbox>();
            spellNum = playerControlXbox.spellSelected;
            dashTarget = GameObject.Find("Player1").transform.position;
            dashTarget = new Vector3(dashTarget.x, dashTarget.y - .5f, dashTarget.z);
        }
        maxRange = 10;
        transform.LookAt(playerAim.transform);
        throwSpeed = 30;
        rangeCounter = 0;
        //cameraMove = GameObject.Find("MainCamera").GetComponent<CameraMove>();
        hitSlow = 101;
        audioClip = this.GetComponent<AudioSource>().clip;
        audioSource = this.GetComponent<AudioSource>();
        boomSpell = false;
        boomReturn = false;
        boomHover = false;
        minReturnDistance = 5;
        hoverDur = 0;
        boomCurveWidth = 50;


        //spellMesh = this.GetComponent<Mesh>();
        if (AOEspell)
        {
            audioSource.volume = 0.2f;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Ground" && rangeCounter > 3)
        {
            collision.gameObject.GetComponentInParent<TileBehavoir>().destroyed = true;
        }
    }
    private void Start()
    {
        if (AOEspell)
        {
            audioSource.volume = 0.2f;
        }
        if (dashSpell)
        {
            transform.LookAt(dashTarget);
        }
    }

    void FixedUpdate()
    {
        this.transform.eulerAngles += new Vector3(0, 0, 20f);
        newPart = Instantiate(earthParticle);
        newPart.transform.position = this.transform.position;
        newPart.transform.position = new Vector3(newPart.transform.position.x + 0.5f, newPart.transform.position.y, newPart.transform.position.z);
        if (maxRange > 60) // ranged spell
        {
            newPart = Instantiate(earthParticle);

            newPart.transform.position = this.transform.position;
            newPart.transform.position = new Vector3(newPart.transform.position.x - 0.5f, newPart.transform.position.y, newPart.transform.position.z);
        }
        if (AOEspell)
        {
            audioSource.volume = 0.2f;
        }
        if (!dashSpell)
        {
            if (!boomHover)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * throwSpeed, Space.Self);
                transform.Translate(Vector3.up * Time.deltaTime * lobSpeed, Space.World);
                lobSpeed -= lobDec;
                // some reall sloppy player specific stuff dont replicate
                if (playerInt == 1)
                {
                    if (playerControl.spellSecondary[spellNum] == "Range")
                    {
                        throwSpeed = 60;
                        if (rangeCounter == 10)
                        {
                            //playerControl.createBomb(1, playerControl.spellProjectile[0]);
                        }
                    }

                }

            }

        }
        if (dashSpell)
        {
            if (playerInt == 1)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * 1.5f * throwSpeed, Space.Self);
                this.transform.position = new Vector3(this.transform.position.x, 1f, this.transform.position.z);
                //transform.position = Vector3.MoveTowards(transform.position, dashTarget, 1.5f * throwSpeed * Time.deltaTime);
            }
            if (playerInt == 2)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * 1.5f * throwSpeed, Space.Self);
                this.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
                //transform.position = Vector3.MoveTowards(transform.position, dashTarget, 1.5f * throwSpeed * Time.deltaTime);
            }
        }
        rangeCounter++;

        if (boomReturn)
        {
            transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y - 1f, player.transform.position.z));
            if (boomSpellCounter == 1)
            {
                transform.RotateAround(player.transform.position, Vector3.up, boomCurveWidth * Time.deltaTime);
            }
            if (boomSpellCounter == 2)
            {
                transform.RotateAround(player.transform.position, Vector3.up, -boomCurveWidth * Time.deltaTime);
            }
            //Debug.Log(Mathf.Abs(this.transform.position.x - player.transform.position.x) + "   " + Mathf.Abs(this.transform.position.z - player.transform.position.z));
            if (Mathf.Abs(this.transform.position.x - player.transform.position.x) < minReturnDistance && Mathf.Abs(this.transform.position.z - player.transform.position.z) < minReturnDistance)
            {
                if (playerInt == 1)
                {
                    Destroy(this.gameObject);
                    //playerControl.canCast[spellNum] = true;
                    //playerControl.spellPrimary[spellNum] = "";
                    //playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
                }
                if (playerInt == 2)
                {
                    Destroy(this.gameObject);
                    //playerControlXbox.canCast[spellNum] = true;
                    //playerControlXbox.spellPrimary[spellNum] = "";
                    //playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
                }
            }
        }

        if (rangeCounter == maxRange + 1)
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
            if (hoverDur > 60 && hoverDur < 85)
            {
                this.transform.position += new Vector3(0, .2f, 0);
            }
            if (hoverDur > 95)
            {
                this.transform.position -= new Vector3(0, 1f, 0);
            }

        }
        if (rangeCounter == (maxRange * 3.5))
        {
            if (boomSpell)
            {
                boomHover = false;
                boomReturn = true;
                audioSource.Play();
            }

        }


    }
}
