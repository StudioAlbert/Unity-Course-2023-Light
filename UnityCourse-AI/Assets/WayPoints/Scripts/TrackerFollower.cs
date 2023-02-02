using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackerFollower : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _wpDistance = 3f;
    [SerializeField] private float _turnSpeed = 0;
    [SerializeField] private GameObject _tracker;

    void Update()
    {
        if (Vector3.Distance(transform.position, _tracker.transform.position) > _wpDistance)
        {
            //transform.LookAt(wpManager.WayPoints[_currentWP].transform.position);
            Quaternion lookAt =
                Quaternion.LookRotation(_tracker.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, _turnSpeed * Time.deltaTime);
        
            transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed);
        
        }
        
    }
}