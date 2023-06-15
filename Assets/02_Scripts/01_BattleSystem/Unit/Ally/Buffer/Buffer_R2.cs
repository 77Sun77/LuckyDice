using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer_R2 : Ally
{
    [Header("Buffer_R2")]
    public List<Vector2> AOERange;
    public float DefenseBuffValue;

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

    public void OnDisable()
    {
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
