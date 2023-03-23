using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCGDungeon
{
    
    [CustomEditor(typeof(PCGDungeon_MapPainter_RuledTile))]
    public class PCGDungeon_MapPainter_RuledTile_DebugEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PCGDungeon_MapPainter_RuledTile mapPainter = (PCGDungeon_MapPainter_RuledTile)target;

            if (GUILayout.Button("Random Walk Generate"))
            {
                mapPainter.RandomWalkGenerate();
            }
            if (GUILayout.Button("Corridors first Generate"))
            {
                mapPainter.CorridorsFirstGenerate();
            }
            if (GUILayout.Button("Tree Graph Generate"))
            {
                mapPainter.TreeGraphGenerate();
            }
            if (GUILayout.Button("BPS Generate"))
            {
                mapPainter.BSPAreaGenerate();
            }
            if (GUILayout.Button("Game Of Life Reset"))
            {
                mapPainter.GameOfLifeReset();
            }
            if (GUILayout.Button("Game Of Life Generate"))
            {
                mapPainter.GameOfLifeGenerate();
            }
            if (GUILayout.Button("Game Of Life Iteration"))
            {
                mapPainter.GameOfLifeIterate();
            }

        }

    }
}
