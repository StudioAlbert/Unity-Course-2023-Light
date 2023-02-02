using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurret : MonoBehaviour
{
    [SerializeField] 
    private Transform _turretTransform;
    
    [SerializeField] 
    private Transform _turretTarget;
    
    // Update is called once per frame
    void Update()
    {
        if (_turretTransform != null && _turretTarget != null)
        {
            Vector3 target = new Vector3(_turretTarget.position.x, _turretTransform.position.y, _turretTarget.position.z);
            
            _turretTransform.LookAt(target);
            
        }
    }
}
