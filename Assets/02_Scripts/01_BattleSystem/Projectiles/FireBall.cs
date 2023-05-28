using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    public List<Vector2> explosionRange; 

    public override void OnAttack(Enemy enemy)
    {
        enemy.explosionStack++;
        
        if (enemy.explosionStack >= 3)
        {
            DoExplosionAttack(enemy);
            enemy.explosionStack = 0;
        }

        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }

    void DoExplosionAttack(Enemy enemy)
    {
        foreach (var tile in explosionRange.GetTileInRange(enemy.pawn.X, enemy.pawn.Y))
        {
            List<Unit> targets = new();
            tile.Do_AOE_Effect(Color.red);
            
            if (tile.EnemyList.Count != 0)
            {
                targets.AddRange(tile.EnemyList);
            }

            foreach (Unit unit in targets)
            {
                unit.TakeDamage(damage);
            }
        }
    }

}
