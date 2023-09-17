using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Base _base;
    public UnitSynthesis us;
    public AllyGenerator pg;

    public int[] unitNumber = new int[6]; // 0:Warrior, 1:Sorcerer, 2:Lancer, 3:Tanker, 4:Buffer, 5:Archer 
    public int[] unitUpgrade = new int[6]; // 0:Warrior, 1:Sorcerer, 2:Lancer, 3:Tanker, 4:Buffer, 5:Archer 

    public int money;

    public Action OnWaveStart;
    public Action OnWaveEnd;
    public bool IsInBattle;

    public List<Unit> SpawnedAllies = new();
    public List<Unit> SpawnedEnemies = new();
    public List<Ally> DeadAllies = new();

    public Inventory inventory;
    public Inventory dice_Inventory;

    public bool isStart;

    public float WaveEndHealPercent;
    public float RevivePercent;


    void Awake()
    {
        SetResolution.Set_Resolution(); // 초기에 게임 해상도 고정

        if (instance == null)
        {
            instance = this;
        }

        for(int i=0; i< unitNumber.Length; i++)
        {
            unitNumber[i] = i + 1;
        }

        unitNumber = ShuffleArray(unitNumber); // 유닛 눈끔 랜덤으로 돌리는 코드

        OnWaveStart += () => { isStart = true; };
        
        OnWaveEnd += () => { isStart = false; };
        OnWaveEnd += () => {
            for (int i = 0; i < 2; i++)
                dice_Inventory.Add_Inventory("AllyDIce");
        };

        OnWaveEnd += ReviveNHealAllies;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale *=2f;
            Debug.Log("Speed Up");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale /= 2f;
            Debug.Log("Speed Down");
        }

    }

    public void Set_Money(int value)
    {
        money += value;
    }

    private T[] ShuffleArray<T>(T[] array)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < array.Length; ++i)
        {
            random1 = UnityEngine.Random.Range(0, array.Length);
            random2 = UnityEngine.Random.Range(0, array.Length);

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }

        return array;
    }

    public int AllyIndex_Return(int num)
    {
        int index = 0;
        for(int i=0; i< unitNumber.Length; i++)
        {
            if(unitNumber[i] == num)
            {
                index = i;
                break;
            }
        }
        return index;
    }
    

    public void StartGame()
    {
        // GameManager속 변수 등 정보 처리
        
        OnWaveStart.Invoke();
    }


    public void ReviveNHealAllies()
    {
        foreach (var ally in SpawnedAllies)
        {
            ally.HealHP(ally.maxHP * WaveEndHealPercent / 100);
        }

        foreach (var ally in DeadAllies)
        {
            ally.pawn.AddTilePawn();
            ally.hp = ally.maxHP * RevivePercent / 100;
            ally.gameObject.SetActive(true);
            SpawnedAllies.Add(ally);
        }
    }

    public void SpeedUp()
    {
        Time.timeScale++;
        if (Time.timeScale == 4)
        {
            Time.timeScale = 1;
        }
    }
}
