using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InEditorScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Coucou");
    }

    // Update is called once per frame
    private void Update()
    {
        SpecificStuff();
    }

    public void SpecificStuff()
    {
        Debug.Log("Re-Re-Coucou");
    }
}

public class ClasseDeSalaud
{
    
}
