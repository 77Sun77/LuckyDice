using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    private Tile pastTile;
    private Tile curTile;
    private Action OnTileChanged;

    public bool IsOverCenter;

    public bool IsMoveGrid;
    public int X, Y;

    private void Awake()
    {
        OnTileChanged += AddTilePawn;
        OnTileChanged += RemoveTilePawn;
    }

    private void Update()
    {
        ShotRay();
        CheckCenter();
    }

    void ShotRay()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Tile"));
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.zero, 0f, layerMask);

        if (raycastHit.collider)
        {
            //Debug.Log(raycastHit.collider.gameObject);
            raycastHit.collider.transform.TryGetComponent(out curTile);
            if (curTile != pastTile) OnTileChanged.Invoke();
            pastTile = curTile;
            X = curTile.X;
            Y = curTile.Y;
        }
        else Debug.Log("None Obj");
    }

    void CheckCenter()
    {
        IsOverCenter = transform.position.x < curTile.transform.position.x;
    }


    public void AddTilePawn()
    {
        if (gameObject.TryGetComponent(out Unit unit))
        {
            curTile.TileUnit = unit;
        }
        else if (gameObject.TryGetComponent(out Enemy enemy))
        {
            if(!curTile.EnemyList.Contains(enemy))
            curTile.EnemyList.Add(enemy);
        }
        else Debug.Log("Pawn에서 캐릭터 스크립터를 찾을 수 없습니다");
    }

    public void RemoveTilePawn()
    {
        if (pastTile == null)
            return;

        pastTile.TileUnit = null;
        pastTile.EnemyList.Clear();
    }

}
