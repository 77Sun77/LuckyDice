using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_R1 : Ally
{
    [Header("Archer_R1")]
    public GameObject Arrow;

    protected override void Update()
    {
        base.Update();
    }

    protected override bool TryAttack()
    {
        Projectile_Attack(Arrow);
        return true;
    }
}
