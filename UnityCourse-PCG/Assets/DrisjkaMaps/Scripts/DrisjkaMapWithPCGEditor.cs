using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DrisjkaMapWithPCG))]
public class DrisjkaMapWithPCGEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DrisjkaMapWithPCG disjktraPCG = (DrisjkaMapWithPCG)target;

        if (GUILayout.Button("DO IT !!!!"))
        {
            disjktraPCG.DoIt();
        }
        if (GUILayout.Button("Clear Maps"))
        {
            disjktraPCG.ClearMaps();
        }
        if (GUILayout.Button("Do The PCG Map"))
        {
            disjktraPCG.GeneratePCGMap();
        }
        if (GUILayout.Button("Do The Drisjka Map"))
        {
            disjktraPCG.GenerateDisjktraMap();
        }

        if (GUILayout.Button("Do A Star"))
        {
            disjktraPCG.DoAStar();
        }
        if (GUILayout.Button("Do A Star Iteration"))
        {
            disjktraPCG.DoAStarIteration();
        }
        
    }
}
