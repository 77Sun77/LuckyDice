using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_R2 : Ally
{
    [Header("Warrior_R2")]
    public List<Vector2> AOERange;

    void Start()
    {
        first_Setting();
    }

    new void Update()
    {
        base.Update();
    }

    protected override bool TryAttack()
    {
        bool _canAttack = false;

        //if (GetClosestTarget(targets).pawn.IsOverCenter)//적이 중앙을 넘어왔을때 근접 발동
        //{
        //     AOE_Attack(AOERange);
        //    _canAttack = true;
        //}
        return _canAttack;
    }

}
