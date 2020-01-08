using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnPlayerDashUI : MonoBehaviour
{
    public GameObject[] dashCharges;
    public PlayerControl playerControl;

    public GameObject dashChargeSelected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i <= playerControl.dashCharges)
            {
                dashCharges[i].GetComponent<Image>().fillAmount = 1;
                dashCharges[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                dashCharges[i].SetActive(true);

            }
            else
            {
                dashCharges[i].SetActive(false);
            }
        }

        if (playerControl.dashCharges != 3)
        {
            dashChargeSelected = dashCharges[playerControl.dashCharges];
            dashChargeSelected.GetComponent<Image>().color = new Color32(255, 255, 255, 120);
            dashChargeSelected.GetComponent<Image>().fillAmount = (float)playerControl.dashChargeTime / playerControl.dashChargeLength;
        }
        if (playerControl.dashCharges == 3)
        {
            dashChargeSelected = null;
        }

    }
}
