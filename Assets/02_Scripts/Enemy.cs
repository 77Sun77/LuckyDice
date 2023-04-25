using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed, hp, damage, range;
    float maxSpeed;
    public enum State { Move, Attack };
    public State enemyStete;
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
            enemyStete = State.Attack;
            if (hit.collider.CompareTag("Player")) // && unit != null �߰�
            {
                // this.unit = (Unit)hit.GetComponent(typeof(Unit)); // Ÿ���� ����
                // �ִϸ��̼� �ߵ�
                Attack(); // �ӽ�
                enemyStete = State.Attack;
            }
        }
        else
        {
            enemyStete = State.Move;
        }
    }

    public void Attack() // Ÿ�� Ÿ�� ����
    {
        // unit.hp -= damage;
    }
}