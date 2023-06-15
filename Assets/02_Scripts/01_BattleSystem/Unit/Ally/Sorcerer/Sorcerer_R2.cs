using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer_R2 : Ally
{
    [Header("Sorcerer_R2")]
    public GameObject FireBall;
    public int MaxExploStack;
    public List<Vector2> explosionRange;
    public float explosionFCTR;

    public float SlowValue;
    public float SlowDuration;

    new void Update()
    {
        base.Update();
    }

    protected override bool TryAttack()
    {
        Projectile_Attack(FireBall).GetComponent<FireBall>().SetFireBall(MaxExploStack, explosionRange, explosionFCTR, SlowValue, SlowDuration);
        return true;
    }


}
