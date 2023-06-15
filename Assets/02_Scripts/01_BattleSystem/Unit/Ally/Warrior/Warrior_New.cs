using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_New : Ally
{
    [Header("Warrior_R2")]
    public List<Vector2> AOERange;

    [Header("Warrior_R3")]
    public float vampPercent;

    protected override void Update()
    {
        base.Update();
    }

    protected override bool TryAttack()
    {
        bool _canAttack = false;

        switch(Rating)
        {
            case 1:
                if (GetClosestTarget(targets).pawn.IsOverCenter)//적이 중앙을 넘어왔을때 근접 발동
                {
                    Active_Attack(damage);
                    _canAttack = true;
                }
                break;
            case 2:
                if (GetClosestTarget(targets).pawn.IsOverCenter)//적이 중앙을 넘어왔을때 근접 발동
                {
                    Vector2 targetPos = new Vector2(pawn.X + 1, pawn.Y);
                    AOE_Attack(targetPos, AOERange);
                    _canAttack = true;
                }
                break;
            case 3:
                if (GetClosestTarget(targets).pawn.IsOverCenter)//적이 중앙을 넘어왔을때 근접 발동
                {
                    Vector2 targetPos = new Vector2(pawn.X + 1, pawn.Y);
                    AOE_Attack(targetPos, AOERange);

                    for (int i = 0; i < AOE_Attack(targetPos, AOERange).Count; i++)
                    {
                        HealHP(damage / vampPercent / 100);
                    }

                    _canAttack = true;
                }
                break;
        }

        return _canAttack;
    }
}
