using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionOrder : MonoBehaviour
{

    public string _orderName;
    
    private void Awake()
    {
        Debug.Log("Awake : " + name + " _ " + _orderName);
    }
    void OnEnable()
    {
        Debug.Log("On Enable : " + name + " _ " + _orderName);
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start : " + name + " _ " + _orderName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
