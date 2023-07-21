using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static List<Tile> GetTileInRange(this List<Vector2> v2_list, int targetX, int targetY)
    {
        List<Tile> TileList = new();

        foreach (Vector2 targetTile in v2_list)
        {
            int x = targetX + (int)targetTile.x;
            int y = targetY + (int)targetTile.y;
            if (!TileManager.Instance.IsRightRange(x, y)) continue;

            var Tile = TileManager.Instance.TileArray[x, y];
            TileList.Add(Tile);
        }

        return TileList;
    }

    public static Unit SpawnUnit(this GameObject prefab, Transform parent, Tile tile, List<Unit> unitList)
    {
        GameObject go = GameObject.Instantiate(prefab, parent);
        Unit unit = go.GetComponent<Unit>();
        unit.enabled = false;
        unit.SpawnHPBar();

        unitList.Add(unit);
        Pawn pawn = go.GetComponent<Pawn>();
        pawn.MoveToTargetTile(tile);
        pawn.isRegenerated = true;
        pawn.Set_CurTile();
        pawn.AddTilePawn();
        PawnPlacementManager.instance.createObj.Add(go);
        PawnPlacementManager.instance.Set_Target(go);
        return unit;
    }

    public static void SpawnItem(this GameObject prefab, Transform parent, Tile tile)
    {
        GameObject go = GameObject.Instantiate(prefab, parent);
        Pawn pawn = go.GetComponent<Pawn>();
        pawn.MoveToTargetTile(tile);
        pawn.isRegenerated = true;
        pawn.Set_CurTile();
        pawn.AddTilePawn();
        PawnPlacementManager.instance.createObj.Add(go);
        PawnPlacementManager.instance.Set_Target(go);

    }
}
