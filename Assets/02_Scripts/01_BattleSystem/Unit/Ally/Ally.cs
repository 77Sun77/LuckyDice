using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Unit
{
    public AllyKind unitKind;
    public bool isMove;

    public GameObject SynthesisPrefab;
    public SynthesisIcon synthesis;

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
        foreach (var Tile in detectRange_List.GetTileInRange(pawn.X, pawn.Y))
        {
            if (Tile.EnemyList.Count != 0)
                targets.AddRange(Tile.EnemyList);
        }

        isTargetDetected = targets.Count != 0;
    }

    protected override void Die()
    {
        PawnGenerator.instance.SpawnedAllies.Remove(this);
        base.Die();
    }

    public void SpawnSynthesis()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject go = Instantiate(SynthesisPrefab, canvas.transform);
        go.name = $"{transform.name} Synthesis Icon";

        synthesis = go.GetComponent<SynthesisIcon>();
        synthesis.InitializeHPBar(this);
    }
    public void DestroySynthesis()
    {
        if (synthesis)
        {
            Destroy(synthesis.gameObject);
            synthesis = null;
        }
        
    }
}
