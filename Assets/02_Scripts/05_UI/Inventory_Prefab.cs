using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    GameObject img;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick_Btn);
        img = transform.Find("BG").gameObject;
        if (Kind == Obj_Kind.Unit)
        {
            text = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

            
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

        if (PawnPlacementManager.instance.ObjTemp == gameObject) img.SetActive(true);
        else img.SetActive(false);
    }

    public void OnClick_Btn()
    {
        //UIManager.instance.Blind.SetActive(true);

        // if (Kind != Obj_Kind.Dice) UIManager.instance.MapMask.SetActive(true);
        // else UIManager.instance.MapMask.SetActive(false);


        if (PawnPlacementManager.instance.ObjTemp)
        {
            PawnPlacementManager.instance.ObjTemp = null;
            PawnPlacementManager.instance.isActive = false;
        }
        else
        {
            PawnPlacementManager.instance.ObjTemp = gameObject;
            PawnPlacementManager.instance.isActive = true;
        }
        

    }

    public void OnClick_Dice()
    {
        if (GameManager.instance.inventory.inventoryCount == 15) return;
        prefab.SetActive(true);
        UIManager.instance.UI.SetActive(false);
    }
}
