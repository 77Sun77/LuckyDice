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

    public bool IsEnemy;
    public bool IsOverCenter;

    public bool IsGrabbed;
    public bool IsMoveGrid;
    public int X, Y;

    public Action OnInitialize_SetTile;

    private void Awake()
    {
        OnTileChanged += AddTilePawn;
        OnTileChanged += RemoveTilePawn;

        if (gameObject.TryGetComponent(out Unit _unit))
        {
            unit = _unit;
            IsEnemy = _unit.isEnemy;
        }
    }

    private void Update()
    {
        if (curTile != null && !IsEnemy) unit.enabled = !curTile.IsTable;
        
        if (IsEnemy) Set_CurTile(); //Enemy�� �ڵ��̵��� ������ �ڵ� ����,Unit�� Ŭ���� ���ؼ� ����

        CheckCenter();

        //����׶� �̸� ��ġ�� Ally�� Tile�� �����Ű�� ����
        if (Input.GetKey(KeyCode.P))
        {
            Set_CurTile();
            AddTilePawn();
            Debug.Log("�ʱ�ȭ��");
        }
    }

    public void Set_CurTile()
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

    public void Set_PastTile()
    {
        if(curTile!=null) pastTile = curTile;
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
            if(!curTile.EnemyList.Contains(unit))
            curTile.EnemyList.Add(unit);
        }
        else Debug.Log("Pawn���� ĳ���� ��ũ���͸� ã�� �� �����ϴ�");
    }

    public void RemoveTilePawn()
    {
        if (pastTile == null)
            return;

        if(!IsEnemy) pastTile.TileUnit = null;
        else pastTile.EnemyList.Remove(unit);
    }

    public void MoveToTargetTile(Tile targetTile)
    {
        Set_PastTile();
        transform.position = targetTile.GetPos();
        Set_CurTile();
    }

}
