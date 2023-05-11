using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public Tile pastTile;
    private Tile curTile;
    private Action OnTileChanged;

    Unit unit;
    Enemy enemy;

    public bool IsEnemy;
    public bool IsOverCenter;

    public bool IsGrabbed;
    public bool IsMoveGrid;
    public int X, Y;
    
    private void Awake()
    {
        OnTileChanged += AddTilePawn;
        OnTileChanged += RemoveTilePawn;

        if (gameObject.TryGetComponent(out Unit _unit))
        {
            unit = _unit;
            IsEnemy = false;
        }
        else if (gameObject.TryGetComponent(out Enemy _enemy))
        {
            enemy = _enemy;
            IsEnemy = true;
        }
    }

    private void Update()
    {
        if (curTile != null && !IsEnemy) unit.enabled = !curTile.IsTable;
        
        if (IsEnemy) ShotRay_Enemy(); //Enemy�� �ڵ��̵��� ������ �ڵ� ����,Unit�� Ŭ���� ���ؼ� ����

        CheckCenter();
    }

    void ShotRay_Enemy()
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

    public void OnClick_Unit()
    {
        if(curTile!=null) pastTile = curTile;
        IsGrabbed = true;
    }
    public void OnDrop_Unit()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Tile"));
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.zero, 0f, layerMask);

        if (raycastHit.collider)
        {
            //Debug.Log(raycastHit.collider.gameObject);
            raycastHit.collider.transform.TryGetComponent(out curTile);
            if (curTile != pastTile) OnTileChanged.Invoke();

            X = curTile.X;
            Y = curTile.Y;
        }

        IsGrabbed = false;
    }

    void CheckCenter()
    {
        if(curTile)
        IsOverCenter = transform.position.x < curTile.transform.position.x;
    }

    public void AddTilePawn()
    {
        if (!IsEnemy)
        {
            curTile.TileUnit = unit;
        }
        else if (IsEnemy)
        {
            if(!curTile.EnemyList.Contains(enemy))
            curTile.EnemyList.Add(enemy);
        }
        else Debug.Log("Pawn���� ĳ���� ��ũ���͸� ã�� �� �����ϴ�");
    }

    public void RemoveTilePawn()
    {
        if (pastTile == null)
            return;

        if(!IsEnemy) pastTile.TileUnit = null;
        else pastTile.EnemyList.Remove(enemy);
    }

    public void MoveToTargetTile(Tile targetTile)
    {
        OnClick_Unit();
        transform.position = targetTile.GetPos();
        OnDrop_Unit();
    }


}
