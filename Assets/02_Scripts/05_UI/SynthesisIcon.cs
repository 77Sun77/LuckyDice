using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthesisIcon : MonoBehaviour
{
    Ally unit;
    public List<Ally> units = new List<Ally>();
    int index;
    
    public void InitializeHPBar(Ally obj)
    {
        unit = obj;

        if (unit.unitKind == AllyKind.Warrior) index = 0;
        else if (unit.unitKind == AllyKind.Sorcerer) index = 1;
        else if (unit.unitKind == AllyKind.Lancer) index = 2;
        else if (unit.unitKind == AllyKind.Tanker) index = 3;
        else if (unit.unitKind == AllyKind.Buffer) index = 4;
        else index = 5;
    }
    void Update()
    {
        if (!unit) DestroyImmediate(gameObject);
        else transform.position = Camera.main.WorldToScreenPoint(unit.transform.position + Vector3.up);

    }

    public void Synthesis()
    {
       // if (GameManager.instance.money < price) return;
        GameObject[] unitsGO = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject go in unitsGO)
        {
            Ally unit = go.GetComponent<Ally>();
            if (unit.unitKind == this.unit.unitKind) this.units.Add(unit);

        }
        List<Ally> units = new List<Ally>();
        foreach(Ally unit in this.units)
        {
            if (unit.Rating == this.unit.Rating) units.Add(unit);
        }
        GameManager.instance.pg.Roll(GameManager.instance.us.prefabs[index][this.unit.Rating - 1]);
        for (int i = 0; i < 3; i++)
        {
            Destroy(units[i].gameObject);
        }
        
    }
}
