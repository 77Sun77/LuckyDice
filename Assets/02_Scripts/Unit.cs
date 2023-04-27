using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float hp, damage, range;

    Enemy enemy;
    void Start()
    {
        
    }
    void Update()
    {
        Search();
    }

    void Search()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Unit"));
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, range, Vector2.zero, 0, layerMask);
        if (hit)
        {
            if (hit.collider.CompareTag("Enemy") && enemy == null) // && unit == null �߰�
            {
                this.enemy = (Enemy)hit.collider.GetComponent(typeof(Enemy)); // Ÿ���� ����
                // �ִϸ��̼� �ߵ�
                Attack(); // �ӽ�
            }
        }
        else
        {
            // �ִϸ��̼� �ʱ�ȭ
        }
    }

    public void Attack() // Ÿ�� Ÿ�� ����, �ִϸ��̼� �̺�Ʈ Ű������
    {
        enemy.HP -= damage;
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
}
