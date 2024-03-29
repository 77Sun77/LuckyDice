using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;

public class EnemyGenerator : MonoBehaviour
{
    public static EnemyGenerator instance;
    [Space(35f)]
    public Transform Generate_Tf;

    const string WaveInfoURL = "https://docs.google.com/spreadsheets/d/1zAXyhM2QQsNdDdPuASgWsA2PUlmcSrFLpN3DMmnkAkw/export?format=csv";
    const string WaveInfoURL_Yejun = "https://docs.google.com/spreadsheets/d/1O-LBZEwci2IgEfiPUMIP21uOMrQPIp10_xE33uk3noA/export?format=csv";

    public GameObject[] enemyPrefabs;
    public List<Wave> WaveList = new();
    public int CurWaveIndex;

    public int spawnX;
    int pastSpawnLine;

    int enemyIndex;

    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.OnWaveStart += StartGame;
        StartCoroutine(Initialize_EnemyGenerator());
    }

    IEnumerator Initialize_EnemyGenerator()
    {
        yield return GoogleSheetManager.instance;

        UnityWebRequest www;

        if (GoogleSheetManager.instance.IsForYejun) www = UnityWebRequest.Get(WaveInfoURL_Yejun);
        else www = UnityWebRequest.Get(WaveInfoURL);

        yield return www.SendWebRequest();
        string data = www.downloadHandler.text;
        List<Dictionary<string, object>> data_Dialog = CSVReader.Read_String(data);

        ParseWaveInfoTable(data_Dialog);
        yield return GameManager.instance;

        DebugWaveList();
        GameManager.instance.OnWaveStart += () => { GameManager.instance.IsInBattle = true; };
        GameManager.instance.OnWaveEnd += () => { GameManager.instance.IsInBattle = false; };
    }

    void ParseWaveInfoTable(List<Dictionary<string, object>> data_Dialog)
    {
        //if (GoogleSheetManager.instance.IsForYejun) data_Dialog = CSVReader.Read("WaveInfo_Yejun");
        //else data_Dialog = CSVReader.Read("WaveInfo");

        int waveIndex = 0;
        Wave wave = null;
        for (int i = 0; i < data_Dialog.Count; i++)
        {
            if ((int)data_Dialog[i]["Wave"] != waveIndex)
            {
                if ((int)data_Dialog[i]["Wave"] == -1) return;

                waveIndex++;
                wave = new Wave(waveIndex);
                WaveList.Add(wave);
            }
            wave.enemySpawnInfo_List.Add(GetEnemySpawnInfo(data_Dialog, i));
        }
    }

    EnemySpawnInfo GetEnemySpawnInfo(List<Dictionary<string, object>> data_Dialog, int i)
    {
        string name = (string)data_Dialog[i]["Name"];
        EnemyKind enemyKind = (EnemyKind)Enum.Parse(typeof(EnemyKind), name);

        int Lv = (int)data_Dialog[i]["Lv"];
        int Pos = (int)data_Dialog[i]["Pos"];
        float minDelay = float.Parse(data_Dialog[i]["MinDelay"].ToString());
        float maxDelay = float.Parse(data_Dialog[i]["MaxDelay"].ToString());

        EnemySpawnInfo enemySpawnInfo = new EnemySpawnInfo(enemyKind, Lv, Pos, minDelay, maxDelay);

        return enemySpawnInfo;
    }

    void DebugWaveList()
    {
        foreach (var item in WaveList)
        {
            string s = "";
            foreach (var _item in item.enemySpawnInfo_List)
            {
                s += $"EnemyKind:{_item.enemyKind}\n";
                s += $"EnemyLv:{_item.enemyLv}\n";
                s += $"SpawnLine:{_item.spawnLine}\n";
                s += $"Delay_Min:{_item.minSpawnDelay}\n";
                s += $"Delay_Max:{_item.maxSpawnDelay}\n\n";
            }
            Debug.Log(s);
        }

        GameManager.instance.OnWaveStart += () => { Debug.Log("Wave Start"); };
        GameManager.instance.OnWaveEnd += () => { Debug.Log("Wave End"); };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) GameManager.instance.OnWaveStart.Invoke();
    }

    IEnumerator EndWave_Cor()
    {
        Func<bool> IsAllEnemyDied = () => { bool b = GameManager.instance.SpawnedEnemies.Count > 0; return !b; };
        yield return new WaitUntil(IsAllEnemyDied);
        GameManager.instance.OnWaveEnd.Invoke();
    }


    IEnumerator SpawnWave(Wave wave)
    {
        enemyIndex = 0;
        //GameManager.instance.OnWaveStart.Invoke();

        foreach (var enemySpawnInfo in wave.enemySpawnInfo_List)
        {
            yield return new WaitForSeconds(enemySpawnInfo.GetRandomDelay());

            //GameObject go = Instantiate(enemyPrefabs[(int)enemySpawnInfo.enemyKind], Generate_Tf);
            //go.name = $"{enemySpawnInfo.enemyKind} {enemyIndex}";

            //enemyIndex++;
            //MoveEnemyToTile(go, enemySpawnInfo.spawnLine);

            //�� �ڵ�
            Unit unit = enemyPrefabs[(int)enemySpawnInfo.enemyKind].SpawnUnit(Generate_Tf, GetEnemySpawnLine(enemySpawnInfo.spawnLine), GameManager.instance.SpawnedEnemies, true);
            unit.gameObject.name = $"{enemySpawnInfo.enemyKind} {enemyIndex}";
            enemyIndex++;
        }

        Func<bool> IsAllEnemyDied = () => { bool b = GameManager.instance.SpawnedEnemies.Count <= 0; return b; };
        yield return new WaitUntil(IsAllEnemyDied);
        GameManager.instance.OnWaveEnd.Invoke();
    }

    public void Spawn_Enemy_Debug(int EnemyKind, int SpawnLine)
    {
        if (!GameManager.instance.IsInBattle) return;

        if (0 <= EnemyKind && EnemyKind <= 4)
        {
            if (0 <= SpawnLine && SpawnLine <= 5)
            {
                Unit unit = enemyPrefabs[EnemyKind].SpawnUnit(Generate_Tf, GetEnemySpawnLine(SpawnLine), GameManager.instance.SpawnedEnemies, true);
                unit.gameObject.name = $"{enemyPrefabs[0].name} {enemyIndex}";
                enemyIndex++;
            }
        }
    }

    Tile GetEnemySpawnLine(int spawnLineIndex)
    {
        if (spawnLineIndex == -1)
        {
            spawnLineIndex = UnityEngine.Random.Range(0, TileManager.Instance.MapY);
        }
        else if (spawnLineIndex == -2)
        {
            spawnLineIndex = pastSpawnLine;
        }

        pastSpawnLine = spawnLineIndex;

        return TileManager.Instance.TileArray[spawnX, spawnLineIndex];
    }

    public void StartGame()
    {
        if (DebugManager.instance.IsDebugMode_EnemyGenerator)
        {
            enemyIndex = 0;
            for (int i = 0; i < DebugManager.instance.DebugSpawnCount; i++)
            {
                Unit unit = enemyPrefabs[0].SpawnUnit(Generate_Tf, GetEnemySpawnLine(0), GameManager.instance.SpawnedEnemies, true);
                unit.gameObject.name = $"{enemyPrefabs[0].name} {enemyIndex}";
                enemyIndex++;
            }
            StartCoroutine(EndWave_Cor());
        }
        else
        {
            StartCoroutine(SpawnWave(WaveList[CurWaveIndex]));
            CurWaveIndex++;
        }
    }


}
