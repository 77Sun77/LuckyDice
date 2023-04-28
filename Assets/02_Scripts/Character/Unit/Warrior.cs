using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Unit
{
    void Start()
    {
        hp = 0;
        damage = 0;
        range = 0;

    }

    // Update is called once per frame
    void Update()
    {
        Search();
    }
}
