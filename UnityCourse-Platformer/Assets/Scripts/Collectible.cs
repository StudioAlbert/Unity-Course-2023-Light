using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public event Action OnPickup; 
    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.CompareTag("Player");
        if (player != null)
        {
            OnPickup?.Invoke();
            gameObject.SetActive(false);
        }
            
    }
}
