using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
            Destroy(gameObject);
        
        if(collision.collider.CompareTag("Player"))
            Destroy(gameObject);
    }
}
