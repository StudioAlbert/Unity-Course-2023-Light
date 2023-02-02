using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _waypoints;

    private int _idxCurrentWaypoint = 0;
    
    public Vector3 GetCurrentDestination()
    {
        if(_idxCurrentWaypoint < _waypoints.Count && _waypoints.Count > 0)
            return _waypoints[_idxCurrentWaypoint].position;
        else
            return Vector3.zero;
        
    }

    public Vector3 GetNextPatrolDestination()
    {
        _idxCurrentWaypoint++;
        if(_idxCurrentWaypoint >= _waypoints.Count)
            _idxCurrentWaypoint = 0;

        return GetCurrentDestination();

    }
}
