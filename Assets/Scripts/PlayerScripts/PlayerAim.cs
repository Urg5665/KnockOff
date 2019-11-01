using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform parent;
    public GameObject player;
    public PlayerControl playerControl;

    public float angle;

    public float xPos;
    public float zPos;
    public float zDif;
    public float xDif;
    public float hypo;

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

            //Debug.Log(angle);

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
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                this.transform.position = hit.point;

            }

            this.transform.position = new Vector3(this.transform.position.x, parent.transform.position.y, this.transform.position.z);
        }
    }

}

