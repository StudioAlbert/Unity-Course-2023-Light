using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fundamentals : MonoBehaviour
{
    
    Vector3 _start;
    Vector3 _end;

    private float ratio = 0; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ratio += Time.deltaTime;
        transform.position = Vector3.Lerp(_start, _end, ratio);
    }
}
