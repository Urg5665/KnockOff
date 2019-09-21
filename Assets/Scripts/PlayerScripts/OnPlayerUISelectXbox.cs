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
            childIcon.GetComponent<Image>().enabled = true;
            outerRing.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            innerRing.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
            if (playerControlXbox.spellPrimary[localDirection] == "Fire")
            {
                image.sprite = red;
                innerRing.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                colorInner = new Color32(255, 0, 0, 255);
                childIcon.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            }
            if (playerControlXbox.spellPrimary[localDirection] == "Wind")
            {
                image.sprite = cyan;
                //67, 215, 255, 255
                innerRing.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
                colorInner = new Color32(67, 215, 255, 255);
                childIcon.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
            }
            if (playerControlXbox.spellPrimary[localDirection] == "Water")
            {
                image.sprite = blue;
                innerRing.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                colorInner = new Color32(0, 0, 255, 255);
                childIcon.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
            }
            if (playerControlXbox.spellPrimary[localDirection] == "Earth")
            {
                //image.sprite = brown;
                //image.sprite = brown;
                innerRing.GetComponent<Image>().color = new Color32(50, 205, 50, 255);
                colorInner = new Color32(50, 205, 50, 255);
                childIcon.GetComponent<Image>().color = new Color32(50, 205, 50, 255);
            }
            if (playerControlXbox.spellPrimary[localDirection] == "")
            {
                image.sprite = white;
            }

            if (playerControlXbox.spellSecondary[localDirection] == "AOE")
            {
                outerRing.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                colorOuter = new Color32(255, 0, 0, 255);
                childIcon.GetComponent<Image>().sprite = cone;
            }
            if (playerControlXbox.spellSecondary[localDirection] == "Range")
            {
                outerRing.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
                colorOuter = new Color32(67, 215, 255, 255);
                childIcon.GetComponent<Image>().sprite = line;
            }
            if (playerControlXbox.spellSecondary[localDirection] == "Dash")
            {
                outerRing.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                colorOuter = new Color32(0, 0, 255, 255);
                childIcon.GetComponent<Image>().sprite = dash;
            }
            if (playerControlXbox.spellSecondary[localDirection] == "Boom")
            {
                childIcon.GetComponent<Image>().sprite = boom;
                outerRing.GetComponent<Image>().color = new Color32(50, 205, 50, 255);
            colorOuter = new Color32(90, 80, 0, 255);
            }
            if (playerControlXbox.spellSecondary[localDirection] == "")
            {
                childIcon.GetComponent<Image>().sprite = null;
                childIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                colorOuter = new Color32(255, 255, 255, 0);
            }
            if (playerControlXbox.spellPrimary[localDirection] == "")
            {
            childIcon.GetComponent<Image>().sprite = null;
            childIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            colorInner = new Color32(255, 255, 255, 0);
            }

        if (playerControlXbox.spellSelected != spellNumber)
        {
            image.enabled = false;
            childIcon.GetComponent<Image>().enabled = false;
            innerRing.GetComponent<Image>().color = new Color32(colorInner.r, colorInner.g, colorInner.b, 50);
            outerRing.GetComponent<Image>().color = new Color32(colorOuter.r, colorOuter.g, colorOuter.b, 50);
        }

        

    }

}
