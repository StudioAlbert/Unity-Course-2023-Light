using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private List<SpawnPoint> _points;

    private SpawnPoint _lastSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        _points = GetComponentsInChildren<SpawnPoint>().ToList();
        foreach (SpawnPoint spawnPoint in _points)
        {
            spawnPoint._activateSpawnPoint = SetLastSpawnPoint;
        }
    }

    private void SetLastSpawnPoint(SpawnPoint obj)
    {
        _lastSpawnPoint = obj;
    }

    public Transform GetLastCheckPoint()
    {
        return _lastSpawnPoint.gameObject.transform;
    }
    
}
