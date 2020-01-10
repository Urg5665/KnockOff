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
    public bool lobShot; // True for meteor

    // for mountain
    public bool destructive; // False for mountain
    public bool isMountain; //True for first lob, then false for the two that come out after


    // For meteor
    public bool isMeteor;
    public GameObject[] newSpellAOE;
    public Transform AOEpoint;
    public int aoeWidth;
    public GameObject fireProjectile;
    public GameObject earthProjectile;
    public int bombRange;

    private void Awake()
    {
        if (playerInt == 1)
        {
            player = GameObject.Find("Player1");
            playerAim = player.transform.GetChild(0).gameObject;
            playerControl = player.GetComponent<PlayerControl>();
            spellNum = playerControl.spellSelected;
            dashTarget = GameObject.Find("Player2").transform.position;
            dashTarget = new Vector3(dashTarget.x , dashTarget.y - .5f, dashTarget.z );
            AOEpoint = GameObject.Find("AOEPoint1").GetComponent<Transform>();
        }
        if (playerInt == 2)
        {
            player = GameObject.Find("Player2");
            playerAim = GameObject.Find("Player2Aim");
            playerControlXbox = player.GetComponent<PlayerControlXbox>();
            spellNum = playerControlXbox.spellSelected;
            dashTarget = GameObject.Find("Player1").transform.position;
            dashTarget = new Vector3(dashTarget.x, dashTarget.y - .5f, dashTarget.z);
            AOEpoint = GameObject.Find("AOEPoint2").GetComponent<Transform>();
        }
        maxRange = 10;
        bombRange = 15;
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
        if (collision.gameObject.tag == "Ground" )
        {
            if (destructive && rangeCounter > 3)
            {
                collision.gameObject.GetComponentInParent<TileBehavoir>().shattered = true;
            }
            else if (!destructive && !isMountain) // Mountain
            {
                collision.gameObject.GetComponentInParent<TileBehavoir>().rising = true;
                collision.gameObject.GetComponentInParent<TileBehavoir>().risingTimer = 0;
                collision.gameObject.GetComponentInParent<TileBehavoir>().mountainDir = this.transform.forward;
            }

        }
        if (collision.gameObject.tag == "Player2" && destructive && !isMeteor)
        {
            if (collision.gameObject.GetComponent<PlayerControlXbox>().inflamed == true)
            {
                collision.gameObject.GetComponent<PlayerControlXbox>().stunLength = 100;
                collision.gameObject.GetComponent<PlayerControlXbox>().maxStunLength = 100;
                print("inflamed Consumed");
                collision.gameObject.GetComponent<PlayerControlXbox>().inflamedTime = 0;
                collision.gameObject.GetComponent<PlayerControlXbox>().inflamed = false;

            }
            else
            {
                collision.gameObject.GetComponent<PlayerControlXbox>().stunLength = 50;
                collision.gameObject.GetComponent<PlayerControlXbox>().maxStunLength = 50;
            }

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
        if (destructive)
        {
            newPart = Instantiate(earthParticle);
            newPart.transform.position = this.transform.position;
            newPart.transform.position = new Vector3(newPart.transform.position.x + 0.5f, newPart.transform.position.y, newPart.transform.position.z);
        }
        
        if (!destructive)
        {
            this.GetComponent<MeshRenderer>().enabled = true;

        }
        

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
                if (lobShot)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * lobSpeed, Space.World);
                }
                lobSpeed -= lobDec;

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
                    if (isMeteor)
                    {
                        createBomb(8); // creates 8 firballs that will travel out from meteor
                    }
                    if (isMountain)
                    {
                        createMountainTangent(); // Creates 2 earth projectiles that will rise tiles
                        Debug.Log("Mounttaingent");
                    }
                    Destroy(this.gameObject);
                }
                /*
                playerControl.canCast[spellNum] = true;
                playerControl.spellPrimary[spellNum] = "";
                playerControl.spellSecondary[spellNum] = ""; // Reset Spell to empty
                */
            }
            if (playerInt == 2)
            {
                if (boomSpell)
                {
                    boomHover = true;
                }
                else if (!boomSpell)
                {
                    if (isMeteor)
                    {
                        createBomb(8); // creates 8 fireballs that will travel out from meteor
                    }
                    Destroy(this.gameObject);
                }
                /*
                playerControlXbox.canCast[spellNum] = true;
                playerControlXbox.spellPrimary[spellNum] = "";
                playerControlXbox.spellSecondary[spellNum] = ""; // Reset Spell to empty
                */
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
    public void createMountainTangent()
    {
        for (int i = 0; i < 2; i++)
        {
            newSpellAOE[i] = Instantiate(earthProjectile, earthProjectile.transform.position, this.transform.rotation);
            Debug.Log(newSpellAOE[i].name);
           
            //newSpellAOE[i].GetComponent<Transform>().rotation = Quaternion
            newSpellAOE[i].GetComponent<EarthQuakeThrow>().playerInt = playerInt;
            newSpellAOE[i].GetComponent<EarthQuakeThrow>().spellNum = spellNum;
            newSpellAOE[i].GetComponent<EarthQuakeThrow>().maxRange = 10;
            //newSpellAOE[i].GetComponent<EarthQuakeThrow>().AOEspell = true;;
            newSpellAOE[i].GetComponent<EarthQuakeThrow>().throwSpeed = 30;
            newSpellAOE[i].GetComponent<SphereCollider>().radius = 1f;
            newSpellAOE[i].GetComponent<EarthQuakeThrow>().destructive = false;
            newSpellAOE[i].GetComponent<EarthQuakeThrow>().isMountain = false;
            //bombCircle(newSpellAOE[i], i);
            splitPoints(newSpellAOE[i], i);
            newSpellAOE[i].GetComponent<EarthQuakeThrow>().transform.LookAt(AOEpoint);
            newSpellAOE[i].transform.position = new Vector3(newSpellAOE[i].transform.position.x, newSpellAOE[i].transform.position.y , newSpellAOE[i].transform.position.z);
            print(this.transform.rotation.eulerAngles.y);
            if (i == 0)
            {
                newSpellAOE[i].transform.rotation = Quaternion.Euler(newSpellAOE[i].transform.rotation.x, this.transform.rotation.eulerAngles.y + 90, newSpellAOE[i].transform.rotation.z);
            }
            else if (i == 1)
            {
                newSpellAOE[i].transform.rotation = Quaternion.Euler(newSpellAOE[i].transform.rotation.x, this.transform.rotation.eulerAngles.y - 90, newSpellAOE[i].transform.rotation.z);
            }


        }
    }


    public void createBomb(int L)
    {
        for (int i = 0; i < L; i++)
        {
            newSpellAOE[i] = Instantiate(fireProjectile, this.transform.position, fireProjectile.transform.rotation);
            newSpellAOE[i].GetComponent<FireBallThrow>().playerInt = playerInt;
            newSpellAOE[i].GetComponent<FireBallThrow>().spellNum = spellNum;
            newSpellAOE[i].GetComponent<FireBallThrow>().maxRange = 20;
            newSpellAOE[i].GetComponent<FireBallThrow>().AOEspell = true;
            newSpellAOE[i].GetComponent<FireBallThrow>().fireForce = 700;
            newSpellAOE[i].GetComponent<FireBallThrow>().fireKnockUp = 200;
            newSpellAOE[i].GetComponent<FireBallThrow>().throwSpeed = 40;
            newSpellAOE[i].GetComponent<FireBallThrow>().isMeteor = true;
            newSpellAOE[i].GetComponent<SphereCollider>().radius = 1f;
            bombCircle(newSpellAOE[i], i);
            newSpellAOE[i].GetComponent<FireBallThrow>().transform.LookAt(AOEpoint);
            newSpellAOE[i].transform.position = new Vector3(newSpellAOE[i].transform.position.x, newSpellAOE[i].transform.position.y + 1f, newSpellAOE[i].transform.position.z);
        }
    }
    public void splitPoints(GameObject parent,int i)
    {
        AOEpoint.position = parent.transform.position;
        float ang = 180 * (i) + (parent.transform.rotation.y);
        float radius = .1f;
        AOEpoint.position = new Vector3(parent.transform.position.x + (radius * Mathf.Sin(ang * Mathf.Deg2Rad)), this.transform.position.y, parent.transform.position.z + (radius * Mathf.Cos(ang * Mathf.Deg2Rad)));

    }
    public void bombCircle(GameObject parent, int i)
    {
        AOEpoint.position = parent.transform.position;
        float ang = 45 * i;
        float radius = .1f;
        AOEpoint.position = new Vector3(AOEpoint.transform.position.x + (radius * Mathf.Sin(ang * Mathf.Deg2Rad)), this.transform.position.y, AOEpoint.transform.position.z + (radius * Mathf.Cos(ang * Mathf.Deg2Rad)));
        //Debug.Log(i + ":" + AOEpoint.position);
        //AOEpoint.position 
        /*
        if (spellNum == 0 || spellNum == 2)
        {
            if (i == 1)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x + 7.5f, this.transform.position.y, AOEpoint.transform.position.z + 7.5f);
            }
            if (i == 2)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x, this.transform.position.y, AOEpoint.transform.position.z + 10f);
            }
            if (i == 3)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x + 7.5f, this.transform.position.y, AOEpoint.transform.position.z + 7.5f);
            }
            if (i == 4)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x - (aoeWidth / 2), this.transform.position.y, AOEpoint.transform.position.z);
            }
        }
        if (spellNum == 1 || spellNum == 3)
        {
            if (i == 1)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x, this.transform.position.y, AOEpoint.transform.position.z + aoeWidth / 2);
            }
            if (i == 2)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x, this.transform.position.y, AOEpoint.transform.position.z + (aoeWidth / 2));
            }
            if (i == 3)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x, this.transform.position.y, AOEpoint.transform.position.z - (aoeWidth * 1.5f));
            }
            if (i == 4)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x, this.transform.position.y, AOEpoint.transform.position.z - (aoeWidth / 2));
            }

        }*/
    }
}
