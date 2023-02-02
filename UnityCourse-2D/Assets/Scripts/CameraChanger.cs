using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _camera;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("T Camera on .............. " + _camera.name);
        _camera.enabled = true;
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("T Camera off .............. " + _camera.name);
        _camera.enabled = false;
    }

}
