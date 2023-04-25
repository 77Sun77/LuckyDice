using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    /// <summary>
    /// Tile의 X,Y값 할당
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Initialize_Tile(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    private void Update()
    {
        transform.position = TileManager.Instance.GetTilePos(X, Y);
        transform.localScale = new Vector3(TileManager.Instance.XScale_Tile, TileManager.Instance.YScale_Tile, 1);
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }


}
