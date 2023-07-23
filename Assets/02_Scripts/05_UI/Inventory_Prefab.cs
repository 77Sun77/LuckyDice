using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Prefab : MonoBehaviour
{
    public enum Obj_Kind { Dice, Unit, Item };
    public Obj_Kind Kind;

    public GameObject prefab;
    public string objectType;
    void Update()
    {
        
    }

    public void OnClick_Dice()
    {
        prefab.SetActive(true);
        UIManager.instance.UI.SetActive(false);
    }
}
