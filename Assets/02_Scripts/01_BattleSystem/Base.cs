using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    float hp; 
    
    public float HP
    {
        get { return hp; }
        set { hp = value; }
    }
    void Start()
    {
        HP = 100;
    }
    
    void Update()
    {
        
    }
}
