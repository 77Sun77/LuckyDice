using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; set; }

    [SerializeField]
    private GameObject Tile_Prefab;
    
    public Color Color1, Color2;

    public float Width, Heigth;

    public Tile[,] TileArray { get; set; }
    public int MapX, MapY;
    [HideInInspector]
    public float XScale_Tile, YScale_Tile;
    [HideInInspector]
    public float XDistance, YDistance;
    [SerializeField]
    private Vector3 originPos;

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
        if (transform.childCount > 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
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
            }
        }
    }

    public Vector3 GetTilePos(int x, int y)
    {
        return new Vector3(XDistance * x, YDistance * y, 0) + originPos;
    }
}
