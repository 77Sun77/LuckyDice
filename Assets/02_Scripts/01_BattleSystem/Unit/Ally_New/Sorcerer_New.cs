using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer_New : Ally
{
    [Header("Sorcerer_R1")]
    public GameObject FireBall;
    public int MaxExploStack;
    public List<Vector2> explosionRange;
    public float explosionFCTR;

    [Header("Sorcerer_R2")]
    public float SlowValue;
    public float SlowDuration;

    [Header("Sorcerer_R3")]
    public List<Vector2> MeteorRange;
    public float DotDmgFCTR;
    public float DotDmgDuration;
    public float DotDmgTick;

    new void Update()
    {
        switch (GameManager.instance.unitUpgrade[1]) // 업글 능력치 적용
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
        base.Update();
    }

    protected override bool TryAttack()
    {
        switch(Rating)
        {
            case 1:
            case 2:
                Projectile_Attack(damage,FireBall).GetComponent<FireBall>().SetFireBall(MaxExploStack, explosionRange, explosionFCTR, 0, 0);
                return true;
            case 3:
                Vector2 targetPos = new Vector2(this.GetClosestTarget(this.targets).pawn.X, this.GetClosestTarget(this.targets).pawn.Y);
                AOE_Attack(damage,targetPos, MeteorRange);

                foreach (var unit in AOE_Attack(damage,targetPos, MeteorRange))
                {
                    Enemy enemy = unit.GetComponent<Enemy>();

                    enemy.TakeDamage(damage, this.gameObject);
                    enemy.GetSlow(SlowValue, SlowDuration);
                    enemy.TakeDotDamage(damage * DotDmgFCTR, DotDmgDuration, DotDmgTick);
                }
                return true;
        }

        return false;
    }
}
