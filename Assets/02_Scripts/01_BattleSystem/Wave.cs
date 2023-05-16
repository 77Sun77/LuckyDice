using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public Wave(int _waveIndex)
    {
        waveIndex = _waveIndex; 
    }

    public int waveIndex;
    public List<EnemySpawnInfo> enemySpawnInfo_List = new();

    public float GetRanSpawnDelay()
    {


        return 0f;
    }
}

public struct EnemySpawnInfo
{
    public EnemySpawnInfo(EnemyKind _enemyKind, int _enemyLv, int _spawnLine ,float _minSpawnDelay, float _maxSpawnDelay)
    {
        enemyKind = _enemyKind;
        enemyLv = _enemyLv;
        spawnLine = _spawnLine;
        minSpawnDelay = _minSpawnDelay;
        maxSpawnDelay = _maxSpawnDelay;
    }

    public EnemyKind enemyKind;
    public int enemyLv;
    public float minSpawnDelay;
    public float maxSpawnDelay;

    public int spawnLine; //-1=랜덤,-2=마지막 적의 라인

    public float GetRandomDelay()
    {
        return Random.Range(minSpawnDelay, maxSpawnDelay);
    }


}