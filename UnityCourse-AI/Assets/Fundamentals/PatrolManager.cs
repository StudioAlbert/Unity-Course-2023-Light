using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolManager : MonoBehaviour
{

    [SerializeField] private List<PatrolPoint> _points;
    private int idxPatrolPoint;

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetPatrolPoint()
    {
        return _points[idxPatrolPoint].transform.position;
    }

    public void SetNextPoint()
    {
        idxPatrolPoint++;
        
        if (idxPatrolPoint >= _points.Count)
            idxPatrolPoint = 0;
        
    }
}
