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
    private void Update()
    {
        IsHoldingMouse = Input.GetKey(KeyCode.Mouse0);//나중에 터치로 바꿀것

        if (IsHoldingMouse) ShotRay();
        else if(!IsHoldingMouse&&selectPawn)
        {
            MovePawnToGrid();
            ClearVars();
        }

        if (selectPawn)
        {
            deltaVec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - clickPoint;
            selectPawn.gameObject.transform.position = pawnPos_OnClick + deltaVec;
        }
    }

    void ShotRay()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int layerMask = 1 << LayerMask.NameToLayer("Pawn");
        RaycastHit2D raycastHit = Physics2D.Raycast(mousePos,Vector2.zero,0f, layerMask);

        if (raycastHit.collider)
        {
            //Debug.Log(raycastHit.collider.gameObject);

            if (selectPawn) return;

            selectTarget = raycastHit.collider.transform.parent.gameObject;
            selectPawn = selectTarget.GetComponent<Pawn>();
            clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pawnPos_OnClick = selectPawn.gameObject.transform.position;
        }
    }

    void MovePawnToGrid()
    {
        Vector2 pawnPos = selectPawn.gameObject.transform.position;
        int layerMask = 1 << LayerMask.NameToLayer("Tile");
        RaycastHit2D raycastHit = Physics2D.Raycast(pawnPos, Vector2.zero, 0f, layerMask);

        if (raycastHit.collider)
        {
            Debug.Log(raycastHit.collider.gameObject);

            if (raycastHit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Tile")))
            {
                selectPawn.transform.position = raycastHit.collider.gameObject.transform.position;
            }
        }
    }

    void ClearVars()
    {
        selectTarget = null;
        selectPawn = null;

        clickPoint = Vector3.zero;
        deltaVec = Vector3.zero;
        pawnPos_OnClick = Vector3.zero;
    }

}
