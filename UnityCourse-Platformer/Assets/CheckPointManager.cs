using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{

    private Transform _lastPoint;

    [SerializeField] private GameObject _player;

    private void Start()
    {
        _lastPoint = transform;
    }

    public void Respawn()
    {
        Instantiate(_player, _lastPoint);
    }
    
    public void UpdateSpawnPoint(Transform transformPosition)
    {
        _lastPoint = transformPosition;
    }
}
