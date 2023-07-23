using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : Ally
{
    [Header("Buffer")]
    public float buffValue;
    public float WaveHealValue;
    public List<Vector2> AOERange;

    List<Unit> skillTargets_Last = new();

    private void Start()
    {
        first_Setting();
    }

    new void Update()
    {
        Search_Targets();

        switch (Rating)
        {
            case 1:
                break;
            case 2:
                GiveDefenseBuff();
                break;
            case 3:
                GiveDefenseBuff();
                break;
        }

        if (GameManager.instance.IsInBattle)
        {
            HealAllies();
        }

        time -= Time.deltaTime;

        if (pawn.pastTile)
        {
            mySprite.sortingLayerName = (pawn.pastTile.Y + 1) + "_Hierarchy";
            if (shadowSR) shadowSR.sortingLayerName = mySprite.sortingLayerName;
        }
    }

    private void OnEnable()
    {
        switch (Rating)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                StartCoroutine(OnEnable_Cor());
                break;
        }
    }
    IEnumerator OnEnable_Cor()
    {
        yield return EnemyGenerator.instance;

        GameManager.instance.OnWaveStart += HealOnWaveStart_N_End;
        GameManager.instance.OnWaveEnd += HealOnWaveStart_N_End;
    }

    public void OnDisable()
    {
        GameManager.instance.OnWaveStart -= HealOnWaveStart_N_End;
        GameManager.instance.OnWaveEnd -= HealOnWaveStart_N_End;

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

    public void HealAllies()
    {
        if (!isTargetDetected || time >= 0)
            return;

        time = delayTime;

        List<Unit> skillTargets = new();

        Vector2 AOEPos = new Vector2(pawn.X, pawn.Y);

        foreach (var _tile in AOERange.GetTileInRange((int)AOEPos.x, (int)AOEPos.y))
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

    public void HealOnWaveStart_N_End()
    {
        foreach (var ally in GameManager.instance.SpawnedAllies)
        {
            ally.HealHP(WaveHealValue);
        }
    }


    public void GiveDefenseBuff()
    {
        List<Unit> skillTargets = new();

        Vector2 AOEPos = new Vector2(pawn.X, pawn.Y);

        foreach (var _tile in AOERange.GetTileInRange((int)AOEPos.x, (int)AOEPos.y))
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
