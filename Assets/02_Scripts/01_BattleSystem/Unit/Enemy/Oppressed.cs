using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oppressed : Enemy
{
    public float hp1, hp2;
    public int phase;
    void Start()
    {
        phase = 1;
        hp = hp1;
        maxHP = hp;
        SpawnHPBar();
    }

    new void Update()
    {
        base.Update();
    }

    public void TakeDamage(float damage)
    {
        damage -= defense;
        hp -= damage;
        if (hp <= 0)
        {
            if(phase == 1)
            {
                phase=2;
                speed *= 2;
                hp = hp2;
                maxHP = hp;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
    }
}
