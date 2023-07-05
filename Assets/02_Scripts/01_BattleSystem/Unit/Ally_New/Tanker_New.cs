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
        switch (GameManager.instance.unitUpgrade[3]) // 업글 능력치 적용
        {
            case 1:
                Upgrade(0, 0, 0);
                break;
            case 2:
                Upgrade(0, 0, 0);
                break;
            case 3:
                Upgrade(0, 0, 0);
                break;

        }
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
        switch (Rating)
        {
            case 1:
                break;
            case 2:
            case 3:
                Vector2 targetPos = new Vector2(pawn.X, pawn.Y);
                AOE_Attack(damage * SelfDestructionFCTR, targetPos, AOERange);
                break;
        }
        base.Die();
    }

}
