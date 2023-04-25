using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance { get; set; }

    [SerializeField]
    private GameObject Tile_Prefab;
    
    public Color Color1, Color2;

    public Tile[,] TileArray { get; set; }
    public int MapX, MapY;
    public float XScale_Tile, YScale_Tile;
    public float XDistance, YDistance;

    private void Awake()
    {
        Instance = this;
        //Initialize_TileManager();
    }

    [ContextMenu("Initialize_TileManager")]
    public void Initialize_TileManager()
    {
        XDistance = XScale_Tile;
        YDistance = YScale_Tile;

        GenerateTiles();
    }

    void GenerateTiles()
    {
        TileArray = new Tile[MapX,MapY];

        for (int x = 0; x < MapX; x++)
        {
            for (int y = 0; y < MapY; y++)
            {
                GameObject go = Instantiate(Tile_Prefab);
                go.name = $"Tile {x},{y}";
                go.transform.localPosition = GetTilePos(x, y);

                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                int colorIndex = x + y;
                
                if (colorIndex % 2 == 0) sr.color = Color1;
                else sr.color = Color2;

                Tile tile = go.GetComponent<Tile>();
                TileArray.SetValue(tile, x, y);
            }
        }
    }

    Vector3 GetTilePos(int x, int y)
    {
        return new Vector3(XDistance * x, YDistance * y, 0);
    }
}
