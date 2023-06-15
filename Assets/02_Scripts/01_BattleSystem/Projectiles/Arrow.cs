using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    public override void OnAttack(Enemy enemy)
    {
        enemy.TakeDamage(damage,this.gameObject);

        if (!CanPass) Destroy(gameObject);
    }
}
