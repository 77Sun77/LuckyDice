using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string name;
    public float damage, hp, defense, speed;
    public float delayTime, time;
    public float drop_Gold;

    public bool isAttack;

    Unit unit;

    public enum Debuff { None, Damage, Defense, Speed };
    public Debuff debuff;

    void Start()
    {
        
    }

    protected void Update()
    {
        Search();
        Move();

        if(isAttack)
        {
            if(time <= 0)
            {
                Attack();
                time = delayTime;
            }
            time -= Time.deltaTime;
        }

    }

    void Move()
    {
        if(!isAttack)
        {
            transform.Translate(Vector2.left * (speed/15) * Time.deltaTime);
        }
    }

    void Search()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Unit");
        RaycastHit2D hit = Physics2D.Raycast(transform.position+new Vector3(0.5f, 0, 0), Vector2.left, 1.75f, layerMask); // distance == 칸 x
        if (hit)
        {
            if (hit.collider.CompareTag("Unit") && unit == null) // && unit == null 추가
            {
                this.unit = (Unit)hit.collider.GetComponent(typeof(Unit)); // 타겟을 지정
                // 애니메이션 발동
                isAttack = true;
            }
        }
        else
        {
            isAttack = false;
            // 애니메이션 초기화
            time = delayTime;
        }
    }

    public void Attack() // 타겟 타입 구분, 애니메이션 이벤트 키프레임
    {
        unit.TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        damage -= defense;
        hp -= damage;
        if (hp <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Base"))
        {
            coll.gameObject.GetComponent<Base>().HP -= 1;
            Destroy(gameObject);
        }
    }
}
