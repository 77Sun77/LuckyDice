using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthesisIcon : MonoBehaviour
{
    Ally unit;
    Inventory_Prefab _unit;
    int price;
    
    public void Initialize_SynthesisIcon(Ally obj = null, Inventory_Prefab _obj = null)
    {
        unit = obj;
        _unit = _obj;

        int Rating = unit ? unit.Rating : _unit.Rating;
        if (Rating == 1) price = 300; // ���׷��̵� ���� (���� �ٽ� ����)
        else if (Rating == 2) price = 500;
        else price = 1000;

        if (_unit) transform.parent = _unit.transform;
    }
    void Update()
    {


        if (unit) transform.position = Camera.main.WorldToScreenPoint(unit.transform.position + Vector3.up);
        else if(_unit) transform.position = _unit.transform.position + (Vector3.up*45);
        else Destroy(gameObject);


    }

    public void Synthesis()
    {
        if (GameManager.instance.money < price) return; // ���� ���� ���ݺ��� ���� ��� ���� ���� (���� ���� �� UI ���� �ʿ�)
        GameManager.instance.money -= price;

        if (unit)
        {
            GameManager.instance.inventory.Add_Inventory(unit.allyKind.ToString(), unit.Rating + 1);
            foreach (GameObject go in unit.SynthesisUnits)
            {
                if (go.GetComponent<Inventory_Prefab>()) GameManager.instance.inventory.Delete_Inventory(go);
                else Destroy(go);
            }
        }
        else
        {
            GameManager.instance.inventory.Add_Inventory(_unit.objectType, _unit.Rating + 1);
            foreach (GameObject go in _unit.SynthesisUnits)
            {
                if (go.GetComponent<Inventory_Prefab>()) GameManager.instance.inventory.Delete_Inventory(go);
                else Destroy(go);
            }
        }
        
    }
}
