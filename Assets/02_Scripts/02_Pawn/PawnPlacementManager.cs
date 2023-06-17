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
    
    Tile origin;
    Transform target;

    public GameObject DebugObj;

    private void Update()
    {
        IsHoldingMouse = Input.GetKey(KeyCode.Mouse0);//���߿� ��ġ�� �ٲܰ�

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

        int layerMask = (1 << LayerMask.NameToLayer("Pawn")) + (1 << LayerMask.NameToLayer("Enemy")) + (1 << LayerMask.NameToLayer("Unit")) + (1 << LayerMask.NameToLayer("Item"));
        RaycastHit2D raycastHit = Physics2D.Raycast(mousePos,Vector2.zero,0f, layerMask);

        if (raycastHit.collider)
        {
            //Debug.Log(raycastHit.collider.gameObject);

            if (selectPawn) return;

            selectTarget = raycastHit.collider.transform.gameObject;

            if (selectTarget.TryGetComponent(out Ally unit)) // �ʿ� �ִ� ������ �������� �����ϴ� �ڵ�
            {
                if (unit.isMove || unit.GetComponent<Pawn>().pastTile.IsTable)
                {
                    unit.isMove = false;
                }
                //else // ���ѽ� �ּ� ����
                //    return;
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
                
                if (tile.CanPlacement)//���ڸ��� ���
                {
                    selectPawn.transform.position = raycastHit.collider.gameObject.transform.position;
                    
                    selectPawn.Set_CurTile();
                    selectPawn.IsGrabbed = false;
                    
                    return;
                }
                else if (unit != null && unit.pawn != selectPawn)//�ٸ� ������ ���� ���
                {
                    /*
                    if (selectPawn.GetComponent<Unit>() && !selectPawn.GetComponent<Unit>().enabled) // �κ��丮 ���ְ� �� ������ ��ȯ�� �����ϴ� �ڵ�
                    {
                        CancelPawn();
                        return;
                    }*/
                    selectPawn.transform.position = raycastHit.collider.gameObject.transform.position;
                    unit.pawn.MoveToTargetTile(selectPawn.pastTile);
                    selectPawn.Set_PastTile();
                    selectPawn.Set_CurTile();
                    selectPawn.IsGrabbed = false;
                    
                    Debug.Log("Switch Positon");
                    return;
                }
                else if(tile.EnemyList.Count > 0)
                {
                    selectPawn.IsGrabbed = false;
                }
                else if (selectPawn.isItem)
                {
                    selectPawn.IsGrabbed = false;
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

        origin = null;
        target = null;
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

    void CancelPawn()
    {
        target.transform.position = origin.transform.position;
        ClearVars();
    }
}
