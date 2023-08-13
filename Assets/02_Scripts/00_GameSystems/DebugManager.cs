using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public List<GameObject> Inventory_Prefabs;
    
    public bool IsDebugEnemySpawning
    {
        get 
        { 
            return isDebugEnemySpawning; 
        }
        set
        {
            Debug.Log("Changed");
            isDebugEnemySpawning = value;
        }
    }
        
    private bool isDebugEnemySpawning;

    int ranNum;


    private void Update()
    {
        


    }

}
