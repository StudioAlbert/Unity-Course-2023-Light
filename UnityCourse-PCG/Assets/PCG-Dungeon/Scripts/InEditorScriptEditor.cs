using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(InEditorScript))]
public class InEditorScriptEditor : Editor
{

    public override void OnInspectorGUI()
    {
         DrawDefaultInspector();
         InEditorScript myEditorScript = (InEditorScript) target;
         
         if(GUILayout.Button("Mon bouton"))
             myEditorScript.SpecificStuff();
         
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
