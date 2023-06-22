using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public List<Vector2> AOERange_List;//���� ���� ����
    public Pawn pawn;

    public enum ItemKind { Barrier ,HealPotion, BombExplosion, CharacterMove };
    public ItemKind itemKind;

    void Start()
    {
        pawn = GetComponent<Pawn>();
        pawn.isItem = true;
    }

    public abstract void Attack(); // �ִϸ��̼� Ʈ���Ÿ� ���� ȣ��

    public void DestroyThis() // �ִϸ��̼� Ʈ���Ÿ� ���� ȣ��
    {
        Destroy(gameObject);
    }
}
