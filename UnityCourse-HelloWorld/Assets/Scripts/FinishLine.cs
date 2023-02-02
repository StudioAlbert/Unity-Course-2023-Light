using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinishLine : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision enter with " + other.gameObject.name);

        Debug.Log("This is the player");
        MovePlayer movePlayer = other.GetComponent<MovePlayer>();
        if (movePlayer != null)
        {
            movePlayer.SlowDown();
        }
        
        Destroy(other.gameObject);
        
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Collision Stay with " + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Collision exit with " + other.gameObject.name);
    }
    
}
