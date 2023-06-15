using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    public int MaxExploStack;

    public List<Vector2> explosionRange;
    public float explosionFCTR;

    public float SlowValue;
    public float SlowDuration;
    
    public void SetFireBall(int _maxExploStack, List<Vector2> _explosionRange, float _explosionFCTR, float _slowValue, float _slowDuration)
    {
        MaxExploStack = _maxExploStack;

        explosionRange = _explosionRange;
        explosionFCTR = _explosionFCTR;

        SlowValue = _slowValue;
        SlowDuration = _slowDuration;
    }

    public override void OnAttack(Enemy enemy)
    {
        enemy.explosionStack++;
        enemy.GetSlow(SlowValue, SlowDuration);

        if (enemy.explosionStack >= MaxExploStack)
        {
            DoExplosionAttack(enemy);
            enemy.explosionStack = 0;
        }
        else enemy.TakeDamage(damage,this.gameObject);
      
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
                unit.TakeDamage(damage * explosionFCTR,this.gameObject);
            }
        }
    }
}
