using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public TextMeshProUGUI DebugTxt;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) DebugTxt.gameObject.SetActive(!DebugTxt.gameObject.activeSelf);

        DebugTxt.text =
        $"Ally Spawn Rate : {AllyGenerator.instance.DebugSpawnRate}\n" +
        $"Debug Ally Move : {PawnPlacementManager.instance.IsDebugMode}\n" +
        $"Debug Enemy Spawn : {EnemyGenerator.instance.IsDebuggingMode}\n";
    }

}
