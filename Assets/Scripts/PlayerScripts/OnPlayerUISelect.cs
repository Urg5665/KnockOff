using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnPlayerUISelect : MonoBehaviour
{
    public bool selected = false;

    public GameObject player;
    public PlayerControl playerControl;
    public int spellNumber;// playerControlls Spell Number
    public int localDirection; // set 0 in unity for north, 1 for east 2 fro south, 3  for west

    public Image image;

    // pie pieces underneath skillshots
    public Sprite white;
    public Sprite red;
    public Sprite cyan;
    public Sprite blue;
    public Sprite brown;

    public GameObject childIcon;
    public GameObject outerRing;
    public GameObject innerRing;

    public Sprite cone;
    public Sprite line;
    public Sprite dash;
    public Sprite boom;

    public Color32 colorInner;
    public Color32 colorOuter;

    public Color32 whiteColor = new Color32(255, 255, 255, 255);
    public Color32 redColor = new Color32(255, 0, 0, 255);
    public Color32 orangeColor = new Color32(255, 165, 0, 255);
    public Color32 yellowColor = new Color32(215, 255, 0, 255);
    public Color32 greenColor = new Color32(50, 205, 50, 255);
    public Color32 greyColor = new Color32(140, 140, 140, 255);

    public Sprite inner1; // 1 is the smaller 2 is the larger
    public Sprite inner2;
    public Sprite outer1;
    public Sprite outer2;

    private void Start()
    {
        playerControl = player.GetComponent<PlayerControl>();
    }

    private void Update()
    {
        if (playerControl.spellSelected == spellNumber)
        {
            image.enabled = false;
            innerRing.GetComponent<Image>().sprite = inner2;
            outerRing.GetComponent<Image>().sprite = outer2;
            outerRing.SetActive(false);
            innerRing.SetActive(false);
            outerRing.GetComponent<Image>().color = whiteColor;
            innerRing.GetComponent<Image>().color = whiteColor;
            colorInner = whiteColor; 
            colorOuter = whiteColor;
            childIcon.GetComponent<Image>().enabled = true;
        }
        if (playerControl.spellPrimary[localDirection] == "Fire")
        {
            image.sprite = red;
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().color = redColor;
            colorInner = redColor;
            childIcon.GetComponent<Image>().color = redColor;
        }
        if (playerControl.spellPrimary[localDirection] == "Earth")
        {
            image.sprite = brown;
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().color = greenColor;
            colorInner = greenColor;
            childIcon.GetComponent<Image>().color = greenColor;
        }

        if (playerControl.spellPrimary[localDirection] == "Inferno")
        {
            //image.sprite = red;
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().color = yellowColor;
            colorInner = yellowColor;
            childIcon.GetComponent<Image>().color = yellowColor;
        }
        if (playerControl.spellPrimary[localDirection] == "Meteor")
        {
            //image.sprite = cyan;
            innerRing.SetActive(true);
            //67, 215, 255, 255
            innerRing.GetComponent<Image>().color = orangeColor;
            colorInner = orangeColor;
            childIcon.GetComponent<Image>().color = orangeColor;
        }
        if (playerControl.spellPrimary[localDirection] == "Mountain")
        {
            image.sprite = cyan;
            innerRing.SetActive(true);
            //67, 215, 255, 255
            innerRing.GetComponent<Image>().color = greyColor;
            colorInner = greyColor;
            childIcon.GetComponent<Image>().color = greyColor;
        }
        if (playerControl.spellPrimary[localDirection] == "Wind")
        {
            image.sprite = cyan;
            innerRing.SetActive(true);
            //67, 215, 255, 255
            innerRing.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
            colorInner = new Color32(67, 215, 255, 255);
            childIcon.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
        }
        if (playerControl.spellPrimary[localDirection] == "Water")
        {
            image.sprite = blue;
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
            colorInner = new Color32(0, 0, 255, 255);
            childIcon.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
        }

        if (playerControl.spellPrimary[localDirection] == "")
        {
            image.sprite = white;
            outerRing.SetActive(false);
            innerRing.SetActive(false);
        }

        if (playerControl.spellSecondary[localDirection] == "AOE")
        {
            outerRing.SetActive(true);
            outerRing.GetComponent<Image>().color = new Color32(255, 0, 0, 255);

            colorOuter = new Color32(255, 0, 0, 255);
            childIcon.GetComponent<Image>().sprite = cone;
            childIcon.GetComponent<Image>().enabled = false;
        }
        if (playerControl.spellSecondary[localDirection] == "Range")
        {
            //outerRing.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
            //colorOuter = new Color32(67, 215, 255, 255);
            outerRing.SetActive(true);
            outerRing.GetComponent<Image>().color = new Color32(50, 205, 50, 255);
            colorOuter = new Color32(50, 205, 50, 255);
            childIcon.GetComponent<Image>().sprite = line;
            childIcon.GetComponent<Image>().enabled = false;
        }
        if (playerControl.spellSecondary[localDirection] == "Dash")
        {
            outerRing.SetActive(true);
            outerRing.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
            colorOuter = new Color32(0, 0, 255, 255);
            childIcon.GetComponent<Image>().sprite = dash;
            childIcon.GetComponent<Image>().enabled = false;
        }
        if (playerControl.spellSecondary[localDirection] == "Boom")
        {
            outerRing.SetActive(true);
            childIcon.GetComponent<Image>().sprite = boom;
            outerRing.GetComponent<Image>().color = new Color32(50, 205, 50, 255);
        colorOuter = new Color32(90, 80, 0, 255);
            childIcon.GetComponent<Image>().enabled = false;
        }
        if (playerControl.spellSecondary[localDirection] == "" || playerControl.spellSecondary[localDirection] == "None")
        {
            outerRing.SetActive(false);
            childIcon.GetComponent<Image>().sprite = null;
            childIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            colorOuter = new Color32(255, 255, 255, 0);
            childIcon.GetComponent<Image>().enabled = false;
        }
        if (playerControl.spellPrimary[localDirection] == "")
        {
        innerRing.SetActive(false);
        childIcon.GetComponent<Image>().sprite = null;
        childIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        colorInner = new Color32(255, 255, 255, 0);
            childIcon.GetComponent<Image>().enabled = false;
        }

        if (playerControl.spellSelected != spellNumber)
        {
            image.enabled = false;
            childIcon.GetComponent<Image>().enabled = false;
            innerRing.GetComponent<Image>().sprite = inner1;
            outerRing.GetComponent<Image>().sprite = outer1;
            innerRing.GetComponent<Image>().color = new Color32(colorInner.r, colorInner.g, colorInner.b, 160);
            outerRing.GetComponent<Image>().color = new Color32(colorOuter.r, colorOuter.g, colorOuter.b, 160);

        }
    }
}