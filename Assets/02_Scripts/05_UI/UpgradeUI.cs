using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour
{

    public TextMeshProUGUI[] upgradeLevel;

    public Color[] color;
    public Upgrade[] upgradeUnits;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 6; i++)
        {
            upgradeLevel[i].text = "LV " + (GameManager.instance.unitUpgrade[i] + 1);
        }
        for (int i = 0; i < 6; i++)
        {
            if (upgradeUnits[i].price <= GameManager.instance.money)
            {
                upgradeUnits[i].text.color = color[0];
            }
            else
            {
                upgradeUnits[i].text.color = color[1];
            }
        }
    }
}
