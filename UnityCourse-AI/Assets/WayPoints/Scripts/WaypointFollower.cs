using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private WaypointsManager wpManager;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _wpDistance = 3f;
    [SerializeField] private float _turnSpeed = 0;
   
    private int _currentWP = 0;
    void Update()
    {
/*        if (nbTours <= NB_TOURS_MAX)
        {
            Vector3 targetPosition = wpManager.GetCurrentDestination().transform.position;

            if (Vector3.Distance(transform.position, targetPosition) < _wpDistance)
                targetPosition = wpManager.GetNextPatrolDestination().transform.position;
                
            //transform.LookAt(targetPosition);

            Quaternion lookAt = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, _turnSpeed * Time.deltaTime);
        
            transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed);
        }
        else
        {
            turnManager.ReleaseTurn();
        }
*/        
    }

    
}
