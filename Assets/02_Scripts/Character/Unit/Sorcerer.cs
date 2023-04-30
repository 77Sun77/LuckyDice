using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : Unit
{
    void Start()
    {
        //hp = 0;
        //damage = 0;
        //srange = 0;
        first_Setting();
        unitKind = Kind.Sorcerer;
    }

    new void Update()
    {
        base.Update();
    }
}
