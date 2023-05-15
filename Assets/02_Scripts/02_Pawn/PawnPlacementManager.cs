using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPlacementManager : MonoBehaviour
{
    public GameObject selectTarget;
    public Pawn selectPawn;
    public bool IsHoldingMouse;

    Vector3 clickPoint;
    Vector3 deltaVec;

    Vector3 pawnPos_OnClick;

    public GameObject DebugObj;

    private void Update()
    {
        IsHoldingMouse = Input.GetKey(KeyCode.Mouse0);//나중에 터치로 바꿀것

        if (IsHoldingMouse) SetSelectPawn();
        else if(!IsHoldingMouse && selectPawn)
        {
            if (selectPawn.IsMoveGrid) MoveOrSwitchPawn();
            ClearVars();
        }

        if (selectPawn)
        {
            GrabPawn();
        }
    }

    void SetSelectPawn()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int layerMask = (1 << LayerMask.NameToLayer("Pawn")) + (1 << LayerMask.NameToLayer("Enemy")) + (1 << LayerMask.NameToLayer("Unit"));
        RaycastHit2D raycastHit = Physics2D.Raycast(mousePos,Vector2.zero,0f, layerMask);

        if (raycastHit.collider)
        {
            //Debug.Log(raycastHit.collider.gameObject);

            if (selectPawn) return;

            selectTarget = raycastHit.collider.transform.gameObject;

            if (selectTarget.TryGetComponent(out Pawn pawn) && !pawn.IsEnemy) selectPawn = pawn;
            else selectPawn = selectTarget.transform.parent.GetComponent<Pawn>();
          
            clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pawnPos_OnClick = selectPawn.gameObject.transform.position;
        }
        //else Debug.Log("None Obj");
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
                Unit unit = tile.TileUnit;
                
                if (tile.CanPlacement)//빈자리인 경우
                {
                    selectPawn.transform.position = raycastHit.collider.gameObject.transform.position;
                    selectPawn.Set_CurTile();
                    selectPawn.IsGrabbed = false;
                    return;
                }
                else if (unit != null && unit.pawn != selectPawn)//다른 유닛이 있을 경우
                {
                    selectPawn.transform.position = raycastHit.collider.gameObject.transform.position;
                    unit.pawn.MoveToTargetTile(selectPawn.pastTile);
                    selectPawn.Set_CurTile();
                    selectPawn.IsGrabbed = false;
                    Debug.Log("Switch Positon");
                    return;
                }
            }   
        }
        selectPawn.transform.position = selectPawn.pastTile.GetPos();
    }

    void ClearVars()
    {
        selectTarget = null;
        selectPawn = null;

        clickPoint = Vector3.zero;
        deltaVec = Vector3.zero;
        pawnPos_OnClick = Vector3.zero;
    }

    void GrabPawn()
    {
        deltaVec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - clickPoint;
        selectPawn.gameObject.transform.position = pawnPos_OnClick + deltaVec;

        if (selectPawn.IsGrabbed == false)
        {
            selectPawn.Set_PastTile();
            selectPawn.IsGrabbed = true;
        }    

    }
}
