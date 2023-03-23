using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DrisjkaMapTester))]
public class DrisjkaMapTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DrisjkaMapTester driskaTester = (DrisjkaMapTester)target;

        if (GUILayout.Button("Clear Maps"))
        {
            driskaTester.ClearMaps();
        }
        
        if (GUILayout.Button("Do The Drisjka Map (One Goal)"))
        {
            driskaTester.SetAndDoDrisjakMapOneGoal();
        }
        
        if (GUILayout.Button("Do The Drisjka Map (Multi Goals)"))
        {
            driskaTester.SetAndDoDrisjakMapMultiGoals();
        }
    }
}
