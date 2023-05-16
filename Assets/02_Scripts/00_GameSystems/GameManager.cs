using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Base _base;
    public UnitSynthesis us;

    public int money;

    public bool IsInBattle;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Set_Money(int value)
    {
        money += value;
    }
}
