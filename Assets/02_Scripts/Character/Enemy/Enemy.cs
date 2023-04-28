using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed, hp, damage, range;
    float maxSpeed;
    public enum State { Move, Attack };
    public State enemyStete;

    Unit unit;

    enum TargetType { Unit, Base }
    TargetType target;
    void Start()
    {
        maxSpeed = speed;
    }

    void Update()
    {
        Search();
        Move();
    }

    void Move()
    {
        if(enemyStete == State.Move)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    void Search()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Enemy"));
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, range, Vector2.zero,0, layerMask);
        if (hit)
        {
            if (hit.collider.CompareTag("Unit") && unit == null) // && unit == null 추가
            {
                this.unit = (Unit)hit.collider.GetComponent(typeof(Unit)); // 타겟을 지정
                // 애니메이션 발동
                Attack(); // 임시
                enemyStete = State.Attack;
            }
        }
        else
        {
            enemyStete = State.Move;
            // 애니메이션 초기화
        }
    }

    public void Attack() // 타겟 타입 구분, 애니메이션 이벤트 키프레임
    {
        unit.HP -= damage;
    }

    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp <= 0) Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Base"))
        {
            coll.gameObject.GetComponent<Base>().HP -= 1;
            Destroy(gameObject);
        }
    }
}
