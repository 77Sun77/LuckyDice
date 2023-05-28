using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPawn : Pawn
{
    private void OnEnable()
    {
        OnTileChanged += AddTilePawn;
        OnTileChanged += RemoveTilePawn;
        
    }
    void Update()
    {
        Set_CurTile();
    }
}
