using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;

    public TextMeshProUGUI DebugTxt;

    public int DebugNum;

    [Header("AllyGenerator")]
    public bool IsDebugMode_AllyGenerator;
    public int DebugSpawnRate;

    [Header("EnemyGenerator")]
    public bool IsDebugMode_EnemyGenerator;
    public int DebugSpawnCount;
    public int DebugSpawnLine;

    private void Awake()
    {
        instance = this;
        DebugSpawnRate = 1;
    }

    private void Update()
    {
        //디버그 창 온오프
        if (Input.GetKeyDown(KeyCode.LeftControl)) DebugTxt.gameObject.SetActive(!DebugTxt.gameObject.activeSelf);

        MakeKeyEvent(KeyCode.Alpha1);
        MakeKeyEvent(KeyCode.Alpha2);
        MakeKeyEvent(KeyCode.Alpha3);
        MakeKeyEvent(KeyCode.Alpha4);
        MakeKeyEvent(KeyCode.Alpha5);
        MakeKeyEvent(KeyCode.Alpha6);

        IsDebugMode_AllyGenerator = Input.GetKey(KeyCode.LeftShift);
        IsDebugMode_EnemyGenerator = Input.GetKey(KeyCode.LeftAlt);

        //유닛 스폰시 Rating 설정
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (IsDebugMode_EnemyGenerator) DebugSpawnLine = DebugSpawnLine >= 5 ? 1 : DebugSpawnLine + 1;
            else DebugSpawnRate = DebugSpawnRate >= 3 ? 1 : DebugSpawnRate + 1;
        }
    }


    private void LateUpdate()
    {
        DebugTxt.text =
      $"Ally Spawn Rate : {DebugSpawnRate}\n" +
      $"Debug Ally Move : {PawnPlacementManager.instance.IsDebugMode}\n" +
      $"AllyGenerator_DebugMode : {IsDebugMode_AllyGenerator}\n" +
      $"EnemyGenerator_DebugMode : {IsDebugMode_EnemyGenerator}\n" +
      $"Enemy Debug Spawn Count : {DebugSpawnCount}\n" +
      $"Enemy Debug Spawn Line : {DebugSpawnLine}\n";

    }

    void MakeKeyEvent(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            DebugNum = (int)keyCode - 49;
            if (IsDebugMode_AllyGenerator) AllyGenerator.instance.Spawn_Ally_Debug();
            else if (IsDebugMode_EnemyGenerator) EnemyGenerator.instance.Spawn_Enemy_Debug(DebugNum, DebugSpawnLine - 1);
        }
    }

    
}
