using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // [SerializeField] private SpawnManager _manager;

    public Action<SpawnPoint> _activateSpawnPoint;
    
    [SerializeField] private GameObject activeObject;
    [SerializeField] private GameObject inactiveObject;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        activeObject.SetActive(false);
        inactiveObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        Debug.Log("Spanw point collide who ?" + col.gameObject.name);
        
        if (col.CompareTag("Player"))
        {
            _activateSpawnPoint(this);
            
            inactiveObject.SetActive(false);
            activeObject.SetActive(true);
            
        }
        
    }

}
