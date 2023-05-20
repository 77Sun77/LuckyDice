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
            string rating = data_Dialog[i]["���"].ToString();
            string name = data_Dialog[i]["ĳ���� ��"].ToString();
            string HP = data_Dialog[i]["ü��"].ToString();
            string defense = data_Dialog[i]["����"].ToString();
            string damage = data_Dialog[i]["���ݷ�"].ToString();
            string AS = data_Dialog[i]["���ݼӵ�"].ToString();
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
                case "����":
                    AdjustUnitInfo(Warrior, unitInfo);
                    //Debug.Log("����");
                    break;
                case "�ü�":
                    AdjustUnitInfo(Archer, unitInfo);
                    //Debug.Log("�ü�");
                    break;
                case "��Ŀ":
                    AdjustUnitInfo(Tanker, unitInfo);
                    //Debug.Log("��Ŀ");
                    break;
                case "����":
                    AdjustUnitInfo(Buffer, unitInfo);
                    //Debug.Log("����");
                    break;
                case "�����":
                    AdjustUnitInfo(Debuffer, unitInfo);
                    //Debug.Log("�����");
                    break;
                case "������":
                    AdjustUnitInfo(Sorcerer, unitInfo);
                    //Debug.Log("������");
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