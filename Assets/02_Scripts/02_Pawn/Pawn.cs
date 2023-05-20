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

    public bool isSearch;

    public bool isRegenerated;
    private void OnEnable()
    {
        OnTileChanged += AddTilePawn;
        OnTileChanged += RemoveTilePawn;

        if (gameObject.TryGetComponent(out Unit _unit))
        {
            unit = _unit;
            IsEnemy = _unit.isEnemy;
        }
        if(!isRegenerated)
        StartCoroutine(PreSpawnedPawnInitialize());
    }

    /// <summary>
    /// 디버깅때 미리 배치된 Ally를 Tile에 적용시키기 위함
    /// </summary>
    /// <returns></returns>
    IEnumerator PreSpawnedPawnInitialize()
    {
        yield return TileManager.Instance;
        yield return TileManager.Instance.IsMapGeneratingOver;
        yield return GoogleSheetManager.instance;
        yield return PawnGenerator.instance;
        Vector3 curPos = gameObject.transform.position;
        string unitKindString = "";
        
        if (!IsEnemy)
        {
            GameObject go = null;
            unitKindString = unit.GetComponent<Ally>().unitKind.ToString();

            switch(unitKindString)
            {
                case "Warrior":
                    go = Instantiate(GoogleSheetManager.instance.Warrior, PawnGenerator.instance.UnitSpawn_Tf);
                    break;
                case "Archer":
                    go = Instantiate(GoogleSheetManager.instance.Archer, PawnGenerator.instance.UnitSpawn_Tf);
                    break;
                case "Tanker":
                    go = Instantiate(GoogleSheetManager.instance.Tanker, PawnGenerator.instance.UnitSpawn_Tf);
                    break;
                case "Sorcerer":
                    go = Instantiate(GoogleSheetManager.instance.Sorcerer, PawnGenerator.instance.UnitSpawn_Tf);
                    break;
                case "Debuffer":
                    go = Instantiate(GoogleSheetManager.instance.Debuffer, PawnGenerator.instance.UnitSpawn_Tf);
                    break;
                case "Buffer":
                    go = Instantiate(GoogleSheetManager.instance.Buffer, PawnGenerator.instance.UnitSpawn_Tf);
                    break;
            }
            go.SetActive(false);
            go.transform.position = curPos;
            Pawn pawn = go.GetComponent<Pawn>();
            pawn.isRegenerated = true;
            pawn.Set_CurTile();
            pawn.AddTilePawn();
            go.SetActive(true);
            Destroy(gameObject);
        }
        
        //Set_CurTile();
        //AddTilePawn();
    }

    private void Update()
    {
        if (curTile != null && !IsEnemy)
        {
            unit.enabled = !curTile.IsTable;
            /*
            if (!isSearch && unit.enabled)
            {
                GameManager.instance.us.Search();
                isSearch = true;
            }*/
        }
        
        if (IsEnemy) Set_CurTile(); //Enemy는 자동이동을 함으로 자동 갱신,Unit은 클릭에 의해서 갱신

        CheckCenter();
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
            curTile.Ally = unit;
        }
        else if (IsEnemy)
        {
            if(!curTile.EnemyList.Contains(unit))
            curTile.EnemyList.Add(unit);
        }
        else Debug.Log("Pawn에서 캐릭터 스크립터를 찾을 수 없습니다");
    }

    public void RemoveTilePawn()
    {
        if (pastTile == null)
            return;

        if(!IsEnemy) pastTile.Ally = null;
        else pastTile.EnemyList.Remove(unit);
    }

    public void MoveToTargetTile(Tile targetTile)
    {
        Set_PastTile();
        transform.position = targetTile.GetPos();
        Set_CurTile();
    }

}
