using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnPlayerUISelectXbox : MonoBehaviour
{
    public bool selected = false;

    public GameObject player;
    public GameObject playerAim;
    public PlayerControlXbox playerControlXbox;
    public int spellNumber;
    public int localDirection; // set 0 in unity for north, 1 for east 2 fro south, 3  for west

    public Image image;

    public Sprite white;
    public Sprite red;
    public Sprite cyan;
    public Sprite blue;

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
    public Color32 greyColor = new Color32(80, 80, 80, 255);

    private void Start()
    {
        playerControlXbox = player.GetComponent<PlayerControlXbox>();
        image.enabled = false;
        outerRing.SetActive(true);
        innerRing.SetActive(true);
        outerRing.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        innerRing.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    private void Update()
    {
        if (playerControlXbox.spellSelected == spellNumber)
        {
            image.enabled = false;
            outerRing.SetActive(false);
            innerRing.SetActive(false);
            childIcon.GetComponent<Image>().enabled = true;
            outerRing.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            innerRing.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        if (playerControlXbox.spellPrimary[localDirection] == "Fire")
        {
            image.sprite = red;
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            colorInner = new Color32(255, 0, 0, 255);
            childIcon.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }
        if (playerControlXbox.spellPrimary[localDirection] == "Inferno")
        {
            image.sprite = red;
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().color = yellowColor;
            colorInner = yellowColor;
            childIcon.GetComponent<Image>().color = yellowColor;
        }
        if (playerControlXbox.spellPrimary[localDirection] == "Meteor")
        {
            image.sprite = red;
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().color = orangeColor;
            colorInner = orangeColor;
            childIcon.GetComponent<Image>().color = orangeColor;
        }
        if (playerControlXbox.spellPrimary[localDirection] == "Mountain")
        {
            image.sprite = red;
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().color = greyColor;
            colorInner = greyColor;
            childIcon.GetComponent<Image>().color = greyColor;
        }

        if (playerControlXbox.spellPrimary[localDirection] == "Wind")
        {
            image.sprite = cyan;
            //67, 215, 255, 255
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
            colorInner = new Color32(67, 215, 255, 255);
            childIcon.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
        }
        if (playerControlXbox.spellPrimary[localDirection] == "Water")
        {
            image.sprite = blue;
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
            colorInner = new Color32(0, 0, 255, 255);
            childIcon.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
        }
        if (playerControlXbox.spellPrimary[localDirection] == "Earth")
        {
            //image.sprite = brown;
            //image.sprite = brown;
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().color = new Color32(50, 205, 50, 255);
            colorInner = new Color32(50, 205, 50, 255);
            childIcon.GetComponent<Image>().color = new Color32(50, 205, 50, 255);
        }
        if (playerControlXbox.spellPrimary[localDirection] == "")
        {
            image.sprite = white;
            innerRing.SetActive(false);
        }

        if (playerControlXbox.spellSecondary[localDirection] == "AOE")
        {
            //outerRing.SetActive(true);
            outerRing.GetComponent<Image>().color = new Color32(255, 0, 0, 255);

            colorOuter = new Color32(255, 0, 0, 255);
            childIcon.GetComponent<Image>().sprite = cone;
            childIcon.GetComponent<Image>().sprite = null;
            childIcon.GetComponent<Image>().enabled = false;
        }
        if (playerControlXbox.spellSecondary[localDirection] == "Range")
        {
            //outerRing.SetActive(true);
            outerRing.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
            colorOuter = new Color32(67, 215, 255, 255);
            childIcon.GetComponent<Image>().sprite = line;
            childIcon.GetComponent<Image>().sprite = null;
            childIcon.GetComponent<Image>().enabled = false;
        }
        if (playerControlXbox.spellSecondary[localDirection] == "Dash")
        {
            outerRing.SetActive(true);
            outerRing.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
            colorOuter = new Color32(0, 0, 255, 255);
            childIcon.GetComponent<Image>().sprite = dash;
            childIcon.GetComponent<Image>().sprite = null;
            childIcon.GetComponent<Image>().enabled = false;
        }
        if (playerControlXbox.spellSecondary[localDirection] == "Boom")
        {
            outerRing.SetActive(true);
            childIcon.GetComponent<Image>().sprite = boom;
            childIcon.GetComponent<Image>().sprite = null;
            outerRing.GetComponent<Image>().color = new Color32(50, 205, 50, 255);
            colorOuter = new Color32(90, 80, 0, 255);
            childIcon.GetComponent<Image>().enabled = false;
        }
        if (playerControlXbox.spellSecondary[localDirection] == "")
        {
            outerRing.SetActive(false);
            childIcon.GetComponent<Image>().sprite = null;
            childIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            colorOuter = new Color32(255, 255, 255, 0);
            childIcon.GetComponent<Image>().enabled = false;
        }
        if (playerControlXbox.spellPrimary[localDirection] == "")
        {
            innerRing.SetActive(false);
            childIcon.GetComponent<Image>().sprite = null;
            childIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            colorInner = new Color32(255, 255, 255, 0);
            childIcon.GetComponent<Image>().enabled = false;
        }

        if (playerControlXbox.spellSelected != spellNumber)
        {
            image.enabled = false;
            childIcon.GetComponent<Image>().enabled = false;
            innerRing.GetComponent<Image>().color = new Color32(colorInner.r, colorInner.g, colorInner.b, 160);
            outerRing.GetComponent<Image>().color = new Color32(colorOuter.r, colorOuter.g, colorOuter.b, 160);
        }

        

    }

}
