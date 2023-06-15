using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_R3 : Ally
{
    [Header("Archer_R3")]
    public GameObject Arrow;
    public float DoubleShotDelay;

  

    new void Update()
    {
        base.Update();
    }

    protected override bool TryAttack()
    {
        StartCoroutine(ShotDouble());
        return true;
    }

    IEnumerator ShotDouble()
    {
        Projectile_Attack(damage,Arrow);

        yield return new WaitForSeconds(DoubleShotDelay);

        Projectile_Attack(damage,Arrow);
    }
}
