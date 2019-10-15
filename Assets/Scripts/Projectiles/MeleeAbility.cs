using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbility : MonoBehaviour
{
    public float rotSpeed;
    public float throwSpeed;
    public int playerInt;
    public int swordNum;

    public GameObject player;
    public GameObject playerAim;
    public PlayerControl playerControl;
    public PlayerControlXbox playerControlXbox;

    public BoxCollider swordCollider;

    public string resType;
    public string resType2;
    public bool hitRes;
    public bool resAssigned;

    public float swordForce;
    public float swordKnockUp;

    public int rangeCounter;
    public int maxRange;
    public GameObject target;
    // Start is called before the first frame update
    private void Awake()
    {
        if (playerInt == 1)
        {
            player = GameObject.Find("Player1");
            playerAim = player.transform.GetChild(0).gameObject;
            playerControl = player.GetComponent<PlayerControl>();
            swordNum = playerControl.spellSelected;
        }
        if (playerInt == 2)
        {
            player = GameObject.Find("Player2");
            playerAim = player.transform.GetChild(0).gameObject;
            playerControlXbox = player.GetComponent<PlayerControlXbox>();
            swordNum = playerControlXbox.spellSelected;
        }
        maxRange = 10;
        rangeCounter = 0;
        hitRes = false;
        resAssigned = false;
        swordForce = 500;
        swordKnockUp = 200;
    }


    // Update is called once per frame
    void Update()
    {
        rangeCounter++;
        transform.LookAt(player.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, throwSpeed * 2);
        transform.position = new Vector3(transform.position.x, transform.position.y , transform.position.z);
        if (playerInt == 1)
        {
            if (rangeCounter > maxRange)
            {
                Destroy(this.gameObject);
                playerControl.meleeGathering = false;
            }
            if (hitRes)
            {
                if (playerControl.spellPrimary[swordNum] != "" && !resAssigned)
                {
                    playerControl.spellSecondary[swordNum] = resType2;
                    resAssigned = true;
                    hitRes = false;
                }
                else if (!resAssigned)
                {
                    playerControl.spellPrimary[swordNum] = resType;
                    resAssigned = true;
                    hitRes = false;
                }
                playerControl.canCast[swordNum] = true;
                playerControl.cardsThrown--;
            }
        }
        if (playerInt == 2)
        {
            if (rangeCounter > maxRange)
            {
                Destroy(this.gameObject);
                playerControlXbox.meleeGathering = false;
            }
            if (hitRes)
            {
                if (playerControlXbox.spellPrimary[swordNum] != "" && !resAssigned)
                {
                    playerControlXbox.spellSecondary[swordNum] = resType2;
                    resAssigned = true;
                    hitRes = false;
                }
                else if (!resAssigned)
                {
                    playerControlXbox.spellPrimary[swordNum] = resType;
                    resAssigned = true;
                    hitRes = false;
                }
                playerControlXbox.canCast[swordNum] = true;
                playerControlXbox.cardsThrown--;
            }
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "fireRes")
        {
            resType = "Fire";
            resType2 = "AOE";
            Destroy(collision.gameObject);
            hitRes = true;
        }
        if (collision.gameObject.tag == "windRes")
        {
            resType = "Wind";
            resType2 = "Range";
            hitRes = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "waterRes")
        {
            resType = "Water";
            resType2 = "Dash";
            Destroy(collision.gameObject);
            hitRes = true;
        }
        if (collision.gameObject.tag == "earthRes")
        {
            resType = "Earth";
            //Debug.Log("Boom Picked");
            resType2 = "Range";
            Destroy(collision.gameObject);
            hitRes = true;
        }

        if (playerInt == 1 && collision.gameObject.tag == "Player2")
        {
            collision.gameObject.GetComponent<PlayerControlXbox>().finishDash();
            collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * -1 * swordForce); // Knock Back
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * swordKnockUp); // Knock Up
            collision.GetComponent<BoxCollider>().isTrigger = true;
            Debug.Log("hit Player 2");

        }
        if (playerInt == 2 && collision.gameObject.tag == "Player1")
        {
            collision.gameObject.GetComponent<PlayerControl>().finishDash();
            collision.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * - 1 * swordForce); // Knock Back
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * swordKnockUp); // Knock Up
            collision.GetComponent<BoxCollider>().isTrigger = true;

        }

    }
}
