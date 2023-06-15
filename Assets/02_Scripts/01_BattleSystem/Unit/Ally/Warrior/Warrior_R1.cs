using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_R1 : Ally
{
    void Start()
    {
        first_Setting();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override bool TryAttack()
    {
        bool _canAttack = false;

        if (GetClosestTarget(targets).pawn.IsOverCenter)//적이 중앙을 넘어왔을때 근접 발동
        {
            Active_Attack();
            _canAttack = true;
        }
        return _canAttack;
    }

}
