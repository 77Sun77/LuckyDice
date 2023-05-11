using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class EnemyGenerator : MonoBehaviour
{
    public Transform Generate_Tf;

    List<Dictionary<string, object>> data_Dialog;
    public GameObject[] enemyPrefabs;
    public List<Wave> WaveList = new();
    public int CurWaveIndex;
    
    public int spawnX;
    int pastSpawnLine;

    
    
    public void Awake()
    {
        ParseEnemyTable();
        DebugWaveList(); 
    }

    void ParseEnemyTable()
    {
        data_Dialog = CSVReader.Read("WaveInfo");

        int waveIndex = 0;
        Wave wave = null;
        for (int i = 0; i < data_Dialog.Count; i++)
        {
            if ((int)data_Dialog[i]["Wave"] != waveIndex)
            {
                waveIndex++;
                wave = new Wave(waveIndex);
                WaveList.Add(wave);
            }
            wave.enemySpawnInfo_List.Add(GetEnemySpawnInfo(data_Dialog, i)); 
        }
    }

    EnemySpawnInfo GetEnemySpawnInfo(List<Dictionary<string,object>> data_Dialog,int i)
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
    }

    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.Return))
       {
            StartCoroutine(SpawnWave(WaveList[CurWaveIndex]));
            CurWaveIndex++;
       }

    }

    IEnumerator SpawnWave(Wave wave)
    {
        foreach (var enemySpawnInfo in wave.enemySpawnInfo_List)
        {
            yield return new WaitForSeconds(enemySpawnInfo.GetRandomDelay());

            GameObject go = Instantiate(enemyPrefabs[(int)enemySpawnInfo.enemyKind], Generate_Tf);
            
            MoveEnemyToTile(go, enemySpawnInfo.spawnLine);
        }
    }

    void MoveEnemyToTile(GameObject go,int spawnLineIndex)
    {
        if (spawnLineIndex == -1)
        {
            spawnLineIndex = UnityEngine.Random.Range(0, TileManager.Instance.MapY);
        }
        else if (spawnLineIndex == -2)
        {
            spawnLineIndex = pastSpawnLine;
        }

        go.transform.position = TileManager.Instance.TileArray[spawnX, spawnLineIndex].GetPos();
        Debug.Log(spawnLineIndex);
        pastSpawnLine = spawnLineIndex;
    }

}
