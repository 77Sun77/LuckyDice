using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject Target;
    public float speed, damage;
    bool isContact;
    public void SetProjectile(float dmg, GameObject go)
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
            if (isContact)
                return;

            isContact = true;

            if (Mathf.Abs(coll.gameObject.transform.position.x - Target.gameObject.transform.position.x) < 0.05f)
            {
                Enemy enemy = (Enemy)Target.GetComponent(typeof(Enemy));
                Debug.Log(enemy.gameObject.name);
                enemy.TakeDamage(damage);
                Destroy(gameObject);

            }
            else
            {
                Enemy enemy = (Enemy)coll.GetComponent(typeof(Enemy));
                Debug.Log(enemy.gameObject.name);
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
