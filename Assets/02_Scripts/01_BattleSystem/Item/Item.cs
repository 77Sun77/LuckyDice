using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public List<Vector2> AOERange_List;//���� ���� ����
    public Pawn pawn;
    void Start()
    {
        pawn = GetComponent<Pawn>();
        Invoke("Attack", 1f); // �ִϸ��̼� ���� ǥ���� �ð� ��
    }

    void Update()
    {
        
    }

    public abstract void Attack();
}
