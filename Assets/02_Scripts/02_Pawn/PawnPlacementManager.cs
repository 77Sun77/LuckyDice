using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PawnPlacementManager : MonoBehaviour
{
    public static PawnPlacementManager instance;
    public GameObject selectTarget;
    public Pawn selectPawn;
    public bool IsHoldingMouse, isInventoryHold, isActive, DiceControlOnOff;
    public bool IsDebugMode;

    Vector3 clickPoint;
    Vector3 deltaVec;

    Vector3 pawnPos_OnClick;

    Tile origin;
    Transform target;

    public GameObject DebugObj;

    public GameObject canvas;
    public GraphicRaycaster gr;
    public PointerEventData pe;
    public EventSystem es;
    bool isTrigger, AllDstroy, Disable;

    float timer;
    public GameObject ObjTemp;

    public List<GameObject> createObj = new List<GameObject>();

    Vector3 inventoryPos;

    GameObject tileTemp;
    private void Start()
    {
        instance = this;


    }
    private void Update()
    {
        IsHoldingMouse = Input.GetKey(KeyCode.Mouse0);//나중에 터치로 바꿀것

        if (isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Touch();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Touch(true);
            }
        }

        if (IsHoldingMouse) SetSelectPawn();
        else if (!IsHoldingMouse)
        {

            if (selectPawn)
            {
                if (selectPawn.IsMoveGrid) MoveOrSwitchPawn();
                ClearVars();
                isActive = false;
            }
            isTrigger = false;
            List<GameObject> DestroyObj = new List<GameObject>();
            if (AllDstroy)
            {
                foreach (GameObject go in createObj)
                {
                    Destroy(go);
                }
                AllDstroy = false;
                return;
            }
            if (createObj.Count != 0) UIManager.instance.UI.SetActive(true);
            foreach (GameObject go in createObj)
            {
                if (go && go.TryGetComponent(out Pawn pawn) && (pawn.pastTile.CanPlacement || pawn.isItem) && !pawn.pastTile.IsTable)
                {

                    continue;
                }
                DestroyObj.Add(go);
            }
            foreach (GameObject go in DestroyObj) Destroy(go);

            createObj.Clear();
            timer = 0.2f;
            Disable = false;
            isInventoryHold = false;
            TileManager.Instance.ResetTile();
            if (!isActive && !DiceControlOnOff) ObjTemp = null;
        }

        if (selectPawn)
        {
            GrabPawn();
            if (Input.GetKeyDown(KeyCode.Mouse1)) Destroy(selectPawn.gameObject);
        }

    }

    void SetSelectPawn()
    {
        if (!isInventoryHold)
        {
            timer -= Time.deltaTime;
            pe = new PointerEventData(es);
            pe.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pe, results);
            if (results.Count != 0)
            {
                GameObject go = null;
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("Inventory"))
                    {
                        go = result.gameObject;
                        break;
                    }
                }
                #region
                /*
                if (go && !ObjTemp)
                {
                    ObjTemp = go;
                    inventoryPos = ObjTemp.transform.parent.position;
                }
                else if (!go || go != ObjTemp || (ObjTemp && Vector3.Distance(inventoryPos, ObjTemp.transform.parent.position) >= 20))
                {
                    Disable = true;
                    ObjTemp = null;
                }*/
                #endregion
                if (ObjTemp && go != ObjTemp)
                {
                    if (go)
                    {
                        go.GetComponent<Inventory_Prefab>().OnClick_Btn();
                    }

                    timer = 0.2f;
                    Disable = true;
                }
                if (ObjTemp && ObjTemp.GetComponent<Inventory_Prefab>().Kind == Inventory_Prefab.Obj_Kind.Dice) Disable = true;
            }

        }

        if (!isTrigger && timer < 0 && !Disable)
        {
            if (ObjTemp && ObjTemp.GetComponent<Inventory_Prefab>().Kind != Inventory_Prefab.Obj_Kind.Dice)
            {
                Inventory_Prefab contents = ObjTemp.GetComponent<Inventory_Prefab>();
                Tile tile = Instantiate(TileManager.Instance.Tile_Prefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity).GetComponent<Tile>();
                tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, 0);
                tile.GetComponent<SpriteRenderer>().enabled = false;
                tile.IsTable = true;

                GameObject go;
                if (contents.Kind == Inventory_Prefab.Obj_Kind.Unit) go = SpawnUnitByName(contents.objectType, contents.Rating, tile);
                else go = Instantiate(contents.prefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

                Debug.Log("SetSelectPawn");

                Pawn pawn = go.GetComponent<Pawn>();
                pawn.tempTile = tile;
                createObj.Add(tile.gameObject);
                createObj.Add(pawn.gameObject);

                UIManager.instance.UI.SetActive(false);
                UIManager.instance.UnActive_StorePanel();

                clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pawnPos_OnClick = Camera.main.ScreenToWorldPoint(ObjTemp.transform.position);

                isTrigger = true;
                isInventoryHold = true;
                return;
            }

        }


        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int layerMask = (1 << LayerMask.NameToLayer("Pawn")) + (1 << LayerMask.NameToLayer("Enemy")) + (1 << LayerMask.NameToLayer("Unit")) + (1 << LayerMask.NameToLayer("Item"));
        RaycastHit2D raycastHit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, layerMask);

        if (raycastHit.collider)
        {
            //Debug.Log(raycastHit.collider.gameObject);


            if (selectPawn)
            {
                return;
            }

            selectTarget = raycastHit.collider.transform.gameObject;

            if (!IsDebugMode)
            {
                if (selectTarget.TryGetComponent(out Ally unit)) // 맵에 있는 유닛의 움직임을 제한하는 코드
                {
                    if (unit.isMove || (unit.GetComponent<Pawn>().pastTile && unit.GetComponent<Pawn>().pastTile.IsTable))
                    {
                        unit.isMove = false;
                    }
                    else // 제한시 주석 해제
                        return;
                }
            }

            if (selectTarget.TryGetComponent(out Pawn pawn)) selectPawn = pawn;
            else selectPawn = selectTarget.transform.parent.GetComponent<Pawn>();

            origin = selectPawn.pastTile;
            target = selectTarget.transform;

            clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pawnPos_OnClick = selectPawn.gameObject.transform.position;


        }
        //else Debug.Log("None Obj");


    }

    void Touch(bool isUp = false)
    {
        pe = new PointerEventData(es);
        pe.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pe, results);
        if (results.Count != 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Inventory"))
                {
                    return;

                }
            }
        }

        clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int layerMask = 1 << LayerMask.NameToLayer("Tile");
        RaycastHit2D raycastHit = Physics2D.Raycast(clickPoint, Vector2.zero, 0f, layerMask);

        if (raycastHit.collider)
        {
            if (!isUp)
            {
                isInventoryHold = true;
                Disable = true;
                tileTemp = raycastHit.collider.gameObject;

            }
            else if (tileTemp == raycastHit.collider.gameObject)
            {
                Inventory_Prefab temp = ObjTemp.GetComponent<Inventory_Prefab>();
                if (temp.Kind == Inventory_Prefab.Obj_Kind.Dice)
                {
                    temp.OnClick_Dice();
                    //temp.OnClick_Btn();
                    isActive = false;
                    DiceControlOnOff = true;
                    return;
                }

                bool isItem = temp.prefab.GetComponent<Pawn>().isItem;
                if (!isItem && (tileTemp.GetComponent<Tile>().Ally || tileTemp.GetComponent<Tile>().EnemyList.Count != 0)) return;
                else if (isItem && temp.prefab.GetComponent<Item>().itemKind == Item.ItemKind.CharacterMove && !tileTemp.GetComponent<Tile>().Ally) return;



                GameObject go = null;
                if (temp.Kind == Inventory_Prefab.Obj_Kind.Unit) go = SpawnUnitByName(temp.objectType, temp.Rating, tileTemp.GetComponent<Tile>());
                else go = Instantiate(temp.prefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

                Pawn pawn = go.GetComponent<Pawn>();
                //pawn.MoveToTargetTile(tileTemp.GetComponent<Tile>());

                createObj.Add(pawn.gameObject);
                selectPawn = pawn;
                selectPawn.Set_PastTile();
                selectPawn.Set_CurTile();
                selectPawn.tempTile = tileTemp.GetComponent<Tile>();
                //GameManager.instance.inventory.Delete_Inventory(ObjTemp);
            }

        }
    }
    public void Set_Target(GameObject go)
    {
        selectTarget = go;

        if (selectTarget.TryGetComponent(out Pawn pawn)) selectPawn = pawn;
        else selectPawn = selectTarget.transform.parent.GetComponent<Pawn>();

        origin = selectPawn.pastTile;
        target = selectTarget.transform;
    }
    void MoveOrSwitchPawn()
    {
        Vector2 pawnPos = selectPawn.gameObject.transform.position;
        int layerMask = 1 << LayerMask.NameToLayer("Tile");
        RaycastHit2D raycastHit = Physics2D.Raycast(pawnPos, Vector2.zero, 0f, layerMask);

        if (raycastHit.collider)
        {
            //Debug.Log(raycastHit.collider.gameObject);
            if (raycastHit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Tile")))
            {
                var tile = raycastHit.collider.transform.GetComponent<Tile>();
                Unit unit = tile.Ally;
                if (tile.EnemySpawn)
                {
                    if (!selectPawn.isItem)
                    {
                        selectPawn.GetComponent<Ally>().isMove = true;
                        CancelPawn();
                        AllDstroy = true;
                    }
                    else
                    {
                        AllDstroy = true;
                    }


                    return;
                }



                if (selectPawn.GetComponent<CharacterMove>() && unit)
                {
                    tile.Ally.GetComponent<Ally>().isMove = true;
                    GameManager.instance.inventory.Delete_Inventory(ObjTemp);
                    AllDstroy = true;
                    return;
                }

                if (!selectPawn.isItem && unit == selectPawn.GetComponent<Unit>())
                {
                    selectPawn.IsGrabbed = false;
                    tile.Ally.GetComponent<Ally>().isMove = true;
                }

                if ((tile.CanPlacement || selectPawn.isItem) && !tile.IsTable && !tile.EnemySpawn)//빈자리인 경우
                {
                    if (selectPawn.GetComponent<CharacterMove>())
                    {
                        AllDstroy = true;
                        return;
                    }
                    selectPawn.transform.position = raycastHit.collider.gameObject.transform.position;
                    selectPawn.Set_CurTile();
                    selectPawn.IsGrabbed = false;
                    GameManager.instance.inventory.Delete_Inventory(ObjTemp);

                    return;
                }
                else if (unit != null && unit.pawn != selectPawn)//다른 유닛이 있을 경우
                {

                    if (selectPawn.GetComponent<Unit>() && selectPawn.GetComponent<Pawn>().pastTile.IsTable) // 인벤토리 유닛과 맵 유닛의 교환을 제한하는 코드
                    {
                        return;
                    }

                    selectPawn.transform.position = raycastHit.collider.gameObject.transform.position;
                    unit.pawn.MoveToTargetTile(selectPawn.pastTile);
                    selectPawn.Set_PastTile();
                    selectPawn.Set_CurTile();
                    selectPawn.IsGrabbed = false;
                    Debug.Log("Switch Positon");
                    return;
                }


            }
        }
        AllDstroy = true;
        selectPawn.transform.position = selectPawn.pastTile.GetPos();
        selectPawn = null;
    }

    void ClearVars()
    {
        selectTarget = null;
        selectPawn = null;

        clickPoint = Vector3.zero;
        deltaVec = Vector3.zero;
        pawnPos_OnClick = Vector3.zero;

        origin = null;
        target = null;
    }

    void GrabPawn()
    {
        deltaVec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - clickPoint;
        Vector3 vec = pawnPos_OnClick + deltaVec;
        vec.z = 0;
        selectPawn.gameObject.transform.position = vec;

        if (selectPawn.IsGrabbed == false)
        {
            selectPawn.Set_PastTile();
            selectPawn.IsGrabbed = true;
        }

    }

    void CancelPawn()
    {
        target.transform.position = origin.transform.position;
        ClearVars();
    }

    GameObject SpawnUnitByName(string unitName, int rating, Tile tile)
    {
        return AllyGenerator.instance.SpawnAlly((AllyKind)Enum.Parse(typeof(AllyKind), unitName), rating, tile);
    }

}
