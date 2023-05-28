using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int X { get; set; }
    public int Y { get; set; }

    public Unit Ally;
    public List<Unit> EnemyList = new();
    
    public bool IsTable;
    public bool CanPlacement;

    public Color originColor;
    /// <summary>
    /// Tile의 X,Y값 할당
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Initialize_Tile(int x, int y,Color _originColor)
    {
        X = x;
        Y = y;
        CanPlacement = false;
        originColor = _originColor;
    }

    public void Initialize_Table(int i)
    {
        CanPlacement = true;
        IsTable = true;
        X=i;
    }

    private void Update()
    {
        CanPlacement = EnemyList.Count == 0 && Ally == null;

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

    public void Do_AOE_Effect(Color effectColor)
    {
        StartCoroutine(DO_AOE_Effect_Cor(this, effectColor));
    }

    IEnumerator DO_AOE_Effect_Cor(Tile tile, Color effectColor)
    {
        SpriteRenderer tileSR = tile.GetComponent<SpriteRenderer>();
        tileSR.color = effectColor;
        yield return new WaitForSeconds(0.3f);
        tileSR.color = tile.originColor;
    }
}
