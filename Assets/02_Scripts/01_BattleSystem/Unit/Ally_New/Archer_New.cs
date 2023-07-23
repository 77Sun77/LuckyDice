using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_New : Ally
{
    [Header("Archer_R1")]
    public GameObject Arrow;

    [Header("Archer_R2")]
    public float DoubleShotDelay;

    [Header("Archer_R3")]
    public bool CanPass;

    protected override void OnEnable()
    {
        base.OnEnable();
        Arrow.GetComponent<Projectile>().CanPass = CanPass;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override bool TryAttack()
    {
        switch(Rating)
        {
            case 1:
                Projectile_Attack(damage,Arrow);
                return true;

            case 2:
            case 3:
                StartCoroutine(ShotDouble());
                return true;
        }

        return false;
    }

    IEnumerator ShotDouble()
    {
        Projectile_Attack(damage,Arrow);

        yield return new WaitForSeconds(DoubleShotDelay);

        Projectile_Attack(damage,Arrow);
    }
}
