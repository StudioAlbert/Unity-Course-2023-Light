using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed = 1;
    [SerializeField] private float _distance;
    [SerializeField] private float _rotationDistance = 0.5f;
    [SerializeField] private PatrolManager _patrolManager;

    private Vector3 _destination;
    private Vector3 _distanceVector;
    private Coroutine _co_rotation;
    
    // Update is called once per frame
    void Update()
    {

        _destination = _patrolManager.GetPatrolPoint();
        _distanceVector = _destination - transform.position;
        
        Quaternion targetRotation = Quaternion.LookRotation(_distanceVector);
        
        if(Quaternion.Angle(transform.rotation, targetRotation) > _rotationDistance)
        {
            transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
        else
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            if (_distanceVector.magnitude < _distance)
            {
                _patrolManager.SetNextPoint();
            }
        }

    }

}
