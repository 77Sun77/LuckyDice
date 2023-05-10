using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int X { get; set; }
    public int Y { get; set; }

    public Unit TileUnit;
    public List<Enemy> EnemyList = new();
    
    public bool IsTable;
    public bool CanPlacement;

    /// <summary>
    /// Tile의 X,Y값 할당
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Initialize_Tile(int x, int y)
    {
        X = x;
        Y = y;
        CanPlacement = false;
    }

    public void Initialize_Table(int i)
    {
        CanPlacement = true;
        IsTable = true;
        X=i;
    }

    private void Update()
    {
        CanPlacement = EnemyList.Count == 0 && TileUnit == null;

        if (!TileManager.Instance.IsUpdatingTilePos)
            return;

        if(!IsTable) transform.position = TileManager.Instance.GetTilePos(X, Y);
        else if (IsTable) transform.position = TileManager.Instance.GetTablePos(X);

        transform.localScale = new Vector3(TileManager.Instance.XScale_Tile, TileManager.Instance.YScale_Tile, 1);
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }
}
