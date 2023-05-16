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

    protected override bool TryAttack()//���� ����
    {
        bool _canAttack = false;

        switch (attackType)
        {
            case AttackType.None:
                break;
            case AttackType.Active:
                if (GetClosestTarget(targets).pawn.IsOverCenter)
                {
                    Active_Attack();//���� �߾��� �Ѿ������ ���� �ߵ�
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

        //detectRange���� Tile�� EnemyList �������� �߰�
        foreach (var Tile in GetTileInRange(pawn.X, pawn.Y, detectRange_List))
        {
            if (Tile.EnemyList.Count != 0)
                targets.AddRange(Tile.EnemyList);
        }

        isTargetDetected = targets.Count != 0;
    }
}
