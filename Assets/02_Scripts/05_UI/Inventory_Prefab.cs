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

    private void Start()
    {
        if (Kind == Obj_Kind.Unit)
        {
            text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
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
