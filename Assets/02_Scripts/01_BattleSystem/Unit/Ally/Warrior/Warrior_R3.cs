using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_R3 : Ally
{
    [Header("Warrior_R3")]
    public List<Vector2> AOERange;
    public float vampPercent;

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

        //if (GetClosestTarget(targets).pawn.IsOverCenter)//���� �߾��� �Ѿ������ ���� �ߵ�
        //{
        //    AOE_Attack(AOERange);

        //    for (int i = 0; i < AOE_Attack(AOERange).Count; i++)
        //    {
        //        HealHP(damage / vampPercent / 100);
        //    }

        //    _canAttack = true;
        //}
        return _canAttack;
    }


}
