using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; set; }

    public Transform Map_Tf;

    public Transform Table_Tf;
    public Vector3 OriginPos_Table;
    public int TableCount;
    public Tile[] TableArray;

    [SerializeField]
    private GameObject Tile_Prefab;
    
    public Color Color1, Color2;

    public float Width, Heigth;//¹°¸®ÀûÀÎ Å©±â
    [HideInInspector]
    public float XScale_Tile, YScale_Tile;
    [HideInInspector]
    public float XDistance, YDistance;
    [SerializeField]
    private Vector3 originPos;

    public int MapX, MapY;//Ä­ °¹¼ö
    public Tile[,] TileArray { get; set; }
    
    [SerializeField]
    private Vector2 VisibleTile_Start;
    [SerializeField]
    private Vector2 VisibleTile_End;

    public bool IsUpdatingTilePos;

    private void Awake()
    {
        Instance = this;

        Initialize_TileManager();
    }

    private void Update()
    {
        XDistance = XScale_Tile;
        YDistance = YScale_Tile;
    }

    [ContextMenu("Initialize_TileManager")]
    public void Initialize_TileManager()
    {
        XScale_Tile = Width / MapX;
        YScale_Tile = Heigth / MapY;

        XDistance = XScale_Tile;
        YDistance = YScale_Tile;
        
        GenerateMap();
        GenerateTable();

    }

    void GenerateMap()
    {
        Transform[] tfs = Map_Tf.GetComponentsInChildren<Transform>();

        for (int i = 1; i < tfs.Length; i++)
        {
            DestroyImmediate(tfs[i].gameObject);
        }
     
        TileArray = new Tile[MapX,MapY];

        for (int x = 0; x < MapX; x++)
        {
            for (int y = 0; y < MapY; y++)
            {
                GameObject go = Instantiate(Tile_Prefab,Map_Tf);
                go.name = $"Tile {x},{y}";
                go.transform.position = GetTilePos(x, y);
                go.transform.localScale = new Vector3(XScale_Tile, YScale_Tile, 1);

                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                int colorIndex = x + y;
                
                if (colorIndex % 2 == 0) sr.color = Color1;
                else sr.color = Color2;

                Tile tile = go.GetComponent<Tile>();
                tile.Initialize_Tile(x, y);
                TileArray.SetValue(tile, x, y);

                sr.enabled = false;
            }
        }

        for (int x = (int)VisibleTile_Start.x; x <= (int)VisibleTile_End.x; x++)
        {
            for (int y = (int)VisibleTile_Start.y; y <= (int)VisibleTile_End.y; y++)
            {
                TileArray[x,y].GetComponent<SpriteRenderer>().enabled = true;
                TileArray[x, y].CanPlacement = true;
            }
        }
    }

    void GenerateTable()
    {
        TableArray = new Tile[TableCount];

        Transform[] tfs = Table_Tf.GetComponentsInChildren<Transform>();

        for (int i = 1; i < tfs.Length; i++)
        {
            DestroyImmediate(tfs[i].gameObject);
        }

        for (int i = 0; i < TableCount; i++)
        {
            GameObject go = Instantiate(Tile_Prefab, Table_Tf);
            go.name = $"Table {i}";
            go.transform.position = GetTablePos(i);
            go.transform.localScale = new Vector3(XScale_Tile, YScale_Tile, 1);

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            
            if (i % 2 == 0) sr.color = Color1;
            else sr.color = Color2;

            Tile tile = go.GetComponent<Tile>();
            tile.Initialize_Table(i);
            TableArray.SetValue(tile,i);
        }
    }

    public Vector3 GetTilePos(int x, int y)
    {
        return new Vector3(XDistance * x, YDistance * y, 0) + originPos;
    }
    public Vector3 GetTablePos(int i)
    {
        return OriginPos_Table + Vector3.right * XDistance * i;
    }

    public Tile GetTableEmptySlot()
    {
        for (int i = 0; i < TableArray.Length; i++)
        {
            if(TableArray[i].CanPlacement)
                return TableArray[i];
        }

        return null;
    }


    public bool IsRightRange(int x, int y)
    {
        if (0 <= x && x < MapX && 0 <= y && y < MapY)
        {
            return true;
        }
        else return false;
    }
}
