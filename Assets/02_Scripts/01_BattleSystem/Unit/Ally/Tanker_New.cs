using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanker_New : Ally
{
    [Header("Tanker_R3")]
    public List<Vector2> AOERange;
    public float SelfDestructionFCTR;

    protected override void Update()
    {
        base.Update();
    }

    public override void TakeDamage(float _damage,GameObject atkTarget)
    {
        Debug.Log("TakeDamage");
        base.TakeDamage(_damage, atkTarget);

        switch(Rating)
        {
            case 1:
                break;
            case 2:
            case 3:
                if (atkTarget.TryGetComponent(out Enemy enemy)) enemy.TakeDamage(damage, atkTarget);
                break;
        }
    }

    protected override void Die()
    {
        Vector2 targetPos = new Vector2(pawn.X, pawn.Y);
        AOE_Attack(targetPos, AOERange);
    }

}
