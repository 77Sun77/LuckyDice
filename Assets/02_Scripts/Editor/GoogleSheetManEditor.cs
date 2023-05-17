using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GoogleSheetManager))]
public class GoogleSheetManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Get GoogleSheetInfo"))
        {
            GoogleSheetManager googleMan = target as GoogleSheetManager;
            googleMan.Do_SetUnitInfo();
        }
    }
}
