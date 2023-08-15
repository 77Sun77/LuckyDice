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
        if (Rating == 1) price = 300; // 업그레이드 가격 (추후 다시 설정)
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
        if (GameManager.instance.money < price) return; // 현재 돈이 가격보다 적을 경우 실행 안함 (가격 설정 후 UI 연결 필요)
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
