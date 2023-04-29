using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Unit
{
    void Start()
    {
        //hp = 0;
        //damage = 0;
        //srange = 0;
        Rating = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Search();
    }
}
