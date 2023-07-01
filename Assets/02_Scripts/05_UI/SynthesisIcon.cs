using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthesisIcon : MonoBehaviour
{
    Ally unit;
    public List<Ally> units = new List<Ally>();
    int index, price;
    
    public void Initialize_SynthesisIcon(Ally obj)
    {
        unit = obj;

        if (unit.Rating == 1) price = 300; // 업그레이드 가격 (추후 다시 설정)
        else if (unit.Rating == 2) price = 500;
        else price = 1000;

        if (unit.allyKind == AllyKind.Warrior) index = 0;
        else if (unit.allyKind == AllyKind.Sorcerer) index = 1;
        else if (unit.allyKind == AllyKind.Lancer) index = 2;
        else if (unit.allyKind == AllyKind.Tanker) index = 3;
        else if (unit.allyKind == AllyKind.Buffer) index = 4;
        else if(unit.allyKind == AllyKind.Archer) index = 5;
    }
    void Update()
    {
        if (!unit) DestroyImmediate(gameObject);
        else transform.position = Camera.main.WorldToScreenPoint(unit.transform.position + Vector3.up);

    }

    public void Synthesis()
    {
        if (GameManager.instance.money < price) return; // 현재 돈이 가격보다 적을 경우 실행 안함 (가격 설정 후 UI 연결 필요)

        GameObject[] unitsGO = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject go in unitsGO)
        {
            Ally unit = go.GetComponent<Ally>();
            if (unit.allyKind == this.unit.allyKind) this.units.Add(unit);

        }
        List<Ally> units = new List<Ally>();
        foreach(Ally unit in this.units)
        {
            if (unit.Rating == this.unit.Rating) units.Add(unit);
        }
        //GameManager.instance.pg.Roll(GameManager.instance.us.prefabs[index][this.unit.Rating - 1]);
        AllyGenerator.instance.SpawnAlly(unit.allyKind, unit.Rating+1);
        GameManager.instance.money -= price;
        for (int i = 0; i < 3; i++)
        {
            Destroy(units[i].gameObject);
        }
        
    }
}
