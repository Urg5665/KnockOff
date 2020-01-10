using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillShotRotate : MonoBehaviour
{
    public GameObject playerBelong;
    public GameObject playerAim;

    public PlayerControl playerControl;
    public PlayerControlXbox playerControlXbox;

    public GameObject cardSkillShot;

    public GameObject meleeSkillShot;

    public GameObject baseSkillShot;
    public GameObject aoeSkillShot;
    public GameObject rangeSkillShot;
    public GameObject dashSkillShot;
    public GameObject boomSkillShot;

    public GameObject meteorSkillShot;
    public GameObject mountainSkillShot;

    public float xPos;
    public float zPos;
    public float zDif;
    public float xDif;
    public float hypo;
    public float angle;
    public float angle2;

    public Color32 whiteColor = new Color32(255, 255, 255, 255);
    public Color32 redColor = new Color32(255, 0, 0, 255);
    public Color32 orangeColor = new Color32(255, 165, 0, 255);
    public Color32 yellowColor = new Color32(215, 255, 0, 255);
    public Color32 greenColor = new Color32(50, 205, 50, 255);
    public Color32 greyColor = new Color32(140, 140, 140, 255);

    private void Start()
    {
        if (playerBelong.name == "Player1")
        {
            playerControl = playerBelong.GetComponent<PlayerControl>();
        }
        if (playerBelong.name == "Player2")
        {
            playerControlXbox = playerBelong.GetComponent<PlayerControlXbox>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        SkillShotUIUpdate(); // Visual Updates
        xPos = this.transform.position.x - playerAim.transform.position.x;
        zPos = this.transform.position.z - playerAim.transform.position.z;
        xDif = Mathf.Abs(this.transform.position.x - playerAim.transform.position.x);
        zDif = Mathf.Abs(this.transform.position.z - playerAim.transform.position.z);

        hypo = Mathf.Sqrt((xDif * xDif + zDif * zDif));

        angle = Mathf.Rad2Deg * (Mathf.Asin(zDif / hypo));
        
        if (angle > 45) // north south
        {
            if (zPos > 0) // South
            {
                if (xPos > 0)
                {
                    angle2 = 270 - Mathf.Rad2Deg * (Mathf.Asin(xDif / hypo));
                }
                if (xPos <= 0)
                {
                    angle2 = 270 + Mathf.Rad2Deg * (Mathf.Asin(xDif / hypo));
                }
            }
            if (zPos <= 0) 
            {
                if (xPos > 0)
                {
                    angle2 = 90 + Mathf.Rad2Deg * (Mathf.Asin(xDif / hypo));
                }
                if (xPos <= 0)
                {
                    angle2 = 90 - Mathf.Rad2Deg * (Mathf.Asin(xDif / hypo));
                }
            }
        }
        else if (angle <= 45) // east west
        {
            if (xPos > 0)
            {
                if (zPos > 0)
                {
                    angle2 = 180 + Mathf.Rad2Deg * (Mathf.Asin(zDif / hypo));
                }
                if (zPos <= 0)
                {
                    angle2 = 180 - Mathf.Rad2Deg * (Mathf.Asin(zDif / hypo));
                }
            }
            if (xPos <= 0)
            {
                if (zPos > 0)
                {
                    angle2 = 360 - Mathf.Rad2Deg * (Mathf.Asin(zDif / hypo));
                }
                if (zPos <= 0)
                {
                    angle2 = Mathf.Rad2Deg * (Mathf.Asin(zDif / hypo));
                }

            }
        }

        transform.localEulerAngles = new Vector3(0, 0, angle2);
    }

    private void SkillShotUIUpdate()
    {
        if (playerBelong.name == "Player1")
        {
            if (playerControl.spellPrimary[playerControl.spellSelected] == "")
            {
                cardSkillShot.SetActive(false);

                mountainSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
            }  

            if (playerControl.spellPrimary[playerControl.spellSelected] != "" && playerControl.spellSecondary[playerControl.spellSelected] == "") // aplies base spell
            {
                cardSkillShot.SetActive(true);

                baseSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
            }
            if (playerControl.spellPrimary[playerControl.spellSelected] == "Fire") // aaplies base spell and color
            {
                cardSkillShot.GetComponent<Image>().color = redColor;
                aoeSkillShot.GetComponent<Image>().color = redColor;
                rangeSkillShot.GetComponent<Image>().color = redColor;
                dashSkillShot.GetComponent<Image>().color = redColor;
                boomSkillShot.GetComponent<Image>().color = redColor;
                cardSkillShot.SetActive(true);
                baseSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
                mountainSkillShot.SetActive(false);
            }
            if (playerControl.spellPrimary[playerControl.spellSelected] == "Earth") // aaplies base spell ccolor
            {
                cardSkillShot.GetComponent<Image>().color = greenColor;
                aoeSkillShot.GetComponent<Image>().color = greenColor;
                rangeSkillShot.GetComponent<Image>().color = greenColor;
                dashSkillShot.GetComponent<Image>().color = greenColor;
                boomSkillShot.GetComponent<Image>().color = greenColor;
                cardSkillShot.SetActive(true);
                baseSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
                mountainSkillShot.SetActive(false);

            }
            if (playerControl.spellPrimary[playerControl.spellSelected] == "Inferno") // aaplies base spell and color
            {
                cardSkillShot.GetComponent<Image>().color = yellowColor;
                aoeSkillShot.GetComponent<Image>().color = yellowColor;
                rangeSkillShot.GetComponent<Image>().color = yellowColor;
                dashSkillShot.GetComponent<Image>().color = yellowColor;
                boomSkillShot.GetComponent<Image>().color = yellowColor;
                cardSkillShot.SetActive(true);
                baseSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
                mountainSkillShot.SetActive(false);

            }
            if (playerControl.spellPrimary[playerControl.spellSelected] == "Meteor") // aaplies base spell ccolor
            {
                cardSkillShot.GetComponent<Image>().color = orangeColor;
                aoeSkillShot.GetComponent<Image>().color = orangeColor;
                rangeSkillShot.GetComponent<Image>().color = orangeColor;
                dashSkillShot.GetComponent<Image>().color = orangeColor;
                boomSkillShot.GetComponent<Image>().color = orangeColor;
                meteorSkillShot.GetComponent<Image>().color = orangeColor;
                cardSkillShot.SetActive(false);
                baseSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
                meteorSkillShot.SetActive(true);
                mountainSkillShot.SetActive(false);

            }
            if (playerControl.spellPrimary[playerControl.spellSelected] == "Mountain") // aaplies base spell ccolor
            {
                cardSkillShot.GetComponent<Image>().color = greyColor;
                aoeSkillShot.GetComponent<Image>().color = greyColor;
                rangeSkillShot.GetComponent<Image>().color = greyColor;
                dashSkillShot.GetComponent<Image>().color = greyColor;
                boomSkillShot.GetComponent<Image>().color = greyColor;
                mountainSkillShot.GetComponent<Image>().color = greyColor;
                cardSkillShot.SetActive(false);
                baseSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
                mountainSkillShot.SetActive(true);

            }
            if (playerControl.spellPrimary[playerControl.spellSelected] == "Water") // aaplies base spell ccolor
            {
                cardSkillShot.GetComponent<Image>().color = Color.blue;
                aoeSkillShot.GetComponent<Image>().color = Color.blue;
                rangeSkillShot.GetComponent<Image>().color = Color.blue;
                dashSkillShot.GetComponent<Image>().color = Color.blue;
                boomSkillShot.GetComponent<Image>().color = Color.blue;
            }
            /*
            if (playerControl.spellPrimary[playerControl.spellSelected] == "Wind") // aaplies base spell and color
            {
            cardSkillShot.GetComponent<Image>().color = new Color32(67, 215, 255, 255); 
            aoeSkillShot.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
            rangeSkillShot.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
            dashSkillShot.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
            boomSkillShot.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
            }


            if (playerControl.spellSecondary[playerControl.spellSelected] == "Range")
            {
                rangeSkillShot.SetActive(true);

                meleeSkillShot.SetActive(false);
                baseSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                cardSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
            }
            else if (playerControl.spellSecondary[playerControl.spellSelected] == "Boom")
            {
                
                boomSkillShot.SetActive(true);

                meleeSkillShot.SetActive(false);
                baseSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                cardSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
            }
            else if (playerControl.spellSecondary[playerControl.spellSelected] == "AOE")
            {
                aoeSkillShot.SetActive(true);

                meleeSkillShot.SetActive(false);
                baseSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                cardSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
            }
            else if (playerControl.spellSecondary[playerControl.spellSelected] == "Dash")
            {
                dashSkillShot.SetActive(true);

                meleeSkillShot.SetActive(false);
                baseSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                cardSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
            }

            if (playerControl.spellSecondary[playerControl.spellSelected] != "") // Full Spell
            {
                cardSkillShot.SetActive(true);
                meleeSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
                mountainSkillShot.SetActive(false);
            }
            */
            if (playerControl.spellPrimary[playerControl.spellSelected] == "") // Reset Spell Completely
            {
                cardSkillShot.SetActive(false);
                cardSkillShot.GetComponent<Image>().color = new Color32(255,255,255, 255);
                baseSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
                mountainSkillShot.SetActive(false);
            }
        }
        if (playerBelong.name == "Player2")
        {
            if (playerControlXbox.spellPrimary[playerControlXbox.spellSelected] == "")
            {
                cardSkillShot.SetActive(false);
                meleeSkillShot.SetActive(true);
                mountainSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
            }
            if (playerControlXbox.spellPrimary[playerControlXbox.spellSelected] != "" && playerControlXbox.spellSecondary[playerControlXbox.spellSelected] == "") // aaplies base spell
            {
                cardSkillShot.SetActive(true);
                meleeSkillShot.SetActive(true);
                baseSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
            }
            if (playerControlXbox.spellPrimary[playerControlXbox.spellSelected] == "Fire") // aaplies base spell and color
            {
                cardSkillShot.GetComponent<Image>().color = redColor;
                aoeSkillShot.GetComponent<Image>().color = redColor;
                rangeSkillShot.GetComponent<Image>().color = redColor;
                dashSkillShot.GetComponent<Image>().color = redColor;
                boomSkillShot.GetComponent<Image>().color = redColor;
                mountainSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
                cardSkillShot.SetActive(true);
            }
            if (playerControlXbox.spellPrimary[playerControlXbox.spellSelected] == "Earth") // aaplies base spell ccolor
            {
                cardSkillShot.GetComponent<Image>().color = greenColor;
                aoeSkillShot.GetComponent<Image>().color = greenColor;
                rangeSkillShot.GetComponent<Image>().color = greenColor;
                dashSkillShot.GetComponent<Image>().color = greenColor;
                boomSkillShot.GetComponent<Image>().color = greenColor;
                mountainSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
                cardSkillShot.SetActive(true);

            }
            if (playerControlXbox.spellPrimary[playerControlXbox.spellSelected] == "Inferno") // aaplies base spell and color
            {
                cardSkillShot.GetComponent<Image>().color = yellowColor;
                aoeSkillShot.GetComponent<Image>().color = yellowColor;
                rangeSkillShot.GetComponent<Image>().color = yellowColor;
                dashSkillShot.GetComponent<Image>().color = yellowColor;
                boomSkillShot.GetComponent<Image>().color = yellowColor;
                meleeSkillShot.SetActive(false);
                mountainSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
                cardSkillShot.SetActive(true);
            }
            if (playerControlXbox.spellPrimary[playerControlXbox.spellSelected] == "Meteor") // aaplies base spell ccolor
            {
                cardSkillShot.GetComponent<Image>().color = orangeColor;
                aoeSkillShot.GetComponent<Image>().color = orangeColor;
                rangeSkillShot.GetComponent<Image>().color = orangeColor;
                dashSkillShot.GetComponent<Image>().color = orangeColor;
                boomSkillShot.GetComponent<Image>().color = orangeColor;
                meteorSkillShot.GetComponent<Image>().color = orangeColor;
                mountainSkillShot.SetActive(false);
                meteorSkillShot.SetActive(true);
                cardSkillShot.SetActive(false);
                meleeSkillShot.SetActive(false);

            }
            if (playerControlXbox.spellPrimary[playerControlXbox.spellSelected] == "Mountain") // aaplies base spell ccolor
            {
                cardSkillShot.GetComponent<Image>().color = greyColor;
                aoeSkillShot.GetComponent<Image>().color = greyColor;
                rangeSkillShot.GetComponent<Image>().color = greyColor;
                dashSkillShot.GetComponent<Image>().color = greyColor;
                boomSkillShot.GetComponent<Image>().color = greyColor;
                mountainSkillShot.GetComponent<Image>().color = greyColor;
                mountainSkillShot.SetActive(true);
                meteorSkillShot.SetActive(false);
                cardSkillShot.SetActive(false);
                meleeSkillShot.SetActive(false);

            }
            if (playerControlXbox.spellPrimary[playerControlXbox.spellSelected] == "Wind") // aaplies base spell and color
            {
                cardSkillShot.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
                aoeSkillShot.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
                rangeSkillShot.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
                dashSkillShot.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
                boomSkillShot.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
            }
            if (playerControlXbox.spellPrimary[playerControlXbox.spellSelected] == "Water") // aaplies base spell ccolor
            {
                cardSkillShot.GetComponent<Image>().color = Color.blue;
                aoeSkillShot.GetComponent<Image>().color = Color.blue;
                rangeSkillShot.GetComponent<Image>().color = Color.blue;
                dashSkillShot.GetComponent<Image>().color = Color.blue;
                boomSkillShot.GetComponent<Image>().color = Color.blue;

            }
            /*
            if (playerControlXbox.spellSecondary[playerControlXbox.spellSelected] == "Range")
            {
                rangeSkillShot.SetActive(true);

                meleeSkillShot.SetActive(false);
                baseSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                cardSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
            }
            else if (playerControlXbox.spellSecondary[playerControlXbox.spellSelected] == "AOE")
            {
                aoeSkillShot.SetActive(true);

                meleeSkillShot.SetActive(false);
                baseSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                cardSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
            }
            else if (playerControlXbox.spellSecondary[playerControlXbox.spellSelected] == "Dash")
            {
                dashSkillShot.SetActive(true);

                meleeSkillShot.SetActive(false);
                baseSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                cardSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
            }
            else if (playerControlXbox.spellSecondary[playerControlXbox.spellSelected] == "Boom")
            {
                boomSkillShot.SetActive(true);

                meleeSkillShot.SetActive(false);
                baseSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                cardSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
            }
            
            if (playerControlXbox.spellSecondary[playerControlXbox.spellSelected] != "") // Full Spell
            {
                cardSkillShot.SetActive(true);
            }
            */
            if (playerControlXbox.spellPrimary[playerControlXbox.spellSelected] == "") // Reset Spell Completely
            {
                meleeSkillShot.SetActive(true);
                cardSkillShot.SetActive(false);
                cardSkillShot.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                baseSkillShot.SetActive(false);
                aoeSkillShot.SetActive(false);
                rangeSkillShot.SetActive(false);
                dashSkillShot.SetActive(false);
                boomSkillShot.SetActive(false);
                mountainSkillShot.SetActive(false);
                meteorSkillShot.SetActive(false);
            }
        }
    }
    
}


