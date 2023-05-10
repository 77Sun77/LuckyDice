using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //public Unit unit;
    public GameObject Target;
    public float speed, damage;
    //public void Set_Projectile(Unit unit, float speed, float damage)
    //{
    //    //this.unit = unit;
    //    this.speed = speed;
    //    this.damage = damage;
    //    Destroy(gameObject, 5f);
    //}

    public void SetTarget(GameObject go)
    {
        Target = go;
    }


    private void OnEnable()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * TileManager.Instance.XScale_Tile * Time.deltaTime);

        if (!Target)
            return;
        LookTarget();
    }

    void LookTarget()
    {
        Vector3 lookVec = Target.transform.position - transform.position;
        transform.right = lookVec;
        //transform.rotation.SetLookRotation(Target.transform.position);
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
