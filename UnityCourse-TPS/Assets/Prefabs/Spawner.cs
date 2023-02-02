using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{

    [SerializeField] private ExecutionOrder _prefab;

    public void Spawn()
    {
        Debug.Log("Spawn");
        
        Instantiate(_prefab, new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), Quaternion.identity);
    }
    
}
