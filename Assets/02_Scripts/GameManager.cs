using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Base _base;

    public int money;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        
    }

    public void Set_Money(int value)
    {
        money += value;
    }
}
