using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyGenerator : MonoBehaviour
{
    public static AllyGenerator instance;

    public GameObject[] UnitPrefabs;
    public Transform UnitSpawn_Tf;

    public UnitList_Store[] Store;
    public enum UnitList_Store {Àü»ç,¸¶¹ý»ç,·£¼­,ÅÊÄ¿,Èú·¯,¾ÆÃ³,ÆÈ¸²};
    public int InputNum;

    public bool IsDebuggingMode;
    public int DebugNum;

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
        IsDebuggingMode = Input.GetKey(KeyCode.LeftShift);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Roll();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetStore();
        }

        MakeKeyEvent(KeyCode.Alpha0);
        MakeKeyEvent(KeyCode.Alpha1);
        MakeKeyEvent(KeyCode.Alpha2);
        MakeKeyEvent(KeyCode.Alpha3);
        MakeKeyEvent(KeyCode.Alpha4);
        MakeKeyEvent(KeyCode.Alpha5);
    }

    void MakeKeyEvent(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            if (IsDebuggingMode)
            {
                DebugNum = (int)keyCode - 48;
                Roll_Debug();
                return;
            }
            InputNum = (int)keyCode - 48;
        }
    }

    void ResetStore()
    {
        for (int i = 0; i < Store.Length; i++)
        {
            int randomInt = Random.Range(0, 6);
            Store.SetValue(randomInt,i);
        }
        UIManager.instance.SetStoreImg();
    }

    public void Roll(int inputNum)
    {
        //inputNum--;
        DiceManager.instance.TheNumberOfDice--;
  
        if(Store[inputNum] ==UnitList_Store.ÆÈ¸²)
        {
            Debug.Log("²Î");
            return;
        }

        switch (Store[inputNum])
        {
            case UnitList_Store.Àü»ç:
                GameManager.instance.inventory.Add_Inventory("Warrior",1);
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
        if (DiceManager.instance.TheNumberOfDice == 0)
        {
            UIManager.instance.UnActive_StorePanel();
            UIManager.instance.ResetUI();
        }

    }

    void Roll_Debug()
    {
        switch (DebugNum)
        {
            case 0:
                GameManager.instance.inventory.Add_Inventory("Warrior", 1);
                break;
            case 1:
                GameManager.instance.inventory.Add_Inventory("Sorcerer", 1);
                break;
            case 2:
                GameManager.instance.inventory.Add_Inventory("Lancer", 1);
                break;
            case 3:
                GameManager.instance.inventory.Add_Inventory("Tanker", 1);
                break;
            case 4:
                GameManager.instance.inventory.Add_Inventory("Buffer", 1);
                break;
            case 5:
                GameManager.instance.inventory.Add_Inventory("Archer", 1);
                break;
        }
    }

    public GameObject SpawnAlly(AllyKind allyKind,int rating)
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
        if(goType == 0)
            go.SpawnUnit(UnitSpawn_Tf,TileManager.Instance.GetTableEmptySlot(), GameManager.instance.SpawnedAllies);
        else
            go.SpawnItem(UnitSpawn_Tf, TileManager.Instance.GetTableEmptySlot());

        Debug.Log("Roll");
    }

    void OnEndWave_Store()//³ªÁß¿¡ WaveManagerÀÇ event·Î Ãß°¡
    {
        DiceManager.instance.TheNumberOfDice += 2;
        ResetStore();
    }



}
