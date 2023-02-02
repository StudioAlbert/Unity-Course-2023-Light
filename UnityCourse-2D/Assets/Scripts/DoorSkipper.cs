using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorSkipper : MonoBehaviour
{
    private float timeSkip = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    IEnumerator SkipTheDoor()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        
        input.enabled = false;
        yield return new WaitForSeconds(timeSkip);
        input.enabled = true;

    }
    
}
