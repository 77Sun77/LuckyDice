using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer_R3 : Ally
{
    [Header("Buffer_R3")]
    public List<Vector2> AOERange;
    public float DefenseBuffValue;
    public float WaveHealValue;

    List<Unit> skillTargets_Last = new();

    protected override void Update()
    {
        time -= Time.deltaTime;

        Search_Targets();
        
        if (GameManager.instance.IsInBattle)
        {
            HealAllies(AOERange);
        }

        GiveDefenseBuff(AOERange, DefenseBuffValue);

        SyncHPBar();

        CheckDefenseBuff();
    }

    private void OnEnable()
    {
        StartCoroutine(OnEnable_Cor());
    }
    IEnumerator OnEnable_Cor()
    {
        yield return EnemyGenerator.instance;

        EnemyGenerator.instance.OnWaveStart += HealOnWaveStart_N_End;
        EnemyGenerator.instance.OnWaveEnd += HealOnWaveStart_N_End;
    }

    public void OnDisable()
    {
        EnemyGenerator.instance.OnWaveStart -= HealOnWaveStart_N_End;
        EnemyGenerator.instance.OnWaveEnd -= HealOnWaveStart_N_End;

        UnApplyDefenseBuff();
    }

    protected override void Search_Targets()//아군을 타겟으로 삼도록 재정의
    {
        targets.Clear();

        foreach (var Tile in detectRange_List.GetTileInRange(pawn.X, pawn.Y))
        {
            if (Tile.Ally != null) targets.Add(Tile.Ally);
        }
        isTargetDetected = targets.Count != 0;
    }

    public void HealOnWaveStart_N_End()
    {
        foreach (var ally in PawnGenerator.instance.SpawnedAllies)
        {
            ally.HealHP(WaveHealValue);
        }
    }

    public void GiveDefenseBuff(List<Vector2> aoeRange, float buffValue)
    {
        List<Unit> skillTargets = new();

        Vector2 AOEPos = new Vector2(pawn.X, pawn.Y);

        foreach (var _tile in aoeRange.GetTileInRange((int)AOEPos.x, (int)AOEPos.y))
        {
            if (_tile.Ally != null)
            {
                skillTargets.Add(_tile.Ally);
            }
        }

        foreach (var Ally in skillTargets)
        {
            Ally.TakeDefenseBuff(buffValue);
        }

        for (int i = 0; i < skillTargets.Count; i++)
        {
            if (skillTargets_Last.Contains(skillTargets[i]))
                skillTargets_Last.Remove(skillTargets[i]);
        }

        foreach (var unit_Last in skillTargets_Last)
        {
            unit_Last.isBuff = false;
        }

        skillTargets_Last = skillTargets;
    }

    public void UnApplyDefenseBuff()
    {
        foreach (var target in skillTargets_Last)
        {
            target.isBuff = false;
        }
    }

}
