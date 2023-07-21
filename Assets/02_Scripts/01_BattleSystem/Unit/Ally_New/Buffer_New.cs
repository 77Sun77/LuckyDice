using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer_New : Ally
{
    List<Unit> skillTargets_Last = new();

    [Header("Buffer_R1")]
    public List<Vector2> AOERange;

    [Header("Buffer_R2")]
    public float DefenseBuffValue;

    [Header("Buffer_R3")]
    public float WaveHealValue;

    private void OnEnable()
    {
        base.OnEnable();
        switch (Rating)
        {
            case 1:
            case 2:
                break;
            case 3:
                StartCoroutine(OnEnable_Cor());
                break;
        }
        
    }
    IEnumerator OnEnable_Cor()
    {
        yield return GameManager.instance;

        GameManager.instance.OnWaveStart += HealOnWaveStart_N_End;
        GameManager.instance.OnWaveEnd += HealOnWaveStart_N_End;
    }
    public void HealOnWaveStart_N_End()
    {
        foreach (var ally in GameManager.instance.SpawnedAllies)
        {
            ally.HealHP(WaveHealValue);
        }
    }

    protected override void Update()
    {
        switch (GameManager.instance.unitUpgrade[4]) // 업글 능력치 적용
        {
            case 1:
                Upgrade(0, 0, 0);
                break;
            case 2:
                Upgrade(0, 0, 0);
                break;
            case 3:
                Upgrade(0, 0, 0);
                break;

        }

        time -= Time.deltaTime;

        Search_Targets();

        if (GameManager.instance.IsInBattle) HealAllies(AOERange);
      
        switch(Rating)
        {
            case 1:
                break;
            case 2:
            case 3:
                GiveDefenseBuff(AOERange, DefenseBuffValue);
                break;
        }

        SyncHPBar();

        CheckDefenseBuff();

        if (pawn.pastTile) mySprite.sortingLayerName = (pawn.pastTile.Y + 1) + "_Hierarchy";
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

    public void OnDisable()
    {
        switch (Rating)
        {
            case 1:
            case 2:
                break;
            case 3:
                GameManager.instance.OnWaveStart -= HealOnWaveStart_N_End;
                GameManager.instance.OnWaveEnd -= HealOnWaveStart_N_End;
                break;
        }
        
        UnApplyDefenseBuff();
    }
    public void UnApplyDefenseBuff()
    {
        foreach (var target in skillTargets_Last)
        {
            target.isBuff = false;
        }
    }


}
