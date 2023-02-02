using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered " + other.name);
        
        if(other.CompareTag("Ground"))
            KillBullet();

        Target target = other.GetComponent<Target>();
        if (target != null)
        {
            target.DestroySelf();
        }
        
    }

    private void KillBullet(){
        Destroy(gameObject);
    }
}
