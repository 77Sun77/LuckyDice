using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetManager : MonoBehaviour
{
    public static GoogleSheetManager instance;
    #region Ally
    [Header("Ally")]
    public GameObject Archer;
    public GameObject Archer2;
    public GameObject Archer3;

    public GameObject Buffer;
    public GameObject Buffer2;
    public GameObject Buffer3;

    public GameObject Lancer;
    public GameObject Lancer2;
    public GameObject Lancer3;

    public GameObject Sorcerer;
    public GameObject Sorcerer2;
    public GameObject Sorcerer3;

    public GameObject Tanker;
    public GameObject Tanker2;
    public GameObject Tanker3;

    public GameObject Warrior;
    public GameObject Warrior2;
    public GameObject Warrior3;
    #endregion

    #region Enemy
    [Header("Enemy")]
    public GameObject Blind;
    public GameObject Eat;
    public GameObject Head;
    public GameObject Oppressed;
    public GameObject Prayer;
    #endregion

    #region Item
    [Header("Item")]
    public GameObject HealPotion;
    public GameObject BombExplosion;
    public GameObject Barrier;
    public GameObject CharMove;
    #endregion

    public bool IsForYejun;

    const string AllyStatURL = "https://docs.google.com/spreadsheets/d/1Of--8G94QJGvuqsjvRTf7mtuzd7RifqTrVq14S6_hE4/export?format=csv";
    const string EnemyStatURL = "https://docs.google.com/spreadsheets/d/18LIGb_ar1AT4Qgk5Vvv52Xe6aAFpy0jgApOpOYw9Txo/export?format=csv";
    
    const string AllyStatURL_Yejun = "https://docs.google.com/spreadsheets/d/1gHHI5LSGooUxACVkW-ld9Pn-L6zu45qBVghqMW-jgVk/export?format=csv";
    const string EnemyStatURL_Yejun = "https://docs.google.com/spreadsheets/d/1mhmDgZ3wlzvaSLT2FGiKzDny4IMAr5krRHk6N6RrEK4/export?format=csv";


    List<AllyInfo> allyInfoList = new();
    List<EnemyInfo> enemyInfoList = new();
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DoSetUnitInfo();
    }

    public void DoSetUnitInfo()
    {
        StartCoroutine(SetAllyInfo());
        StartCoroutine(SetEnemyInfo());
    }

    IEnumerator SetAllyInfo()
    {
        UnityWebRequest www;
        if (IsForYejun) www = UnityWebRequest.Get(AllyStatURL_Yejun);
        else www = UnityWebRequest.Get(AllyStatURL);

        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        Debug.Log(data);

        List<Dictionary<string, object>> data_Dialog = CSVReader.Read_String(data);

        Parse_AllyInfo(data_Dialog);

        foreach (var allyInfo in allyInfoList)
        {
            ApplyAllyInfo(allyInfo);
        }
    }
    void Parse_AllyInfo(List<Dictionary<string, object>> data_Dialog)
    {
        for (int i = 0; i < data_Dialog.Count; i++)
        {
            string rating = data_Dialog[i]["���"].ToString();
            string name = data_Dialog[i]["ĳ���� ��"].ToString();
            string HP = data_Dialog[i]["ü��"].ToString();
            string defense = data_Dialog[i]["����"].ToString();
            string damage = data_Dialog[i]["���ݷ�"].ToString();
            string AS = data_Dialog[i]["���ݼӵ�"].ToString();
            AllyInfo unitInfo = new(rating, name, HP, defense, damage, AS);
            allyInfoList.Add(unitInfo);
        }
    }
    void ApplyAllyInfo(AllyInfo allyInfo)
    {
        switch(allyInfo.rating)
        {
            case 1:
                ApplyAllyInfo_Rating_1(allyInfo);
                break;
            case 2:
                ApplyAllyInfo_Rating_2(allyInfo);
                break;
            case 3:
                ApplyAllyInfo_Rating_3(allyInfo);
                break;
        }
    }
    void ApplyAllyInfo_Rating_1(AllyInfo allyInfo)
    {
        string s = allyInfo.name;

        switch (s)
        {
            case "����":
                AdjustAllyStat(Warrior, allyInfo);
                break;
            case "�ü�":
                AdjustAllyStat(Archer, allyInfo);
                break;
            case "��Ŀ":
                AdjustAllyStat(Tanker, allyInfo);
                break;
            case "����":
                AdjustAllyStat(Buffer, allyInfo);
                break;
            case "����":
                AdjustAllyStat(Lancer, allyInfo);
                break;
            case "������":
                AdjustAllyStat(Sorcerer, allyInfo);
                break;
        }
    }
    void ApplyAllyInfo_Rating_2(AllyInfo allyInfo)
    {
        string s = allyInfo.name;

        switch (s)
        {
            case "����":
                AdjustAllyStat(Warrior2, allyInfo);
                break;
            case "�ü�":
                AdjustAllyStat(Archer2, allyInfo);
                break;
            case "��Ŀ":
                AdjustAllyStat(Tanker2, allyInfo);
                break;
            case "����":
                AdjustAllyStat(Buffer2, allyInfo);
                break;
            case "����":
                AdjustAllyStat(Lancer2, allyInfo);
                break;
            case "������":
                AdjustAllyStat(Sorcerer2, allyInfo);
                break;
        }
    }
    void ApplyAllyInfo_Rating_3(AllyInfo allyInfo)
    {
        string s = allyInfo.name;

        switch (s)
        {
            case "����":
                AdjustAllyStat(Warrior3, allyInfo);
                break;
            case "�ü�":
                AdjustAllyStat(Archer3, allyInfo);
                break;
            case "��Ŀ":
                AdjustAllyStat(Tanker3, allyInfo);
                break;
            case "����":
                AdjustAllyStat(Buffer3, allyInfo);
                break;
            case "����":
                AdjustAllyStat(Lancer3, allyInfo);
                break;
            case "������":
                AdjustAllyStat(Sorcerer3, allyInfo);
                break;
        }
    }
    void AdjustAllyStat(GameObject unit_Prefab, AllyInfo allyInfo)
    {
        Unit unit = unit_Prefab.GetComponent<Unit>();

        unit.maxHP = allyInfo.HP;
        unit.hp = allyInfo.HP;
        unit.defense = allyInfo.defense;
        unit.damage = allyInfo.damage;
        unit.delayTime = allyInfo.AS;
    }

    IEnumerator SetEnemyInfo()
    {
        UnityWebRequest www;
        if (IsForYejun) www = UnityWebRequest.Get(EnemyStatURL_Yejun);
        else www = UnityWebRequest.Get(EnemyStatURL);

        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        Debug.Log(data);

        List<Dictionary<string, object>> data_Dialog = CSVReader.Read_String(data);

        Parse_EnemyInfo(data_Dialog);

        foreach (var uniInfo in enemyInfoList)
        {
            ApplyEnemyInfo(uniInfo);
        }
    }
    void Parse_EnemyInfo(List<Dictionary<string, object>> data_Dialog)
    {
        for (int i = 0; i < data_Dialog.Count; i++)
        {
            string name = data_Dialog[i]["�̸�"].ToString();
            string HP = data_Dialog[i]["ü��"].ToString();
            string defense = data_Dialog[i]["����"].ToString();
            string speed = data_Dialog[i]["�̵��ӵ�"].ToString();
            string damage = data_Dialog[i]["���ݷ�"].ToString();
            string AS = data_Dialog[i]["���ݼӵ�"].ToString();
            EnemyInfo unitInfo = new(name, HP, defense, speed , damage, AS);
            enemyInfoList.Add(unitInfo);
        }
    }
    void ApplyEnemyInfo(EnemyInfo enemyInfo)
    {
        string s = enemyInfo.name;
        switch (s)
        {
            case "���� ������":
                AdjustEnemyStat(Blind, enemyInfo);
                //debug.log("����");
                break;
            case "���� ������":
                AdjustEnemyStat(Head, enemyInfo);
                //debug.log("�ü�");
                break;
            case "�⵵�ϴ���":
                AdjustEnemyStat(Prayer, enemyInfo);
                //debug.log("��Ŀ");
                break;
            case "��й޴���":
                AdjustEnemyStat(Oppressed, enemyInfo);
                //debug.log("����");
                break;
            case "���ָ���":
                AdjustEnemyStat(Eat, enemyInfo);
                //debug.log("����");
                break;
        }
    }
    void AdjustEnemyStat(GameObject unit_Prefab, EnemyInfo enemyInfo)
    {
        Enemy enemy = unit_Prefab.GetComponent<Enemy>();

        enemy.maxHP = enemyInfo.HP;
        enemy.hp = enemyInfo.HP;
        enemy.defense = enemyInfo.defense;
        enemy.speed = enemyInfo.speed;
        enemy.damage = enemyInfo.damage;
        enemy.delayTime = enemyInfo.AS;
    }

}

public struct AllyInfo
{
    public string name;
    public float rating, HP, defense, damage, AS;

    public AllyInfo(string _rating, string _name, string _HP, string _defense, string _damage, string _AS)
    {
        rating = float.Parse(_rating);
        name = _name;
        HP = float.Parse(_HP);
        defense = float.Parse(_defense);
        damage = float.Parse(_damage);
        AS = float.Parse(_AS);
    }

}

public struct EnemyInfo
{
    public string name;
    public float HP, defense, speed, damage, AS;

    public EnemyInfo(string _name, string _HP, string _defense, string _speed , string _damage, string _AS)
    {
        name = _name;
        HP = float.Parse(_HP);
        defense = float.Parse(_defense);
        speed = float.Parse(_speed);
        damage = float.Parse(_damage);
        AS = float.Parse(_AS);
    }
}

