using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyGenerator : MonoBehaviour
{
    public static AllyGenerator instance;

    public GameObject[] UnitPrefabs;
    public Transform UnitSpawn_Tf;

    public UnitList_Store[] Store;
    public enum UnitList_Store { Àü»ç, ¸¶¹ý»ç, ·£¼­, ÅÊÄ¿, Èú·¯, ¾ÆÃ³, ÆÈ¸² };
    public int InputNum;

    public bool IsDebuggingMode;
    public int DebugNum, DebugSpawnRate;

    private void Awake()
    {
        instance = this;
        Store = new UnitList_Store[6];
        ResetStore();
        DebugSpawnRate = 1;
    }

    private void Start()
    {
        GameManager.instance.OnWaveEnd += OnEndWave_Store;
    }

    private void Update()
    {
        IsDebuggingMode = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetStore();
        }

        MakeKeyEvent(KeyCode.Alpha1);
        MakeKeyEvent(KeyCode.Alpha2);
        MakeKeyEvent(KeyCode.Alpha3);
        MakeKeyEvent(KeyCode.Alpha4);
        MakeKeyEvent(KeyCode.Alpha5);
        MakeKeyEvent(KeyCode.Alpha6);

        if (Input.GetKeyDown(KeyCode.BackQuote)) DebugSpawnRate = DebugSpawnRate >= 3 ? 1 : DebugSpawnRate + 1;
    }

    void MakeKeyEvent(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (IsDebuggingMode)
            {
                DebugNum = (int)keyCode - 49;
                Roll_Debug();
                return;
            }
            InputNum = (int)keyCode - 49;
        }
    }

    void ResetStore()
    {
        for (int i = 0; i < Store.Length; i++)
        {
            int randomInt = Random.Range(0, 6);
            Store.SetValue(randomInt, i);
        }
        UIManager.instance.SetStoreImg();
    }

    public void Roll(int inputNum)
    {

        if (Store[inputNum] != UnitList_Store.ÆÈ¸²)
        {
            switch (Store[inputNum])
            {
                case UnitList_Store.Àü»ç:
                    GameManager.instance.inventory.Add_Inventory("Warrior", 1);
                    break;
                case UnitList_Store.¸¶¹ý»ç:
                    GameManager.instance.inventory.Add_Inventory("Sorcerer", 1);
                    break;
                case UnitList_Store.·£¼­:
                    GameManager.instance.inventory.Add_Inventory("Lancer", 1);
                    break;
                case UnitList_Store.ÅÊÄ¿:
                    GameManager.instance.inventory.Add_Inventory("Tanker", 1);
                    break;
                case UnitList_Store.Èú·¯:
                    GameManager.instance.inventory.Add_Inventory("Buffer", 1);
                    break;
                case UnitList_Store.¾ÆÃ³:
                    GameManager.instance.inventory.Add_Inventory("Archer", 1);
                    break;
                case UnitList_Store.ÆÈ¸²:
                    break;
            }

            Store.SetValue(UnitList_Store.ÆÈ¸², inputNum);
            UIManager.instance.SetStoreImg();
        }

        foreach (var content in GameManager.instance.dice_Inventory.inventory)
        {
            if (content.GetComponent<Inventory_Prefab>().d_Kind == Inventory_Prefab.Dice_Kind.Ally)
            {
                GameManager.instance.dice_Inventory.Delete_Inventory(content);
                break;
            }
        }

        if (GameManager.instance.dice_Inventory.inventory.Count <= 0) UIManager.instance.UnActive_StorePanel();

    }

    void Roll_Debug()
    {
        switch (DebugNum)
        {
            case 0:
                GameManager.instance.inventory.Add_Inventory("Warrior", DebugSpawnRate);
                break;
            case 1:
                GameManager.instance.inventory.Add_Inventory("Sorcerer", DebugSpawnRate);
                break;
            case 2:
                GameManager.instance.inventory.Add_Inventory("Lancer", DebugSpawnRate);
                break;
            case 3:
                GameManager.instance.inventory.Add_Inventory("Tanker", DebugSpawnRate);
                break;
            case 4:
                GameManager.instance.inventory.Add_Inventory("Buffer", DebugSpawnRate);
                break;
            case 5:
                GameManager.instance.inventory.Add_Inventory("Archer", DebugSpawnRate);
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

    void OnEndWave_Store()//³ªÁß¿¡ WaveManagerÀÇ event·Î Ãß°¡
    {
        ResetStore();
    }



}
