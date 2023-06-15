using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_R2 : Ally
{
    [Header("Archer_R2")]
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
        Projectile_Attack(Arrow);

        yield return new WaitForSeconds(DoubleShotDelay);

        Projectile_Attack(Arrow);
    }

}
