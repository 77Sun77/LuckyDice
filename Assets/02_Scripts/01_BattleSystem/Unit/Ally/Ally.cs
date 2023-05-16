using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Unit
{
    public AllyKind unitKind;

    protected override void Update()
    {
        base.Update();
    }

    protected override bool TryAttack()//적을 공격
    {
        bool _canAttack = false;

        switch (attackType)
        {
            case AttackType.None:
                break;
            case AttackType.Active:
                if (GetClosestTarget(targets).pawn.IsOverCenter)
                {
                    Active_Attack();//적이 중앙을 넘어왔을때 근접 발동
                    _canAttack = true;
                }
                break;

            case AttackType.Projectile:
                Projectile_Attack();
                _canAttack = true;
                break;

            case AttackType.AreaOfEffect:
                AOE_Attack();
                _canAttack = true;
                break;

            case AttackType.AOE_Melee:
                if (GetClosestTarget(targets).pawn.IsOverCenter)
                {
                    AOE_Attack();
                    _canAttack = true;
                }
                break;
        }
        return _canAttack;
    }

    protected override void Search_Targets()
    {
        targets.Clear();

        //detectRange안의 Tile의 EnemyList 가져온후 추가
        foreach (var Tile in GetTileInRange(pawn.X, pawn.Y, detectRange_List))
        {
            if (Tile.EnemyList.Count != 0)
                targets.AddRange(Tile.EnemyList);
        }

        isTargetDetected = targets.Count != 0;
    }
}
