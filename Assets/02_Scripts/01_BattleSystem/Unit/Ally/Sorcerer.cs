using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : Ally
{
    [Header("Sorcerer")]
    public float dotDamage;

    void Start()
    {
        first_Setting();
    }

    new void Update()
    {
        base.Update();
    }
}
