using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavoir : MonoBehaviour
{
    public bool destroyed;
    public int destroyTimer;
    public int destroyLength; // The Maximum Time it remians destroyed

    public bool raised;
    public int raisedTimer;
    public int raisedLength; // The Maximum Time it remians raised


    public MeshRenderer mesh;
    public MeshCollider col;

    public int player1Score;
    public int player2Score;

    public DeathPlane deathPlane;

    // Start is called before the first frame update
    void Start()
    {
        //mesh = this.GetComponentInParent<MeshRenderer>();
        //col = this.GetComponent<MeshCollider>();
        mesh = this.GetComponent<MeshRenderer>();
        col = this.GetComponentInChildren<MeshCollider>();
        destroyed = false;
        destroyTimer = 0;
        player1Score = 3;
        player2Score = 3;
        deathPlane = GameObject.Find("DeathPlane").GetComponent<DeathPlane>();
        destroyLength = 200;
        col.isTrigger = false;
        raised = false;
        raisedTimer = 0;
        raisedLength = 5;
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "earthQuake")
        {
            Debug.Log(this.gameObject.name + " was Destroyed");
            //destroyed = true;
        }
    }

    void FixedUpdate()
    {
        if(player1Score != deathPlane.player1Score || player2Score != deathPlane.player2Score)
        {
            //Debug.Log(player2Score + "  " + deathPlane.player2Score);

            destroyed = false;
            raised = false;
            this.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            col.enabled = true;
            mesh.enabled = true;
            col.isTrigger = false;
            destroyTimer = 0;
            player1Score = deathPlane.player1Score;
            player2Score = deathPlane.player2Score;
            //this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            //destroyed = false;
            //destroyTimer += 5;
        }

        if (Input.GetKey(KeyCode.H))
        {
            destroyed = true;
        }

        if (destroyed)
        {
            col.enabled = false;
            col.isTrigger = false;
            destroyTimer++;
            double destroyTimerFloat = (double)destroyTimer;
            if (destroyTimer > 0 && destroyTimer < destroyLength)
            {
                
                this.transform.position = new Vector3(transform.position.x,  - destroyTimer/2, transform.position.z);
            }
            /*
            else if (destroyTimer >= destroyLength)
            {
                //Debug.Log("Rising");
                this.transform.position = new Vector3(transform.position.x, 1.04f + (- (destroyLength+ 50) + destroyTimer), transform.position.z);
            }*/
        }
        if (raised)
        {
            col.isTrigger = true;
            raisedTimer++;
            double raisedTimerFloat = (double)raisedTimer;
            if (raisedTimer > 0 && raisedTimer < raisedLength)
            {

                this.transform.position = new Vector3(transform.position.x, raisedTimer / 2, transform.position.z);
            }
            /*
            else if (raisedTimer >= raisedLength * 80)
            {
                //Debug.Log("Rising");
                this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                raised = false;
            }*/
        }



        /*
        if (destroyTimer > destroyLength + 50)
        {
            destroyed = false;
            destroyTimer = 0;
            Debug.Log(destroyTimer + " / " + destroyLength);
            mesh.enabled = true;
            col.enabled = true;
            this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }*/
        /*
        if (!destroyed)
        {
            player1Score = deathPlane.player1Score;
            player2Score = deathPlane.player2Score;
        }*/
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            if (raised)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.gameObject.transform.forward * 500 * -1);
            }
        }
    }

}

