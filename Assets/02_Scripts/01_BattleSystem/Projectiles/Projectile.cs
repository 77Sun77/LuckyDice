using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject Target;
    public float speed, damage;
    protected List<Enemy> HitEnemy = new();
    public bool IsGuided;
    public bool CanPass;
    public virtual void SetProjectile(float dmg, GameObject go)
    {
        damage = dmg;
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

        if(IsGuided) LookTarget();
    }

    void LookTarget()
    {
        Vector3 lookVec = Target.transform.position - transform.position;
        transform.right = lookVec;
        //transform.rotation.SetLookRotation(Target.transform.position);
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            Enemy enemy;

            if (Target != null && Mathf.Abs(coll.gameObject.transform.position.x - Target.gameObject.transform.position.x) < 0.05f)
            {
                enemy = Target.GetComponent<Enemy>();
                OnAttack(enemy);
            }
            else
            {
                enemy = coll.GetComponent<Enemy>();
                OnAttack(enemy);
            }
        }
    }

    public virtual void OnAttack(Enemy enemy)
    {
        if (HitEnemy.Contains(enemy))
            return;

        HitEnemy.Add(enemy);
        enemy.TakeDamage(damage,this.gameObject);
        if (!CanPass) Destroy(gameObject);
    }
}
