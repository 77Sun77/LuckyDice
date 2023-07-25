using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory_Prefab : MonoBehaviour
{
    public enum Obj_Kind { Dice, Unit, Item };
    public Obj_Kind Kind;

    public GameObject prefab;
    public string objectType;

    public int Rating;

    TextMeshProUGUI text;

    public enum Dice_Kind { Ally, Item}
    public Dice_Kind d_Kind;

    private void Start()
    {
        if (Kind == Obj_Kind.Unit)
        {
            text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            
        }
        else if(Kind == Obj_Kind.Dice)
        {
            if(d_Kind == Dice_Kind.Ally)
            {
                prefab = UIManager.instance.allyDiceControl;
            }
            else
            {
                prefab = UIManager.instance.itemDiceControl;
            }
        }
        
    }
    void Update()
    {
        if (text)
        {
            string s = "";
            for(int i=1;i<=Rating;i++)
            {
                s += "I";
            }
            text.text = s;
        }
    }

    public void OnClick_Dice()
    {
        prefab.SetActive(true);
        UIManager.instance.UI.SetActive(false);
    }
}
