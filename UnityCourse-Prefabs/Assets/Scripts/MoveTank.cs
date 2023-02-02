using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTank : MonoBehaviour
{

    [SerializeField] private float _tankForwardSpeed = 5;
    [SerializeField] private float _tankRotationSpeed;
    [SerializeField] private float _velocityLimit;
    
    private Rigidbody _rb;
    private float _inputSpeed;
    private float _rotationSpeed;

    

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _inputSpeed = Input.GetAxis("Vertical");
        _rotationSpeed = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (_rb != null)
        {
            _rb.velocity = _rb.transform.forward * (_inputSpeed * _tankForwardSpeed);
            
            if(_rb.velocity.sqrMagnitude <= Mathf.Epsilon)
            {
                _rb.angularVelocity = _rb.transform.up * (_rotationSpeed * _tankRotationSpeed);
            }
            else
            {
                //Debug.Log("can't turn : " + _rb.velocity.magnitude + " : " + _velocityLimit);
            }
        }

    }
}
