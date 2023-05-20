using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Base _base;
    public UnitSynthesis us;
    public PawnGenerator pg;

    public int money;

    public bool IsInBattle;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale *=2f;
            Debug.Log("Speed Up");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale /= 2f;
            Debug.Log("Speed Down");
        }

    }

    public void Set_Money(int value)
    {
        money += value;
    }
}
