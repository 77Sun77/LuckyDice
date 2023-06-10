using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    public List<Vector2> explosionRange;
    public float DmgIncrFCTR;

    public bool HasSlowDebuff;
    public float SlowValue;
    public float SlowDuration;
    
    public override void OnAttack(Enemy enemy)
    {
        enemy.explosionStack++;
        if(HasSlowDebuff) enemy.GetSlow(SlowValue, SlowDuration);

        if (enemy.explosionStack >= 4)
        {
            DoExplosionAttack(enemy);
            enemy.explosionStack = 0;
        }
        else enemy.TakeDamage(damage);
      
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
                unit.TakeDamage(damage * DmgIncrFCTR);
            }
        }
    }

}
