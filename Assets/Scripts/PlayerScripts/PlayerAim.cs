using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform parent;
    public GameObject player;
    public PlayerControl playerControl;

    public float angle; // For Quad System
    public float meshAngle; // For Triad System

    public float xPos;
    public float zPos;
    public float zDif;
    public float xDif;
    public float hypo;

    public GameObject playerMesh; // Using this to get a relative rotation vector for player aim could be problamatic if animtions are messing with the forward axis of this object

    public int spellSelected;

    public void Update()
    {
        if (playerControl.canRotate)
        {
            xPos = this.transform.position.x - parent.position.x;
            zPos = this.transform.position.z - parent.position.z;
            xDif = Mathf.Abs(this.transform.position.x - parent.position.x);
            zDif = Mathf.Abs(this.transform.position.z - parent.position.z);

            hypo = Mathf.Sqrt((xDif * xDif + zDif * zDif));

            angle = Mathf.Rad2Deg * (Mathf.Asin(zDif / hypo));
            //angle = Mathf.Atan2(xDif,zDif) * Mathf.Rad2Deg;
            meshAngle = playerMesh.GetComponent<Transform>().eulerAngles.y;

            //Debug.Log(angle + "  " +spellSelected);
            //print(meshAngle);
            if (playerControl.usingTriad)
            {
                TriadSystem();
            }
            else
            {
                QuadSystem();
            }


            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                this.transform.position = hit.point;

            }

            this.transform.position = new Vector3(this.transform.position.x, parent.transform.position.y, this.transform.position.z);
        }
    }
    public void QuadSystem()
    {
        if (angle > 45) // north south
        {
            if (zPos > 0)
            {
                //Debug.Log("P1 North");
                spellSelected = 0;
            }
            if (zPos <= 0)
            {
                //Debug.Log("P1 South");
                spellSelected = 2;
            }
        }
        else if (angle <= 45) // east west
        {
            if (xPos > 0)
            {
                //Debug.Log("P1 East");
                spellSelected = 1;
            }
            if (xPos <= 0)
            {
                //Debug.Log("P1 West");
                spellSelected = 3;
            }
        }
    }
    public void TriadSystem()
    {
        if (meshAngle >= 0 && meshAngle < 120)
        {
            Debug.Log("P1 Triad 1");
            spellSelected = 0;
        }
        else if (meshAngle >= 120 && meshAngle < 240)
        {
            Debug.Log("P1 Triad 2");
            spellSelected = 1;
        }
        else if (meshAngle >= 240)
        {
            //Debug.Log("P1 Triad 3");
            spellSelected = 2;
        }

    }

}

