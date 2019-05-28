﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnPlayerUISelect : MonoBehaviour
{
    public bool selected = false;

    public GameObject player;
    public PlayerControl playerControl;
    public int spellNumber;

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
            outerRing.SetActive(true);
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().sprite = inner2;
            outerRing.GetComponent<Image>().sprite = outer2;
            outerRing.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            innerRing.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            colorInner = new Color32(255, 255, 255, 255);
            colorOuter = new Color32(255, 255, 255, 255);
            childIcon.GetComponent<Image>().enabled = true;

            if (playerControl.spellPrimary[spellNumber] == "Fire")
            {
                image.sprite = red;
                innerRing.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                colorInner = new Color32(255, 0, 0, 255);
                childIcon.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            }
            if (playerControl.spellPrimary[spellNumber] == "Wind")
            {
                image.sprite = cyan;
                //67, 215, 255, 255
                innerRing.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
                colorInner = new Color32(67, 215, 255, 255);
                childIcon.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
            }
            if (playerControl.spellPrimary[spellNumber] == "Water")
            {
                image.sprite = blue;
                innerRing.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                colorInner = new Color32(0, 0, 255, 255);
                childIcon.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
            }
            if (playerControl.spellPrimary[spellNumber] == "Earth")
            {
                image.sprite = brown;
                innerRing.GetComponent<Image>().color = new Color32(90, 80, 0, 255);
                colorInner = new Color32(90, 80, 0, 255);
                childIcon.GetComponent<Image>().color = new Color32(90, 80, 0, 255);
            }
            if (playerControl.spellPrimary[spellNumber] == "")
            {
                image.sprite = white;
            }

            if (playerControl.spellSecondary[spellNumber] == "AOE")
            {
                outerRing.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                colorOuter = new Color32(255, 0, 0, 255);
                childIcon.GetComponent<Image>().sprite = cone;
            }
            if (playerControl.spellSecondary[spellNumber] == "Range")
            {
                outerRing.GetComponent<Image>().color = new Color32(67, 215, 255, 255);
                colorOuter = new Color32(67, 215, 255, 255);
                childIcon.GetComponent<Image>().sprite = line;
            }
            if (playerControl.spellSecondary[spellNumber] == "Dash")
            {
                outerRing.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
                colorOuter = new Color32(0, 0, 255, 255);
                childIcon.GetComponent<Image>().sprite = dash;
            }
            if (playerControl.spellSecondary[spellNumber] == "Boom")
            {
                childIcon.GetComponent<Image>().sprite = boom;
                outerRing.GetComponent<Image>().color = new Color32(90, 80, 0, 255);
                colorOuter = new Color32(90, 80, 0, 255);
            }
            if (playerControl.spellSecondary[spellNumber] == "")
            {
                childIcon.GetComponent<Image>().sprite = null;
                childIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                colorOuter = new Color32(255, 255, 255, 0);
            }
        }
        else if (playerControl.spellSelected != spellNumber)
        {
            image.enabled = false;
            childIcon.GetComponent<Image>().enabled = false;
            outerRing.SetActive(true);
            innerRing.SetActive(true);
            innerRing.GetComponent<Image>().sprite = inner1;
            outerRing.GetComponent<Image>().sprite = outer1;
            innerRing.GetComponent<Image>().color = new Color32(colorInner.r, colorInner.g, colorInner.b, 90);
            outerRing.GetComponent<Image>().color = new Color32(colorOuter.r, colorOuter.g, colorOuter.b, 90);

        }
    }
}