using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlXbox : MonoBehaviour
{
    public GameObject player2Aim;
    //public PlayerAim playerAim;

    public int playerNum;
    public float speed;
    public GameObject playerUI;
    //public float maxSpeed = 10;

    public Transform movement;
    public Rigidbody rb;

    public bool grounded;
    public bool touchingWall;
    public int timeSinceWalled;

    public GameObject[] spellProjectile; // The actual Fireball, air block, earth wall
    public int spellSelected;
    public bool[] canCast;

    public GameObject[] onPlayerUIButton;
    //public PointerEventData pointerEvent;

    public string[] spellPrimary; // Keywords "Fire", "Wind", "Earth" "Water" use "" for empty
    public string[] spellSecondary; // Keywords "Aoe", "Range", "Lob"? not as sure about the last two use "" for empty

    public GameObject card;
    public GameObject newCard;
    public GameObject cardTrail;
    public GameObject newCardTrail;
    public Transform AOEpoint;

    public int dashDirection; // This is to check if you are fireing a particle afterwards, if still facing the same direction
    public bool dashing;
    public bool AOEKnockBack;
    public int dashingTime;
    public int dashDirectionTime;
    public Vector3 dashAim;
    public float waterDashForceUp;
    public bool castAfterDash;
    public int dashLength;
    public int cardsThrown;
    public float slowDownPerCard = 2.5f;

    public GameObject newSpell;
    public GameObject[] newSpellAOE;

    public int baseRange;
    public int baseSpeed;

    public int aoeRange;
    public float aoeWidth;

    public int rangeRange;
    public int rangeSpeed;

    public int dashSpellRange;

    public int boomBaseRange;
    public int boomBaseSpeed;

    // Testing Stun out on Player
    public int stunLength;
    public Text onPlayerText;
    public Image onPlayerStunRing;

    public bool airBorn;
    public int dirStun; // So that aoe doesnt double stun kill 0,1,2,3

    public int fireBallID;
    public int stunID; // So that players cannot be killed by smae fireball, and needs two differnt fire spells ( any direction) to kill
    public int baseDashCooldown;
    public Image onPlayerDashCooldownRing;
    public bool baseDashing;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        grounded = true;
        touchingWall = false;
        cardsThrown = 0;
        canCast = new bool[4]; // ignore zero here
        //onPlayerUIButton = new GameObject[4];
        waterDashForceUp = 0;
        dashDirectionTime = 0;
        dashing = false;
        AOEKnockBack = false;
        dashingTime = 0;
        castAfterDash = false;
        dashLength = 20;
        spellSelected = 0;
        baseDashCooldown = 0;
        baseRange = 20;
        baseSpeed = 60;
        aoeRange = 18; /// 30

        boomBaseRange = 20;
        boomBaseSpeed = 80;

        rangeRange = 40;
        rangeSpeed = 100;

        dashSpellRange = 15; // should be very close

        stunLength = 0;

        for (int i = 0; i < 4; i++)
        {
            canCast[i] = true;
            spellPrimary[i] = "";
            spellSecondary[i] = "";
        }
        slowDownPerCard = 2.5f;
        player2Aim = GameObject.Find("Player2Aim");
    }

    public void pickDirection()
    {
        spellSelected = player2Aim.GetComponent<PlayerAimXbox>().spellSelected;
    }

    void FixedUpdate()
    {
        //float newFloat = Input.GetAxis("CardThrow");
        //Debug.Log("Left Trigger: " + Input.GetAxisRaw("CardThrow").ToString() + "  Right Tirgger: " + Input.GetAxisRaw("SpellThrow").ToString());
        //Debug.Log("NewFloat: " + newFloat);
        pickDirection();
        dashDirectionTime--;
        aoeWidth = (Vector3.Distance(player2Aim.transform.position, transform.position)) / 2;

        //speed = maxSpeed - (slowDownPerCard * cardsThrown); // apply slow for each card in play
        //Debug.Log("speed" + speed);

        if (stunLength > 0)
        {
            //Debug.Log("Player2 Stunned");
            if (dashing)
            {
                print("Hit While Dashing");
            }
            dashing = false;
            dashingTime = 0;
            stunLength--;
            onPlayerText.text =  "" + stunLength;
            onPlayerStunRing.enabled = true;
            onPlayerStunRing.fillAmount = (float)stunLength/100;
        }
        if (stunLength == 0)
        {
            speed = 7.5f;
            onPlayerText.text = "";
            onPlayerStunRing.enabled = false;
        }


        if (Input.GetKeyDown(KeyCode.G))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.left * 600);
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 400);
        }
        if (baseDashCooldown > 0)
        {
            baseDashCooldown--;
            onPlayerDashCooldownRing.fillAmount = ((float)baseDashCooldown / 200);
        }
        if (!dashing)
        {
            playerUI.SetActive(true);
            //this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (dashing)
        {
            if (dashingTime == 0)
            {
                speed = 7.5f;
                onPlayerText.text = "";
                stunLength = 0;
            }
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            playerUI.SetActive(false);
            dashingTime++;
            if (AOEKnockBack)
            {
                transform.Translate(Vector3.back * Time.deltaTime * speed * 3, Space.Self);
            }
            if (!AOEKnockBack)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed * 5, Space.Self);
            }

            if (this.transform.position.y < 2.5)
            {
                this.GetComponent<BoxCollider>().isTrigger = false; // can fail recover
                Vector3 above = new Vector3(transform.position.x, transform.position.y + 20, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, above, Time.deltaTime);
                //transform.Translate(Vector3.up * Time.deltaTime * speed * 5, Space.Self);
            }
            else
            {
                // Come Back Here
                this.GetComponent<BoxCollider>().isTrigger = true;
            }
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        if (dashing && dashingTime > dashLength)
        {
            finishDash();
        }
        if (castAfterDash)
        {
            castAfterDash = false;
            for (int i = 0; i < 4; i++)
            {
                canCast[i] = true;
            }
            if (spellPrimary[dashDirection] == "Fire" && spellSecondary[dashDirection] == "Dash")
            {
                newSpell = Instantiate(spellProjectile[0], this.transform.position, spellProjectile[0].transform.rotation);
                newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
                newSpell.GetComponent<FireBallThrow>().spellNum = dashDirection;
                newSpell.GetComponent<FireBallThrow>().maxRange = dashSpellRange;
                newSpell.GetComponent<FireBallThrow>().dashSpell = true;
                fireBallID++;
                newSpell.GetComponent<FireBallThrow>().fireBallID = fireBallID;
                spellPrimary[dashDirection] = "";
                spellSecondary[dashDirection] = "";
                canCast[dashDirection] = true;
            }
            if (spellPrimary[dashDirection] == "Wind" && spellSecondary[dashDirection] == "Dash")
            {
                newSpell = Instantiate(spellProjectile[1], this.transform.position, spellProjectile[0].transform.rotation);
                newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
                newSpell.GetComponent<WindWaveThrow>().spellNum = dashDirection;
                newSpell.GetComponent<WindWaveThrow>().maxRange = dashSpellRange;
                newSpell.GetComponent<WindWaveThrow>().dashSpell = true;
                spellPrimary[dashDirection] = "";
                spellSecondary[dashDirection] = "";
                canCast[dashDirection] = true;
            }
            if (spellPrimary[dashDirection] == "Water" && spellSecondary[dashDirection] == "Dash")
            {
                newSpell = Instantiate(spellProjectile[2], this.transform.position, spellProjectile[0].transform.rotation);
                newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
                newSpell.GetComponent<WaterPullThrow>().spellNum = dashDirection;
                newSpell.GetComponent<WaterPullThrow>().maxRange = dashSpellRange;
                newSpell.GetComponent<WaterPullThrow>().dashSpell = true;
                spellPrimary[dashDirection] = "";
                spellSecondary[dashDirection] = "";
                canCast[dashDirection] = true;
            }
            if (spellPrimary[dashDirection] == "Earth" && spellSecondary[dashDirection] == "Dash")
            {
                newSpell = Instantiate(spellProjectile[3], this.transform.position, spellProjectile[0].transform.rotation);
                newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y -1f, newSpell.transform.position.z);
                newSpell.GetComponent<EarthQuakeThrow>().spellNum = dashDirection;
                newSpell.GetComponent<EarthQuakeThrow>().maxRange = dashSpellRange * 2;
                newSpell.GetComponent<EarthQuakeThrow>().dashSpell = true;
                spellPrimary[dashDirection] = "";
                spellSecondary[dashDirection] = "";
                canCast[dashDirection] = true;
            }
            for (int i = 0; i < 4; i++)
            {
                canCast[i] = true;
            }
        }
        // Card Casting Commands
        // Input.GetAxis("CardThrow") == 1
        //Input.GetButton("CardThrow")
        //Input.GetAxis("SpellThrow") == 1
        if (Input.GetButton("Fire1") == true && !dashing && baseDashCooldown <= 0) // Base Dash
        {
            castAfterDash = false;
            baseDashCooldown = 200;
            dashing = true;
            dashDirection = spellSelected;
            dashAim = new Vector3(player2Aim.transform.position.x, player2Aim.transform.position.y, player2Aim.transform.position.z);
            dashDirectionTime = 75;
            transform.LookAt(dashAim);
            baseDashing = true;

            if (this.transform.position.y < 2.5)
            {
                rb.AddForce(Vector3.up * waterDashForceUp);
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            this.GetComponent<BoxCollider>().isTrigger = true;
        }
        if (Input.GetAxis("CardThrow") == 1 && cardsThrown < 4 && canCast[spellSelected] && spellSecondary[spellSelected] == "" ) // Shoot Card
        {
            CardGather();
        }
        if (Input.GetAxis("CardThrow") == 1 && cardsThrown < 4 && canCast[spellSelected] && spellSecondary[spellSelected] != "")  // Disabel Shooitng Card because spell is maxed
        {
            //Debug.Log("Spell Maxed - Cast it!");
        }

        // Should really be Input.GetAxis("SpellThrow") == 1 but my controller trigger has not been working so for no it is Button A
        // Spell Casting Commands
        if (Input.GetButton("Fire1") == true && cardsThrown < 4 && canCast[spellSelected] && spellPrimary[spellSelected] == "") // You Have no Spell
        {
            //Debug.Log("No Spell Avaliable");
        }
        if ((Input.GetAxis("SpellThrow") == 1) && cardsThrown < 4 && canCast[spellSelected] && spellPrimary[spellSelected] == "Fire" ) // Shoot Fireball
        {
            Fireball();
        }
        if ((Input.GetAxis("SpellThrow") == 1) && cardsThrown < 4 && canCast[spellSelected] && spellPrimary[spellSelected] == "Wind") // Shoot Wind Knock
        {
            WindKnockback();
        }
        if ((Input.GetAxis("SpellThrow") == 1 ) && cardsThrown < 4 && canCast[spellSelected] && spellPrimary[spellSelected] == "Water") // Shoot Wind Knock
        {
            WaterPull();
        }
        if ((Input.GetAxis("SpellThrow") == 1) && cardsThrown < 4 && canCast[spellSelected] && spellPrimary[spellSelected] == "Earth") // Shoot Wind Knock
        {
            EarthQuake();
        }

        //Debug.Log(airBorn);

        if (grounded) // movement
        {
            if (Input.GetAxis("Horizontal") > 0)
                transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);
            if (Input.GetAxis("Horizontal") < 0)
                transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
            if (Input.GetAxis("Vertical") < 0)
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
            if (Input.GetAxis("Vertical") > 0)
                transform.Translate(Vector3.back * Time.deltaTime * speed, Space.World);


        }
        if (this.transform.position.y < 2.5f || this.transform.position.y > 3f)
        {
            grounded = false;
            airBorn = true;
        }
        if (this.transform.position.y >= 2.5f && this.transform.position.y <= 3f)
        {
            grounded = true;
            if (airBorn)
            {
                this.GetComponent<BoxCollider>().isTrigger = false;
                airBorn = false;
            }

        }
        if (touchingWall)
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.GetComponent<Rigidbody>().AddForce(Vector3.down * 50);
            timeSinceWalled++;
        }
        if (timeSinceWalled == 10)
        {
            touchingWall = false;
            timeSinceWalled++;
            //print("Done Toching wall p2");
        }

    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Card" && collision.GetComponent<CardThrow>().rangeCounter > collision.GetComponent<CardThrow>().maxRange)
        {
            cardsThrown--;
            canCast[collision.GetComponent<CardThrow>().cardNum] = true;
        }
        if (collision.gameObject.tag == "Cliffs")
        {
            if (!touchingWall)
            {
                //print("Player2 CliffHit");
                this.GetComponent<BoxCollider>().isTrigger = false;
                finishDash();
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.GetComponent<Rigidbody>().AddForce(Vector3.down * 100);
                timeSinceWalled = 0;
                touchingWall = true;
                
            }
        }
        if (collision.gameObject.tag != "Cliffs")
        {
            //touchingWall = false;
        }

    }
    private void CardGather()
    {
        newCard = Instantiate(card, this.transform.position, card.transform.rotation);
        newCard.transform.position = new Vector3(newCard.transform.position.x, newCard.transform.position.y - .25f, newCard.transform.position.z);
        newCard.GetComponent<CardThrow>().cardNum = spellSelected;
        cardsThrown++;
        newCardTrail = Instantiate(cardTrail, this.transform.position, card.transform.rotation);
        newCardTrail.transform.position = new Vector3(newCard.transform.position.x, newCard.transform.position.y - .25f, newCard.transform.position.z);
        newCardTrail.GetComponent<CardTrailThrow>().cardTrailTarget = newCard;
        canCast[spellSelected] = false;
    }
    private void Fireball()
    {
        if (spellSecondary[spellSelected] == "")
        {
            newSpell = Instantiate(spellProjectile[0], this.transform.position, spellProjectile[0].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
            newSpell.GetComponent<FireBallThrow>().spellNum = spellSelected;
            //Debug.Log("Basic");
            newSpell.GetComponent<FireBallThrow>().maxRange = baseRange;
            canCast[spellSelected] = false;
            fireBallID++;
            newSpell.GetComponent<FireBallThrow>().fireBallID = fireBallID;
        }
        if (spellSecondary[spellSelected] == "Boom")
        {
            newSpell = Instantiate(spellProjectile[0], this.transform.position, spellProjectile[0].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
            newSpell.GetComponent<FireBallThrow>().spellNum = spellSelected;
            //Debug.Log("Basic");
            newSpell.GetComponent<FireBallThrow>().maxRange = boomBaseRange;
            newSpell.GetComponent<FireBallThrow>().throwSpeed = boomBaseSpeed;
            newSpell.GetComponent<FireBallThrow>().boomSpell = true;
            canCast[spellSelected] = false;
            fireBallID++;
            newSpell.GetComponent<FireBallThrow>().fireBallID = fireBallID;
        }
        if (spellSecondary[spellSelected] == "AOE")
        {
            fireBallID++;
           
            for (int i = 0; i < 5; i++)
            {
                newSpellAOE[i] = Instantiate(spellProjectile[0], this.transform.position, spellProjectile[0].transform.rotation);
                newSpellAOE[i].transform.position = new Vector3(newSpellAOE[i].transform.position.x, newSpellAOE[i].transform.position.y - .25f, newSpellAOE[i].transform.position.z);
                newSpellAOE[i].GetComponent<FireBallThrow>().spellNum = spellSelected;
                newSpellAOE[i].GetComponent<FireBallThrow>().maxRange = aoeRange;
                newSpellAOE[i].GetComponent<FireBallThrow>().AOEspell = true;
                aoeCone(i);
                newSpellAOE[i].GetComponent<FireBallThrow>().transform.LookAt(AOEpoint);
                newSpellAOE[i].GetComponent<FireBallThrow>().fireBallID = fireBallID;
            }
            canCast[spellSelected] = false;
            //
            dashing = true;
            AOEKnockBack = true;
            dashDirection = spellSelected;
            dashAim = new Vector3(player2Aim.transform.position.x, player2Aim.transform.position.y, player2Aim.transform.position.z);
            dashDirectionTime = 75;
            dashingTime = 0;
            transform.LookAt(dashAim);
            if (this.transform.position.y < 2.5)
            {
                rb.AddForce(Vector3.up * waterDashForceUp);
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }
            // COME BACK HERE
            this.GetComponent<BoxCollider>().isTrigger = false;
        }
        if (spellSecondary[spellSelected] == "Range")
        {
            newSpell = Instantiate(spellProjectile[0], this.transform.position, spellProjectile[0].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
            newSpell.GetComponent<FireBallThrow>().spellNum = spellSelected;
            newSpell.GetComponent<FireBallThrow>().maxRange = rangeRange;
            newSpell.GetComponent<FireBallThrow>().throwSpeed = rangeSpeed;
            canCast[spellSelected] = false;
            fireBallID++;
            newSpell.GetComponent<FireBallThrow>().fireBallID = fireBallID;
        }
        if (spellSecondary[spellSelected] == "Dash")
        {
            //Debug.Log("Dash");
            canCast[spellSelected] = false;
            dashing = true;
            dashDirection = spellSelected;
            dashAim = new Vector3(player2Aim.transform.position.x, player2Aim.transform.position.y, player2Aim.transform.position.z);
            dashDirectionTime = 75;
            transform.LookAt(dashAim);
            if (this.transform.position.y < 2.5)
            {
                rb.AddForce(Vector3.up * waterDashForceUp);
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            this.GetComponent<BoxCollider>().isTrigger = true;
            //Debug.Log("Invulnrble Dash");
        }
    }
    private void WindKnockback()
    {

        if (spellSecondary[spellSelected] == "")
        {
            newSpell = Instantiate(spellProjectile[1], this.transform.position, spellProjectile[1].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
            newSpell.GetComponent<WindWaveThrow>().spellNum = spellSelected;
            //Debug.Log("WindWave" + (spellSelected + 1) + " Thrown");
            canCast[spellSelected] = false;
            newSpell.GetComponent<WindWaveThrow>().maxRange = baseRange;
        }
        if (spellSecondary[spellSelected] == "Boom")
        {
            newSpell = Instantiate(spellProjectile[1], this.transform.position, spellProjectile[0].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
            newSpell.GetComponent<WindWaveThrow>().spellNum = spellSelected;
            //Debug.Log("Basic");
            newSpell.GetComponent<WindWaveThrow>().maxRange = boomBaseRange;
            newSpell.GetComponent<WindWaveThrow>().throwSpeed = boomBaseSpeed;
            newSpell.GetComponent<WindWaveThrow>().boomSpell = true;
            canCast[spellSelected] = false;
        }
        else if (spellSecondary[spellSelected] == "AOE")
        {
            for (int i = 0; i < 5; i++)
            {
                newSpellAOE[i] = Instantiate(spellProjectile[1], this.transform.position, spellProjectile[0].transform.rotation);
                newSpellAOE[i].transform.position = new Vector3(newSpellAOE[i].transform.position.x, newSpellAOE[i].transform.position.y - .25f, newSpellAOE[i].transform.position.z);
                newSpellAOE[i].GetComponent<WindWaveThrow>().spellNum = spellSelected;
                newSpellAOE[i].GetComponent<WindWaveThrow>().maxRange = aoeRange;
                newSpellAOE[i].GetComponent<WindWaveThrow>().AOEspell = true;
                aoeCone(i);
                newSpellAOE[i].GetComponent<WindWaveThrow>().transform.LookAt(AOEpoint);
            }
            canCast[spellSelected] = false;
            //
            dashing = true;
            AOEKnockBack = true;
            dashDirection = spellSelected;
            dashAim = new Vector3(player2Aim.transform.position.x, player2Aim.transform.position.y, player2Aim.transform.position.z);
            dashDirectionTime = 75;
            dashingTime = 0;
            transform.LookAt(dashAim); // opposite dash aim
            if (this.transform.position.y < 2.5)
            {
                rb.AddForce(Vector3.up * waterDashForceUp);
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            this.GetComponent<BoxCollider>().isTrigger = true;
        }
        else if (spellSecondary[spellSelected] == "Range")
        {
            newSpell = Instantiate(spellProjectile[1], this.transform.position, spellProjectile[1].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
            newSpell.GetComponent<WindWaveThrow>().spellNum = spellSelected;
            newSpell.GetComponent<WindWaveThrow>().maxRange = rangeRange;
            newSpell.GetComponent<WindWaveThrow>().throwSpeed = rangeSpeed;
            canCast[spellSelected] = false;
        }
        else if (spellSecondary[spellSelected] == "Dash")
        {
            //Debug.Log("Dash");
            canCast[spellSelected] = false;
            dashing = true;
            dashDirection = spellSelected;
            dashAim = new Vector3(player2Aim.transform.position.x, player2Aim.transform.position.y, player2Aim.transform.position.z);
            dashDirectionTime = 75;
            transform.LookAt(dashAim);
            if (this.transform.position.y < 2.5)
            {
                rb.AddForce(Vector3.up * waterDashForceUp);
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            this.GetComponent<BoxCollider>().isTrigger = true;
            //Debug.Log("Invulnrble Dash");

        }

    }
    private void WaterPull()
    {
        if (spellSecondary[spellSelected] == "")
        {
            newSpell = Instantiate(spellProjectile[2], this.transform.position, spellProjectile[2].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
            newSpell.GetComponent<WaterPullThrow>().spellNum = spellSelected;
            //Debug.Log("Basic");
            newSpell.GetComponent<WaterPullThrow>().maxRange = baseRange;
            canCast[spellSelected] = false;
        }
        if (spellSecondary[spellSelected] == "Boom")
        {
            newSpell = Instantiate(spellProjectile[2], this.transform.position, spellProjectile[0].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
            newSpell.GetComponent<WaterPullThrow>().spellNum = spellSelected;
            //Debug.Log("Basic");
            newSpell.GetComponent<WaterPullThrow>().maxRange = boomBaseRange;
            newSpell.GetComponent<WaterPullThrow>().throwSpeed = boomBaseSpeed;
            newSpell.GetComponent<WaterPullThrow>().boomSpell = true;
            canCast[spellSelected] = false;
        }
        if (spellSecondary[spellSelected] == "AOE")
        {
            for (int i = 0; i < 5; i++)
            {
                newSpellAOE[i] = Instantiate(spellProjectile[2], this.transform.position, spellProjectile[2].transform.rotation);
                newSpellAOE[i].transform.position = new Vector3(newSpellAOE[i].transform.position.x, newSpellAOE[i].transform.position.y - .25f, newSpellAOE[i].transform.position.z);
                newSpellAOE[i].GetComponent<WaterPullThrow>().spellNum = spellSelected;
                newSpellAOE[i].GetComponent<WaterPullThrow>().maxRange = aoeRange;
                newSpellAOE[i].GetComponent<WaterPullThrow>().AOEspell = true;
                aoeCone(i);
                newSpellAOE[i].GetComponent<WaterPullThrow>().transform.LookAt(AOEpoint);
            }
            canCast[spellSelected] = false;
            //
            dashing = true;
            AOEKnockBack = true;
            dashDirection = spellSelected;
            dashAim = new Vector3(player2Aim.transform.position.x, player2Aim.transform.position.y, player2Aim.transform.position.z);
            dashDirectionTime = 75;
            dashingTime = 0;
            transform.LookAt(dashAim); // opposite dash aim
            if (this.transform.position.y < 2.5)
            {
                rb.AddForce(Vector3.up * waterDashForceUp);
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            this.GetComponent<BoxCollider>().isTrigger = true;
        }
        if (spellSecondary[spellSelected] == "Range")
        {
            newSpell = Instantiate(spellProjectile[2], this.transform.position, spellProjectile[2].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - .25f, newSpell.transform.position.z);
            newSpell.GetComponent<WaterPullThrow>().spellNum = spellSelected;
            newSpell.GetComponent<WaterPullThrow>().maxRange = rangeRange;
            newSpell.GetComponent<WaterPullThrow>().throwSpeed = rangeSpeed;
            canCast[spellSelected] = false;
        }
        if (spellSecondary[spellSelected] == "Dash")
        {
            //Debug.Log("Dash");
            canCast[spellSelected] = false;
            dashing = true;
            dashDirection = spellSelected;
            dashAim = new Vector3(player2Aim.transform.position.x, player2Aim.transform.position.y, player2Aim.transform.position.z);
            dashDirectionTime = 75;
            transform.LookAt(dashAim);
            if (this.transform.position.y < 2.5)
            {
                rb.AddForce(Vector3.up * waterDashForceUp);
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            this.GetComponent<BoxCollider>().isTrigger = true;
            //Debug.Log("Invulnrble Dash");

        }



    }
    private void EarthQuake()
    {
        if (spellSecondary[spellSelected] == "")
        {
            newSpell = Instantiate(spellProjectile[3], this.transform.position, spellProjectile[0].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - 1f, newSpell.transform.position.z);
            newSpell.GetComponent<EarthQuakeThrow>().spellNum = spellSelected;
            //Debug.Log("Basic");
            newSpell.GetComponent<EarthQuakeThrow>().maxRange = baseRange * 2;
            canCast[spellSelected] = false;
        }
        if (spellSecondary[spellSelected] == "Boom")
        {
            newSpell = Instantiate(spellProjectile[3], this.transform.position, spellProjectile[0].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - 1, newSpell.transform.position.z);
            newSpell.GetComponent<EarthQuakeThrow>().spellNum = spellSelected;
            //Debug.Log("Basic");
            newSpell.GetComponent<EarthQuakeThrow>().maxRange = boomBaseRange * 2;
            newSpell.GetComponent<EarthQuakeThrow>().throwSpeed = boomBaseSpeed / 2;
            newSpell.GetComponent<EarthQuakeThrow>().boomSpell = true;
            canCast[spellSelected] = false;
        }
        if (spellSecondary[spellSelected] == "AOE")
        {
            for (int i = 0; i < 5; i++)
            {
                newSpellAOE[i] = Instantiate(spellProjectile[3], this.transform.position, spellProjectile[0].transform.rotation);
                newSpellAOE[i].GetComponent<EarthQuakeThrow>().spellNum = spellSelected;
                newSpellAOE[i].GetComponent<EarthQuakeThrow>().maxRange = aoeRange * 2;
                aoeCone(i);
                newSpellAOE[i].GetComponent<EarthQuakeThrow>().AOEspell = true;
                newSpellAOE[i].GetComponent<EarthQuakeThrow>().transform.LookAt(AOEpoint);
                newSpellAOE[i].transform.position = new Vector3(newSpellAOE[i].transform.position.x, newSpellAOE[i].transform.position.y - 1f, newSpellAOE[i].transform.position.z);
            }
            canCast[spellSelected] = false;
            //
            dashing = true;
            AOEKnockBack = true;
            dashDirection = spellSelected;
            dashAim = new Vector3(player2Aim.transform.position.x, player2Aim.transform.position.y, player2Aim.transform.position.z);
            dashDirectionTime = 75;
            dashingTime = 0;
            transform.LookAt(new Vector3(player2Aim.transform.position.x, player2Aim.transform.position.y, player2Aim.transform.position.z)); // opposite dash aim
            if (this.transform.position.y < 2.5)
            {
                rb.AddForce(Vector3.up * waterDashForceUp);
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            this.GetComponent<BoxCollider>().isTrigger = true;
            //
        }
        if (spellSecondary[spellSelected] == "Range")
        {
            newSpell = Instantiate(spellProjectile[3], this.transform.position, spellProjectile[0].transform.rotation);
            newSpell.transform.position = new Vector3(newSpell.transform.position.x, newSpell.transform.position.y - 1f, newSpell.transform.position.z);
            newSpell.GetComponent<EarthQuakeThrow>().spellNum = spellSelected;
            newSpell.GetComponent<EarthQuakeThrow>().maxRange = rangeRange;
            newSpell.GetComponent<EarthQuakeThrow>().throwSpeed = rangeSpeed;
            canCast[spellSelected] = false;
        }
        if (spellSecondary[spellSelected] == "Dash")
        {
            //Debug.Log("Dash");
            canCast[spellSelected] = false;
            dashing = true;
            dashDirection = spellSelected;
            dashAim = new Vector3(player2Aim.transform.position.x, player2Aim.transform.position.y, player2Aim.transform.position.z);
            dashDirectionTime = 75;
            transform.LookAt(dashAim);
            if (this.transform.position.y < 2.5)
            {
                rb.AddForce(Vector3.up * waterDashForceUp);
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            this.GetComponent<BoxCollider>().isTrigger = true;
            //Debug.Log("Invulnrble Dash");
        }
    }
    private void aoeCone(int i)
    {
        // I got Really Really Fucking Lazy and Hard Coded the Draw Cricle about point function to make this work. 
        //Im ashamed of the following code and wil fix when i figrue out abetter draw circle - Mark
        if (i == 0)
        {
            AOEpoint.position = player2Aim.transform.position;
        }
        if (spellSelected == 0 || spellSelected == 2)
        {
            if (i == 1)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x + aoeWidth / 2, this.transform.position.y, AOEpoint.transform.position.z);
            }
            if (i == 2)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x + (aoeWidth / 2), this.transform.position.y, AOEpoint.transform.position.z);
            }
            if (i == 3)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x - (aoeWidth * 1.5f), this.transform.position.y, AOEpoint.transform.position.z);
            }
            if (i == 4)
            {
                AOEpoint.position = new Vector3(AOEpoint.transform.position.x - (aoeWidth / 2), this.transform.position.y, AOEpoint.transform.position.z);
            }
        }
        if (spellSelected == 1 || spellSelected == 3)
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

        }
    }
    public void finishDash()
    {
        dashingTime = 0;
        dashing = false;
        AOEKnockBack = false;
        //transform.Translate(Vector3.forward * Time.deltaTime * speed * 3, Space.Self);
        this.GetComponent<BoxCollider>().isTrigger = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        this.transform.rotation = Quaternion.Euler(0, 45, 0);
        playerUI.SetActive(true);
        if (!baseDashing)
        {
            castAfterDash = true;
        }
        baseDashing = false;
    }
}