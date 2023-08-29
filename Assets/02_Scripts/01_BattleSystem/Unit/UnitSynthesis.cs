using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSynthesis : MonoBehaviour
{
    public List<List<GameObject>> units = new List<List<GameObject>>(); // 0:Warrior, 1:Sorcerer, 2:Lancer, 3:Tanker, 4:Buffer, 5:Archer

    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            units.Add(new List<GameObject>());
        }

        
    }

    void Update()
    {
        
        Search();
        //Search(true);
    }
    public void Search(bool isAuto = false)
    {
        for(int n = 0; n<3; n++)
        {
            foreach (List<GameObject> list_GO in units) list_GO.Clear();
            List<GameObject> unitsGO = new List<GameObject>(GameObject.FindGameObjectsWithTag("Unit"));
            foreach (GameObject inventory in GameManager.instance.inventory.inventory)
            {
                if (inventory.GetComponent<Inventory_Prefab>().Kind == Inventory_Prefab.Obj_Kind.Unit) unitsGO.Add(inventory);
            }

            foreach (GameObject go in unitsGO)
            {
                //print(unitsGO.Count);
                if (!go.GetComponent<Ally>()) break;

                if (!go.GetComponent<Ally>() || go.GetComponent<Pawn>().pastTile.IsTable) break;
                Ally unit = go.GetComponent<Ally>();
                if (unit.Rating != n + 1 || unit.isSynthesis) continue;
                units[ReturnIndex(unit.allyKind)].Add(unit.gameObject);

            }
            for (int i = GameObject.FindGameObjectsWithTag("Unit").Length; i < unitsGO.Count; i++)
            {
                Inventory_Prefab ip = unitsGO[i].GetComponent<Inventory_Prefab>();
                if (ip.Rating != n + 1 || ip.isSynthesis) continue;
                Ally unit = ip.prefab.GetComponent<Ally>();

                units[ReturnIndex(unit.allyKind)].Add(ip.gameObject);

            }


            foreach(List<GameObject> go in units)
            {
                if (go.Count >= 3)
                {
                    List<GameObject> objects = go.GetRange(0, 3);
                    
                    string allyName = objects[0].GetComponent<Ally>() ? objects[0].GetComponent<Ally>().allyKind.ToString() : objects[0].GetComponent<Inventory_Prefab>().objectType;
                    int Rating = objects[0].GetComponent<Ally>() ? objects[0].GetComponent<Ally>().Rating : objects[0].GetComponent<Inventory_Prefab>().Rating;
                    if (Rating == 3) break;
                    if (isAuto) GameManager.instance.inventory.Add_Inventory(allyName, Rating+1);
                    foreach (GameObject g in objects)
                    {
                        if (g.GetComponent<Ally>())
                        {
                            if (isAuto)
                            {
                                Destroy(g);
                            }
                            else
                            {
                                g.GetComponent<Ally>().SynthesisUnits = objects;
                                g.GetComponent<Ally>().isSynthesis = true;
                            }

                        }
                        else
                        {
                            if (isAuto)
                            {
                                Destroy(g);
                                GameManager.instance.inventory.Delete_Inventory(g);
                            }
                            else
                            {
                                g.GetComponent<Inventory_Prefab>().SynthesisUnits = objects;
                                g.GetComponent<Inventory_Prefab>().isSynthesis = true;
                            }
                        }
                    }
                } 
            }
        }
        
    }

    int ReturnIndex(AllyKind ally)
    {
        int count=0;
        if (ally == AllyKind.Warrior) count = 0;
        else if (ally == AllyKind.Sorcerer) count = 1;
        else if (ally == AllyKind.Lancer) count = 2;
        else if (ally == AllyKind.Tanker) count = 3;
        else if (ally == AllyKind.Buffer) count = 4;
        else if (ally == AllyKind.Archer) count = 5;
        return count;
    }
    
}
