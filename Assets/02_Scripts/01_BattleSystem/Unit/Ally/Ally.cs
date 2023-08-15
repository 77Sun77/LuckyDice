using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Unit
{
    public AllyKind allyKind;
    public bool isMove, isSynthesis;
    public List<GameObject> SynthesisUnits;
    [HideInInspector]
    public SynthesisIcon synthesis;

    //protected override bool TryAttack()//적을 공격
    //{
    //    bool _canAttack = false;

    //    switch (attackType)
    //    {
    //        case AttackType.None:
    //            break;
    //        case AttackType.Active:
    //            if (GetClosestTarget(targets).pawn.IsOverCenter)
    //            {
    //                Active_Attack(damage);//적이 중앙을 넘어왔을때 근접 발동
    //                _canAttack = true;
    //            }
    //            break;

    //        case AttackType.Projectile:
    //            Projectile_Attack(damage,ProjectilePrefab);
    //            _canAttack = true;
    //            break;

    //        case AttackType.AreaOfEffect:
    //            AOE_Attack(damage,AOERange_List);
    //            _canAttack = true;
    //            break;

    //        case AttackType.AOE_Melee:
    //            if (GetClosestTarget(targets).pawn.IsOverCenter)
    //            {
    //                AOE_Attack(damage,AOERange_List);
    //                _canAttack = true;
    //            }
    //            break;
    //    }
    //    return _canAttack;
    //}
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
    protected override bool TryAttack()//Melee 관련 로직 추가 고려
    {
        return false;
    }
    
    protected override void Die()
    {
        GameManager.instance.SpawnedAllies.Remove(this);
        base.Die();
    }

    public void SpawnSynthesis()
    {
        if (!synthesis)
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject go = Instantiate(UIManager.instance.SysthesisIcon_Prefab, canvas.transform);
            go.name = $"{transform.name} Synthesis Icon";

            synthesis = go.GetComponent<SynthesisIcon>();
            synthesis.Initialize_SynthesisIcon(this);
        }
        
    }
    public void DestroySynthesis()
    {
        if (synthesis)
        {
            Destroy(synthesis.gameObject);
            synthesis = null;
        }
        
    }

    public virtual void HealAllies(List<Vector2> aoeRange)
    {
        if (!isTargetDetected || time >= 0)
            return;

        time = delayTime;

        List<Unit> skillTargets = new();

        Vector2 AOEPos = new Vector2(pawn.X, pawn.Y);

        foreach (var _tile in aoeRange.GetTileInRange((int)AOEPos.x, (int)AOEPos.y))
        {
            if (_tile.Ally != null)
            {
                skillTargets.Add(_tile.Ally);
            }
            //디버깅용 임시 코드
            _tile.Do_AOE_Effect(Color.green);
        }

        foreach (var Ally in skillTargets)
        {
            Ally.HealHP(damage);
        }
    }

    protected void Upgrade(float maxHP, float damage, float defense) // 각각 캐릭터의 Update에서 UpgradeCount에 맞게 능력치 맞춰서 호출
    {
        this.maxHP += maxHP;
        this.damage += damage;
        this.defense += defense;
    }

    protected override void Update()
    {
        if (isSynthesis)
        {
            isSynthesis = SynthesisUnits.Count == 3 ? true : false;
            foreach (GameObject go in SynthesisUnits)
            {
                if (go == null)
                {
                    isSynthesis = false;
                    break;
                }
            }

            SpawnSynthesis();
        }
        else
        {
            SynthesisUnits.Clear();
            DestroySynthesis();
        }

        base.Update();
    }
}
