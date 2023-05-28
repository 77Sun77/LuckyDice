using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSynthesis : MonoBehaviour
{
    public List<List<Ally>> units = new List<List<Ally>>(); // 0:Warrior, 1:Sorcerer, 2:Debuffer, 3:Tanker, 4:Buffer, 5:Archer
    
    public List<GameObject[]> prefabs = new List<GameObject[]>(); // 0:Warrior, 1:Sorcerer, 2:Debuffer, 3:Tanker, 4:Buffer, 5:Archer
    public List<GameObject> unitPrefabs = new List<GameObject>();
    
    bool NextRating;
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            units.Add(new List<Ally>());
        }

        for (int i = 0; i < 12; i+=2)
        {
            GameObject[] units = { unitPrefabs[i], unitPrefabs[i + 1] };
            prefabs.Add(units);
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
        /*
        for (int i = 0; i < 6; i++)
        {
            units[i].Clear();
        }
        GameObject[] unitsGO = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject go in unitsGO)
        {
            Ally unit = go.GetComponent<Ally>();
            if (unit.unitKind == AllyKind.Warrior) units[0].Add(unit);
            else if (unit.unitKind == AllyKind.Sorcerer) units[1].Add(unit);
            else if (unit.unitKind == AllyKind.Debuffer) units[2].Add(unit);
            else if (unit.unitKind == AllyKind.Tanker) units[3].Add(unit);
            else if (unit.unitKind == AllyKind.Buffer) units[4].Add(unit);
            else units[5].Add(unit);
        }
        foreach (List<Ally> units in this.units)
        {
            Synthesis(units);
        }*/
        Search();
    }
    public void Search()
    {
        for (int i = 0; i < 6; i++)
        {
            units[i].Clear();
        }
        GameObject[] unitsGO = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject go in unitsGO)
        {
            Ally unit = go.GetComponent<Ally>();
            if (unit.unitKind == AllyKind.Warrior) units[0].Add(unit);
            else if (unit.unitKind == AllyKind.Sorcerer) units[1].Add(unit);
            else if (unit.unitKind == AllyKind.Lancer) units[2].Add(unit);
            else if (unit.unitKind == AllyKind.Tanker) units[3].Add(unit);
            else if (unit.unitKind == AllyKind.Buffer) units[4].Add(unit);
            else units[5].Add(unit);
        }

        for (int i = 0; i < units.Count; i++)
        {
            Synthesis(units[i], i);
        }
    }
    public void Synthesis(List<Ally> units, int index) // index´Â À¯´ÖÀÇ ÀÎµ¦½º°ª  0:Warrior, 1:Sorcerer, 2:Debuffer, 3:Tanker, 4:Buffer, 5:Archer
    {
        int[] Rating = { 0, 0, 0 }; // 0:1¼º, 1:2¼º, 2:3¼º
        foreach(Ally unit in units)
        {
            Rating[unit.Rating - 1]++;
        }
        List<GameObject> distroyList = new List<GameObject>();
        
        for(int i = 0; i < 2; i++)
        {
            distroyList.Clear();
            if (Rating[i] >= 3)
            {
                foreach (Ally unit in units)
                {
                    if (unit.Rating == i+1)
                    {
                        print(unit.name + " À¯´Ö »èÁ¦");
                        distroyList.Add(unit.gameObject);
                        Rating[i]--;
                        if (Rating[i] == 0)
                        {
                            
                            NextRating = true;
                            break;
                        }
                    }
                }
            }
            if (NextRating)
            {
                GameManager.instance.pg.Roll(prefabs[index][i]);
                NextRating = false;
                foreach (GameObject unit in distroyList) Destroy(unit);
            }
        }

    }
}
