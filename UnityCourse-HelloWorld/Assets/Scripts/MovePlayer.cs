using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.0f;
    
    [SerializeField]
    private float _forceFactor = 5;

    [SerializeField]
    private float _rotSpeed = 0.0f;
    
     [SerializeField]
   private float _rotFactor = 5;
    
    private Rigidbody _rb;
    private Vector3 _direction;
    private float _angularDirection;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _direction = Vector3.zero;
        _angularDirection = 0.0F;
        
        if(Input.GetKey(KeyCode.UpArrow))
            // Move Forward ---------------------
            // _direction = _rb.transform.forward;
            _direction = Vector3.forward;
        
        if(Input.GetKey(KeyCode.DownArrow))
            // Move Backward ---------------------
            // _direction = _rb.transform.forward * -1;
            _direction = Vector3.back;

        if(Input.GetKey(KeyCode.LeftArrow))
            // Move Forward ---------------------
            _angularDirection = 1;
        
        if(Input.GetKey(KeyCode.RightArrow))
            // Move Backward ---------------------
            _angularDirection = -1;

    }

    private void FixedUpdate()
    {
        //transform.Translate(direction * _speed * Time.deltaTime);
        // _rb.velocity = _direction * _speed;
        _rb.angularVelocity = new Vector3(0, _angularDirection * _rotSpeed, 0);
        
        _rb.AddRelativeForce(_direction * _forceFactor);
        
        //_rb.AddTorque(new Vector3(0, _angularDirection * _rotFactor, 0));

    }
    
    public void SlowDown()
    {
        _rb.velocity *= 0.1f;
    }
}
