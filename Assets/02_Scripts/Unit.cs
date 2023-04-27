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
            if (hit.collider.CompareTag("Enemy") && enemy == null) // && unit == null 추가
            {
                this.enemy = (Enemy)hit.collider.GetComponent(typeof(Enemy)); // 타겟을 지정
                // 애니메이션 발동
                Attack(); // 임시
            }
        }
        else
        {
            // 애니메이션 초기화
        }
    }

    public void Attack() // 타겟 타입 구분, 애니메이션 이벤트 키프레임
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
