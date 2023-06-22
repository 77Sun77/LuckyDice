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
    public int ModifiedInput;

    public int TheNumberOfDice;

    

    private void Awake()
    {
        instance = this;
        Store = new UnitList_Store[6];
        ResetStore();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Roll();
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
        if(Input.GetKeyDown(keyCode))
        InputNum = (int)keyCode - 48;
    }

    void ResetStore()
    {
        for (int i = 0; i < Store.Length; i++)
        {
            int randomInt = Random.Range(0, 6);
            Store.SetValue(randomInt,i);
        }
    }

    void Roll()
    {
        TheNumberOfDice--;

        int randomInt = Random.Range(-1,2);
        ModifiedInput = InputNum + randomInt;

        if (ModifiedInput == -1)
        {
            ModifiedInput = 5;
        }
        else if(ModifiedInput == 6)
        {
            ModifiedInput = 0;
        }
        
        if(Store[ModifiedInput]==UnitList_Store.ÆÈ¸²)
        {
            Debug.Log("²Î");
            return;
        }

        //GameObject go = Instantiate(UnitPrefabs[(int)Store[ModifiedInput]],UnitSpawn_Tf);
        //go.GetComponent<Pawn>().MoveToTargetTile(TileManager.Instance.GetTableEmptySlot());
        var allyKind = UnitPrefabs[(int)Store[ModifiedInput]].GetComponent<Ally>().allyKind;
        SpawnAlly(allyKind, 1);
        Store.SetValue(UnitList_Store.ÆÈ¸², ModifiedInput);
    }

    public void SpawnAlly(AllyKind allyKind,int rating)
    {
        var unit = UnitPrefabs[(int)allyKind].SpawnUnit(UnitSpawn_Tf, TileManager.Instance.GetTableEmptySlot(), GameManager.instance.SpawnedAllies);
        GoogleSheetManager.instance.ApplyAllyInfo(unit.gameObject, rating);
        Debug.Log("SpawnAlly");
    }


    public void Roll(GameObject unit)
    {
       unit.SpawnUnit(UnitSpawn_Tf,TileManager.Instance.GetTableEmptySlot(), GameManager.instance.SpawnedAllies);
    }
    void OnEndWave_Store()//³ªÁß¿¡ WaveManagerÀÇ event·Î Ãß°¡
    {
        TheNumberOfDice += 2;
        ResetStore();
    }

}
