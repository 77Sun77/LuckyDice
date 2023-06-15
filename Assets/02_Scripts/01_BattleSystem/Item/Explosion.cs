using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Item
{
    public float damage;
    public override void Attack()
    {
        foreach (var Tile in AOERange_List.GetTileInRange(pawn.X, pawn.Y))
        {
            List<Unit> targets = new();
            if (Tile.EnemyList.Count != 0)
            {
                targets.AddRange(Tile.EnemyList);
            }

            foreach (Unit unit in targets)
            {
                unit.TakeDamage(damage,this.gameObject);
            }
        }
        
    }
}
