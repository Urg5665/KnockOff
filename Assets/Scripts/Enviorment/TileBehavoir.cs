using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavoir : MonoBehaviour
{
    public Vector3 defaultPosition;

    public Material defaultGround;
    public Material fireGround;

    public Material[] mats;

    public bool shattered; // Pre Requisite to destoryed
    public int shatterTime;
    public int shatterLength; // Max time before it is destroyed
    public float magnitude;

    public bool destroyed;
    public int destroyTimer;
    public int destroyLength; // The Maximum Time it remians destroyed

    public bool inflamed;
    public int inflamedTime;
    public int inflamedLength;

    public bool raised;
    public bool rising;
    public int risingTimer;
    public int risingLength; // The Maximum Time it remians raised

    public int raisedTimer;
    public int raisedLength;


    public MeshRenderer mesh;
    public MeshCollider col;

    public int player1Score;
    public int player2Score;

    public DeathPlane deathPlane;
    public Vector3 mountainDir;

    // Start is called before the first frame update
    void Start()
    {
        //mesh = this.GetComponentInParent<MeshRenderer>();
        //col = this.GetComponent<MeshCollider>();
        mesh = this.GetComponent<MeshRenderer>();
        col = this.GetComponentInChildren<MeshCollider>();
        defaultPosition = this.transform.position;
        destroyed = false;
        destroyTimer = 0;
        player1Score = 3;
        player2Score = 3;
        deathPlane = GameObject.Find("DeathPlane").GetComponent<DeathPlane>();
        destroyLength = 200;
        col.isTrigger = false;
        raised = false;
        rising = false;
        risingTimer = 0;
        risingLength = 7;
        raisedLength = 200;
        raisedTimer = 0;
        shattered = false;
        shatterLength = 100;
        shatterTime = 0;
        magnitude = 0.5f;
        inflamedLength = 200;
        inflamedTime = 0;
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "earthQuake")
        {
            //Debug.Log(this.gameObject.name + " was Destroyed");
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
            inflamed = false;
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
           shattered = true;
        }
        if (inflamed)
        {
            inflamedTime++;
            if(inflamedTime < inflamedLength)
            {
                mats = GetComponent<Renderer>().materials;
                mats[0] = fireGround;
                GetComponent<Renderer>().materials = mats;
            }
            else
            {
                inflamed = false;
                inflamedTime = 0;
                mats = GetComponent<Renderer>().materials;
                mats[0] = defaultGround;
                GetComponent<Renderer>().materials = mats;
            }
        }

        if (shattered)
        {
            shatterTime++;
            magnitude += 0.02f;
            if (shatterTime < shatterLength)
            {
                float x = Random.Range(-.1f, .1f) * magnitude;
                float y = Random.Range(-.1f, .1f) * magnitude;
                float z = Random.Range(-.1f, .1f) * magnitude;
                transform.position = new Vector3(defaultPosition.x + x, defaultPosition.y + y, defaultPosition.z + z);

            }
            else
            {
                this.transform.position = defaultPosition;
                shattered = false;
                destroyed = true;
                shatterTime = 0;
                magnitude = 0.5f;
            }
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
        if (rising)
        {
            inflamed = false;
            inflamedTime = 0;
            shattered = false;
            shatterTime = 0;
            col.isTrigger = true;
            risingTimer++;
            double risingTimerFloat = (double)risingTimer;
            if (risingTimer > 0 && risingTimer < risingLength)
            {
                this.transform.position = new Vector3(transform.position.x, risingTimer / 2, transform.position.z);
            }
            else
            {
                //Debug.Log("Rising to Raised at " + risingTimer);
                raised = true;
                rising = false;
            }
        }
        if (raised)
        {
            mats = GetComponent<Renderer>().materials;
            mats[0] = defaultGround;
            GetComponent<Renderer>().materials = mats;
            raisedTimer++;
            col.isTrigger = false;
            if (raisedTimer >= raisedLength && raisedTimer < raisedLength * 2)
            {
                Vector3.MoveTowards(this.transform.position, defaultPosition, 2 * Time.deltaTime);              
            }
            if (raisedTimer >= raisedLength * 2)
            {
                raised = false;
                raisedTimer = 0;
                this.transform.position = defaultPosition;
            }
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

