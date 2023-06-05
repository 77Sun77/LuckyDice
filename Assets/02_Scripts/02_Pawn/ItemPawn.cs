using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPawn : Pawn
{
    
    void Update()
    {
        Set_CurTile();
        CheckCenter();
        if (GetComponent<Animator>() && curTile != null && !IsGrabbed)
        {
            GetComponent<Animator>().enabled = !curTile.IsTable;
        }
    }
}
