using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; set; }

    [SerializeField]
    private GameObject Tile_Prefab;
    
    public Color Color1, Color2;

    public float Width, Heigth;//¹°¸®ÀûÀÎ Å©±â

    public Tile[,] TileArray { get; set; }
    public int MapX, MapY;//Ä­ °¹¼ö
    [HideInInspector]
    public float XScale_Tile, YScale_Tile;
    [HideInInspector]
    public float XDistance, YDistance;

    [SerializeField]
    private Vector3 originPos;

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
        
        GenerateTiles();
    }

    void GenerateTiles()
    {
        Transform[] tfs = GetComponentsInChildren<Transform>();

        for (int i = 1; i < tfs.Length; i++)
        {
            DestroyImmediate(tfs[i].gameObject);
        }
     
        TileArray = new Tile[MapX,MapY];

        for (int x = 0; x < MapX; x++)
        {
            for (int y = 0; y < MapY; y++)
            {
                GameObject go = Instantiate(Tile_Prefab,transform);
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

    public Vector3 GetTilePos(int x, int y)
    {
        return new Vector3(XDistance * x, YDistance * y, 0) + originPos;
    }

    public bool IsRightRange(int x, int y)
    {
        if (0 < x && x < MapX && 0 < y && y < MapY)
        {
            return true;
        }
        else return false;
    }

}
