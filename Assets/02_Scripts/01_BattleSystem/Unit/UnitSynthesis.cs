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

        /* 2성, 3성 프리팹 가져와서 역할 분리한 하나의 리스트로 처리하는 코드
        for (int i = 0; i < 6; i+=2)
        {
            GameObject[] units = { unitPrefabs[i], unitPrefabs[i + 1] };
            prefabs.Add(units);
        }
        */
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
                else if (unit.unitKind == AllyKind.Debuffer) units[2].Add(unit);
                else if (unit.unitKind == AllyKind.Tanker) units[3].Add(unit);
                else if (unit.unitKind == AllyKind.Buffer) units[4].Add(unit);
                else units[5].Add(unit);
            }

            for(int i=0;i< units.Count; i++)
            {
                Synthesis(units[i], i);
            }
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
    }

    public void Synthesis(List<Ally> units, int index) // index는 유닛의 인덱스값  0:Warrior, 1:Sorcerer, 2:Debuffer, 3:Tanker, 4:Buffer, 5:Archer
    {
        int[] Rating = { 0, 0, 0 }; // 0:1성, 1:2성, 2:3성
        foreach(Ally unit in units)
        {
            Rating[unit.Rating - 1]++;
        }
        for(int i = 0; i < 2; i++)
        {
            if (Rating[i] >= 3)
            {
                foreach (Ally unit in units)
                {
                    if (unit.Rating == i+1)
                    {
                        print(unit.name + " 유닛 삭제");
                        Destroy(unit.gameObject);
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
                //prefabs[index][i + 1];
                NextRating = false;

            }
        }

    }
}
