using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPawn : Pawn
{
    
    void Update()
    {
        Set_CurTile();
        CheckCenter();
    }
}
