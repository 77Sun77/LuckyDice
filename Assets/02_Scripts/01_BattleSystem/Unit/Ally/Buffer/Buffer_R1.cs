using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer_R1 : Ally
{
    [Header("Buffer_R1")]
    public List<Vector2> AOERange;
  
    protected override void Update()
    {
        time -= Time.deltaTime;

        Search_Targets();
        
        if (GameManager.instance.IsInBattle)
        {
            HealAllies(AOERange);
        }

        SyncHPBar();

        CheckDefenseBuff();
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

   
}
