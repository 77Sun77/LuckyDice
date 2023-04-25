using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPlacementManager : MonoBehaviour
{
    public GameObject selectTarget;
    public Pawn selectPawn;
    public bool IsHoldMouse;

    Vector3 clickPoint;
    Vector3 deltaVec;

    private void Update()
    {
        IsHoldMouse = Input.GetKey(KeyCode.Mouse0);//나중에 터치로 바꿀것

        if (IsHoldMouse)
            ShotRay();
    }

    void ShotRay()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D raycastHit = Physics2D.Raycast(mousePos,Vector2.zero,0f);

        if (raycastHit.collider)
        {
            Debug.Log(raycastHit.collider.gameObject);

            if (raycastHit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Pawn")))
            {
                selectTarget = raycastHit.collider.transform.parent.gameObject;
                selectPawn = selectTarget.GetComponent<Pawn>();
            }
        }
    }


}
