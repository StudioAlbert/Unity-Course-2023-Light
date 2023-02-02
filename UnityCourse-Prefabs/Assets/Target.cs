using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public void DestroySelf()
    {
        Debug.Log("I'm dying ..........");
        Destroy(gameObject);
    }
    
}
