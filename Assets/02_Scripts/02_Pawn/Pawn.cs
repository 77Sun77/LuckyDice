using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public Tile pastTile;
    protected private Tile curTile;
    protected private Action OnTileChanged;

    Unit unit;
    Item item;
    Animator anim;

    public bool IsEnemy;
    public bool IsOverCenter;

    public bool IsGrabbed;
    public bool IsMoveGrid;
    public int X, Y;

    public Action OnInitialize_SetTile;

    public bool isSearch;

    public bool isRegenerated;

    public bool isItem;

    public Tile tempTile;
    private void Start()
    {
        if (!isRegenerated)
            StartCoroutine(PreSpawnedPawnInitialize());

        if(!isItem) anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void OnEnable()
    {
        OnTileChanged += RemoveTilePawn;
        OnTileChanged += AddTilePawn;

        if (gameObject.TryGetComponent(out Unit _unit))
        {
            unit = _unit;
            IsEnemy = _unit.isEnemy;

            if (!IsEnemy)
                unit.enabled = false;
        }
        if (gameObject.TryGetComponent(out Item _item))
        {
            item = _item;
        }
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
        yield return AllyGenerator.instance;
        Vector3 curPos = gameObject.transform.position;

        if (!IsEnemy)
        {
            
            GameObject go = null;

            

            if (isItem)
            {
                string itemKindString = item.GetComponent<Item>().itemKind.ToString();
                switch (itemKindString)
                {
                    case "Barrier":
                        go = GoogleSheetManager.instance.Barrier;
                        break;
                    case "HealPotion":
                        go = GoogleSheetManager.instance.HealPotion;
                        break;
                    case "BombExplosion":
                        go = GoogleSheetManager.instance.BombExplosion;
                        break;
                    case "CharacterMove":
                        go = GoogleSheetManager.instance.CharMove;
                        break;
                }
                AllyGenerator.instance.SpawnItem(gameObject, tempTile);
                /*go.SetActive(false);
                go.transform.position = curPos;
                Pawn pawn = go.GetComponent<Pawn>();
                pawn.isRegenerated = true;
                pawn.Set_CurTile();
                pawn.AddTilePawn();
                go.SetActive(true);*/
            }
            else if (unit != null)
            {
                Ally ally = unit.GetComponent<Ally>();
                AllyGenerator.instance.SpawnAlly(ally.allyKind, ally.Rating, tempTile);
                Destroy(GetComponent<Unit>().hPBar.gameObject);
            }

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (curTile != null && !IsEnemy)
        {
            unit.enabled = !curTile.IsTable;
            anim.enabled = !curTile.IsTable;
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
        if (curTile != null) pastTile = curTile;
    }

    protected void CheckCenter()
    {
        if (curTile)
            IsOverCenter = transform.position.x < curTile.transform.position.x;
    }

    public void AddTilePawn()
    {
        if (!IsEnemy)
        {
            if (isItem)
            {
                curTile.item = gameObject;
            }
            else
            {
                curTile.Ally = unit;
            }

        }
        else if (IsEnemy && !isItem)
        {
            if (!curTile.EnemyList.Contains(unit))
                curTile.EnemyList.Add(unit);
        }
        else Debug.Log("Pawn에서 캐릭터 스크립터를 찾을 수 없습니다");

    }

    public void RemoveTilePawn()
    {
        if (pastTile == null)
            return;

        if (isItem)
        {
            pastTile.item = null;
        }
        else
        {
            if (!IsEnemy && pastTile.Ally == unit) pastTile.Ally = null;
            else pastTile.EnemyList.Remove(unit);
        }


    }

    public void MoveToTargetTile(Tile targetTile)
    {
        Set_PastTile();
        transform.position = targetTile.GetPos();
        Set_CurTile();
    }

}
