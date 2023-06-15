using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer_R3 : Ally
{
    [Header("Sorcerer_R3")]
    public List<Vector2> MeteorRange;
    
    public float DotDmgFCTR;
    public float DotDmgDuration;
    public float DotDmgTick;

    public float SlowValue;
    public float SlowDuration;
    
    new void Update()
    {
        base.Update();
    }

    protected override bool TryAttack()
    {
        Vector2 targetPos = new Vector2(this.GetClosestTarget(this.targets).pawn.X, this.GetClosestTarget(this.targets).pawn.Y);

        AOE_Attack(targetPos,MeteorRange);

        foreach (var unit in AOE_Attack(targetPos, MeteorRange))
        {
            Enemy enemy = unit.GetComponent<Enemy>();

            enemy.TakeDamage(damage, this.gameObject);
            enemy.GetSlow(SlowValue, SlowDuration);
            enemy.TakeDotDamage(damage* DotDmgFCTR, DotDmgDuration, DotDmgTick);
        }

        return true;
    }


}
