using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPotion : Item
{
    public override void Attack()
    {
        foreach (var Tile in AOERange_List.GetTileInRange(pawn.X, pawn.Y))
        {
            if (Tile.Ally != null)
            {
                Tile.Ally.HealHP(30); // 수치는 임시
            }
        }
       // Destroy(gameObject);
    }
}
