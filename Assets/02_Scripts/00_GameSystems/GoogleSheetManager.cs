using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetManager : MonoBehaviour
{
    public static GoogleSheetManager instance;

    public GameObject Archer;
    public GameObject Buffer;
    public GameObject Debuffer;
    public GameObject Sorcerer;
    public GameObject Tanker;
    public GameObject Warrior;

    const string URL = "https://docs.google.com/spreadsheets/d/1Of--8G94QJGvuqsjvRTf7mtuzd7RifqTrVq14S6_hE4/export?format=csv";
    List<Dictionary<string, object>> data_Dialog;
    List<UnitInfo> unitInfoList = new();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(SetUnitInfo());
    }

    [ContextMenu("Do_SetUnitInfo")]
    public void Do_SetUnitInfo()
    {
        StartCoroutine(SetUnitInfo());
    }


    IEnumerator SetUnitInfo()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        print(data);

        data_Dialog = CSVReader.Read_String(data);

        Parse_AllyInfo();

        foreach (var uniInfo in unitInfoList)
        {
            ApplyAllyInfo(uniInfo);
        }
    }

    void Parse_AllyInfo()
    {
        for (int i = 0; i < data_Dialog.Count; i++)
        {
            string rating = data_Dialog[i]["등급"].ToString();
            string name = data_Dialog[i]["캐릭터 명"].ToString();
            string HP = data_Dialog[i]["체력"].ToString();
            string defense = data_Dialog[i]["방어력"].ToString();
            string damage = data_Dialog[i]["공격력"].ToString();
            string AS = data_Dialog[i]["공격속도"].ToString();
            UnitInfo unitInfo = new(rating, name, HP, defense, damage, AS);
            unitInfoList.Add(unitInfo);
            
        }
    }

    void ApplyAllyInfo(UnitInfo unitInfo)
    {
        if (unitInfo.rating == 1f)
        {
            string s = unitInfo.name;
            switch (s)
            {
                case "전사":
                    AdjustUnitInfo(Warrior, unitInfo);
                    //Debug.Log("전사");
                    break;
                case "궁수":
                    AdjustUnitInfo(Archer, unitInfo);
                    //Debug.Log("궁수");
                    break;
                case "탱커":
                    AdjustUnitInfo(Tanker, unitInfo);
                    //Debug.Log("탱커");
                    break;
                case "버퍼":
                    AdjustUnitInfo(Buffer, unitInfo);
                    //Debug.Log("버퍼");
                    break;
                case "디버퍼":
                    AdjustUnitInfo(Debuffer, unitInfo);
                    //Debug.Log("디버퍼");
                    break;
                case "마법사":
                    AdjustUnitInfo(Sorcerer, unitInfo);
                    //Debug.Log("마법사");
                    break;
            }
        }
    }

    void AdjustUnitInfo(GameObject unit_Prefab, UnitInfo unitInfo)
    {
        Unit unit = unit_Prefab.GetComponent<Unit>();

        unit.maxHP = unitInfo.HP;
        unit.hp = unitInfo.HP;
        unit.defense = unitInfo.defense;
        unit.damage = unitInfo.damage;
        unit.delayTime = unitInfo.AS;
    }


}

public struct UnitInfo
{
    public string name;
    public float rating, HP, defense, damage, AS;

    public UnitInfo(string _rating, string _name, string _HP, string _defense, string _damage, string _AS)
    {
        rating = float.Parse(_rating);
        name = _name;
        HP = float.Parse(_HP);
        defense = float.Parse(_defense);
        damage = float.Parse(_damage);
        AS = float.Parse(_AS);
    }

}