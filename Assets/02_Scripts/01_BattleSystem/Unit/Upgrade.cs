using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public int price;
    public int unitCount;
    public TMPro.TextMeshProUGUI text;

    void Start()
    {
        
    }
    void Update()
    {
        if (GameManager.instance.unitUpgrade[unitCount] == 0) price = 15; // 임시 수치 적용
        else if (GameManager.instance.unitUpgrade[unitCount] == 1) price = 20;
        else if (GameManager.instance.unitUpgrade[unitCount] == 2) price = 20;

        text.text = price.ToString();
    }

    public void Ally_Upgrade()
    {
        if(price <= GameManager.instance.money)
        {
            GameManager.instance.money -= price;
            GameManager.instance.unitUpgrade[unitCount]++;
        }
    }
}
