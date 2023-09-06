using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceStore : MonoBehaviour
{
    public int price;
    public Inventory_Prefab dicePrefab;
    public TMPro.TextMeshProUGUI text;

    public Color[] color;

    private void Update()
    {
        if (price <= GameManager.instance.money)
        {
            text.color = color[0];
        }
        else
        {
            text.color = color[1];
        }

        
    }

    public void OnClick_Btn()
    {
        if (price <= GameManager.instance.money && GameManager.instance.dice_Inventory.contents.childCount < 10)
        {
            GameManager.instance.dice_Inventory.Add_Inventory(dicePrefab.objectType);
            GameManager.instance.money -= price;
        }
    }
}
