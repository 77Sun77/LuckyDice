using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public List<Vector2> AOERange_List;//광역 공격 범위
    public Pawn pawn;
    void Start()
    {
        pawn = GetComponent<Pawn>();
        Invoke("Attack", 1f); // 애니메이션 등을 표현할 시간 텀
    }

    void Update()
    {
        
    }

    public abstract void Attack();
}
