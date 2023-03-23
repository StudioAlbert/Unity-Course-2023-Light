using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace PCG
{
    [CustomEditor(typeof(Graph))]

    public class DebugGraphEditor : Editor
    {

        private int pointAIndex = 0;
        private int pointBIndex = 0;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Graph myGraph = (Graph)target;

            string[] nodeNames = new string[myGraph.AvailableNodes.Count];

            int i = 0;
            foreach (var node in myGraph.AvailableNodes)
            {
                nodeNames[i++] = node.name;
            }
            pointAIndex = EditorGUILayout.Popup("Point A", pointAIndex, nodeNames);
            pointBIndex = EditorGUILayout.Popup("Point B", pointBIndex, nodeNames);

            if (GUILayout.Button("Clear links"))
            {
                //Debug.Log("Hello world !!!!");
                myGraph.ClearLinks();
            }

            if (GUILayout.Button("Add link"))
            {
                //Debug.Log("Hello world !!!!");
                myGraph.AddLink(myGraph.AvailableNodes[pointAIndex], myGraph.AvailableNodes[pointBIndex]);
            }
        }

    }

}