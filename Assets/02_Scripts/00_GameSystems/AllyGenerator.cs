using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyGenerator : MonoBehaviour
{
    public static AllyGenerator instance;

    public GameObject[] UnitPrefabs;
    public Transform UnitSpawn_Tf;

    public UnitList_Store[] Store;
    public enum UnitList_Store { 전사, 마법사, 랜서, 탱커, 힐러, 아처, 팔림 };
    public int InputNum;

   

    private void Awake()
    {
        instance = this;
        Store = new UnitList_Store[6];
        ResetStore();
       
    }

    private void Start()
    {
        GameManager.instance.OnWaveEnd += OnEndWave_Store;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ResetStore();
    }

    void ResetStore()
    {
        for (int i = 0; i < Store.Length; i++)
        {
            int randomInt = Random.Range(0, 6);
            Store.SetValue(randomInt, i);
        }
        UIManager.instance.SetStoreImg(UIManager.ImageType.ally);
    }

    public void Roll(int inputNum)
    {

        if (Store[inputNum] != UnitList_Store.팔림)
        {
            switch (Store[inputNum])
            {
                case UnitList_Store.전사:
                    GameManager.instance.inventory.Add_Inventory("Warrior", 1);
                    break;
                case UnitList_Store.마법사:
                    GameManager.instance.inventory.Add_Inventory("Sorcerer", 1);
                    break;
                case UnitList_Store.랜서:
                    GameManager.instance.inventory.Add_Inventory("Lancer", 1);
                    break;
                case UnitList_Store.탱커:
                    GameManager.instance.inventory.Add_Inventory("Tanker", 1);
                    break;
                case UnitList_Store.힐러:
                    GameManager.instance.inventory.Add_Inventory("Buffer", 1);
                    break;
                case UnitList_Store.아처:
                    GameManager.instance.inventory.Add_Inventory("Archer", 1);
                    break;
                case UnitList_Store.팔림:
                    break;
            }

            Store.SetValue(UnitList_Store.팔림, inputNum);
            UIManager.instance.SetStoreImg();
        }

        /*
        foreach (var content in GameManager.instance.dice_Inventory.inventory)
        {
            if (content.GetComponent<Inventory_Prefab>().d_Kind == Inventory_Prefab.Dice_Kind.Ally)
            {
                GameManager.instance.dice_Inventory.Delete_Inventory(content);
                break;
            }
        }

        if (GameManager.instance.dice_Inventory.inventory.Count <= 0) UIManager.instance.UnActive_StorePanel();
        */
    }

    public void Spawn_Ally_Debug()
    {
        var DebugMan = DebugManager.instance;

        switch (DebugMan.DebugNum)
        {
            case 0:
                GameManager.instance.inventory.Add_Inventory("Warrior", DebugMan.DebugSpawnRate);
                break;
            case 1:
                GameManager.instance.inventory.Add_Inventory("Sorcerer", DebugMan.DebugSpawnRate);
                break;
            case 2:
                GameManager.instance.inventory.Add_Inventory("Lancer", DebugMan.DebugSpawnRate);
                break;
            case 3:
                GameManager.instance.inventory.Add_Inventory("Tanker", DebugMan.DebugSpawnRate);
                break;
            case 4:
                GameManager.instance.inventory.Add_Inventory("Buffer", DebugMan.DebugSpawnRate);
                break;
            case 5:
                GameManager.instance.inventory.Add_Inventory("Archer", DebugMan.DebugSpawnRate);
                break;
        }
    }

    public GameObject SpawnAlly(AllyKind allyKind, int rating)
    {
        var unit = UnitPrefabs[(int)allyKind].SpawnUnit(UnitSpawn_Tf, TileManager.Instance.GetTableEmptySlot(), GameManager.instance.SpawnedAllies);
        GoogleSheetManager.instance.ApplyAllyInfo(unit.gameObject, rating);
        Debug.Log("SpawnAlly");

        return unit.gameObject;
    }
    public GameObject SpawnAlly(AllyKind allyKind, int rating, Tile tile)
    {
        Unit unit = null;
        if (allyKind != AllyKind.ITEM)
        {
            unit = UnitPrefabs[(int)allyKind].SpawnUnit(UnitSpawn_Tf, tile, GameManager.instance.SpawnedAllies);
            Debug.Log(unit);
            GoogleSheetManager.instance.ApplyAllyInfo(unit.gameObject, rating);

        }
        else
        {
            unit = GoogleSheetManager.instance.Barrier.SpawnUnit(UnitSpawn_Tf, tile, GameManager.instance.SpawnedAllies);
        }

        Debug.Log("SpawnAlly");
        return unit.gameObject;
    }
    public void SpawnItem(GameObject go, Tile tile)
    {
        go.SpawnItem(UnitSpawn_Tf, tile);

    }

    public void Roll(GameObject go, int goType = 0)
    {
        if (goType == 0)
            go.SpawnUnit(UnitSpawn_Tf, TileManager.Instance.GetTableEmptySlot(), GameManager.instance.SpawnedAllies);
        else
            go.SpawnItem(UnitSpawn_Tf, TileManager.Instance.GetTableEmptySlot());

        Debug.Log("Roll");
    }

    void OnEndWave_Store()//나중에 WaveManager의 event로 추가
    {
        ResetStore();
    }



}
