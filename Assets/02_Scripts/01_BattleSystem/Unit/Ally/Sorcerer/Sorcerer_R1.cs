using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer_R1 : Ally
{
    [Header("Sorcerer_R1")]
    public GameObject FireBall;
    public int MaxExploStack;
    public List<Vector2> explosionRange;
    public float explosionFCTR;

    new void Update()
    {
        base.Update();
    }

    protected override bool TryAttack()
    {
        Projectile_Attack(FireBall).GetComponent<FireBall>().SetFireBall(MaxExploStack, explosionRange, explosionFCTR, 0, 0);
        return true;
    }


}
