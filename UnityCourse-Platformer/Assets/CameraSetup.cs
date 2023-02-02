using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{

    private CinemachineVirtualCamera cam;

    private GameObject _truc;
    
    // Start is called before the first frame update
    void Start()
    {
        cam.Follow = _truc.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
