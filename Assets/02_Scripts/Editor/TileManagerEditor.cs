using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileManager))]
public class TileManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("GenerateTiles"))
        {
            TileManager tileMan = target as TileManager;
            tileMan.Initialize_TileManager();
        }
    }
}
