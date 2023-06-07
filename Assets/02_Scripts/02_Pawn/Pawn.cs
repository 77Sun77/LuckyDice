using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public Tile pastTile;
    private Tile curTile;
    protected private Action OnTileChanged;

    Unit unit;
    Item item;

    public bool IsEnemy;
    public bool IsOverCenter;

    public bool IsGrabbed;
    public bool IsMoveGrid;
    public int X, Y;

    public Action OnInitialize_SetTile;

    public bool isSearch;

    public bool isRegenerated;

    public bool isItem;
    private void OnEnable()
    {
        OnTileChanged += RemoveTilePawn;
        OnTileChanged += AddTilePawn;

        if (gameObject.TryGetComponent(out Unit _unit))
        {
            unit = _unit;
            IsEnemy = _unit.isEnemy;

            if(!IsEnemy)
            unit.enabled = false;
        }
        if(gameObject.TryGetComponent(out Item _item))
        {
            item = _item;
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
            
            if(unit != null)
                unitKindString = unit.GetComponent<Ally>().unitKind.ToString();


            if (isItem && item != null)
            {
                unitKindString = item.GetComponent<Item>().itemKind.ToString();
                switch (unitKindString)
                {
                    case "HealPotion":
                        go = Instantiate(GoogleSheetManager.instance.HealPotion, transform.parent);
                        break;
                    case "BombExplosion":
                        go = Instantiate(GoogleSheetManager.instance.BombExplosion, transform.parent);
                        break;
                    case "CharacterMove":
                        go = Instantiate(GoogleSheetManager.instance.CharMove, transform.parent);
                        break;
                }
            }
            go.SetActive(false);
            go.transform.position = curPos;
            Pawn pawn = go.GetComponent<Pawn>();
            pawn.isRegenerated = true;
            pawn.Set_CurTile();
            if (!item)
            {
                pawn.AddTilePawn();
                PawnGenerator.instance.SpawnedAllies.Add(go.GetComponent<Unit>());
            }
            go.SetActive(true);
            Destroy(gameObject);
        }
        
        //Set_CurTile();
        //AddTilePawn();
    }

    //void SpawnPawn(string pawnName,int rating,out GameObject go)
    //{
    //    switch (pawnName)
    //    {
    //        case "Warrior":
    //            switch(rating)
    //            {
    //                case 1:
    //                    go = Instantiate(GoogleSheetManager.instance.Warrior, transform.parent);
    //                    break;
    //                case 2:
    //                    go = Instantiate(GoogleSheetManager.instance.Warrior2, transform.parent);
    //                    break;
    //                case 3:
    //                    go = Instantiate(GoogleSheetManager.instance.Warrior3, transform.parent);
    //                    break;
    //            }
               
    //            break;
    //        case "Archer":
    //            go = Instantiate(GoogleSheetManager.instance.Archer, transform.parent);
    //            break;
    //        case "Tanker":
    //            go = Instantiate(GoogleSheetManager.instance.Tanker, transform.parent);
    //            break;
    //        case "Sorcerer":
    //            go = Instantiate(GoogleSheetManager.instance.Sorcerer, transform.parent);
    //            break;
    //        case "Lancer":
    //            go = Instantiate(GoogleSheetManager.instance.Lancer, transform.parent);
    //            break;
    //        case "Buffer":
    //            go = Instantiate(GoogleSheetManager.instance.Buffer, transform.parent);
    //            break;
    //        case "ITEM":
    //            go = Instantiate(GoogleSheetManager.instance.Barrier, transform.parent);
    //            break;
    //    }
    //}


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
            if (curTile != pastTile && !isItem) OnTileChanged.Invoke();
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
   
    protected void CheckCenter()
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

        if(!IsEnemy && pastTile.Ally == unit) pastTile.Ally = null;
        else pastTile.EnemyList.Remove(unit);

    }

    public void MoveToTargetTile(Tile targetTile)
    {
        Set_PastTile();
        transform.position = targetTile.GetPos();
        Set_CurTile();
    }

}
