using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public List<Vector2> AOERange_List;//광역 공격 범위
    public Pawn pawn;

    public enum ItemKind { Barrier ,HealPotion, BombExplosion, CharacterMove };
    public ItemKind itemKind;

    void Start()
    {
        pawn = GetComponent<Pawn>();
        pawn.isItem = true;
    }

    public abstract void Attack(); // 애니메이션 트리거를 통해 호출

    public void DestroyThis() // 애니메이션 트리거를 통해 호출
    {
        Destroy(gameObject);
    }
}
