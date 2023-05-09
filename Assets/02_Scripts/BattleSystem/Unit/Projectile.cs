using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //public Unit unit;
    public float speed, damage;
    //public void Set_Projectile(Unit unit, float speed, float damage)
    //{
    //    //this.unit = unit;
    //    this.speed = speed;
    //    this.damage = damage;
    //    Destroy(gameObject, 5f);
    //}

    private void OnEnable()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            Enemy enemy = (Enemy)coll.GetComponent(typeof(Enemy));
            
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
