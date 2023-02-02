using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Drive : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _rotationSpeed = 100.0f;
    
    private float _inputMove;
    private float _inputTurn;
    private Rigidbody _rb;


    private void Start()
    {
        if(!TryGetComponent<Rigidbody>(out _rb))
            Debug.LogError("Rigidbody component missing !");
    }

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = _inputMove * _speed;
        float rotation = _inputTurn * _rotationSpeed;
        
        // translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        _rb.velocity = transform.forward * translation;
        transform.Rotate(0, rotation, 0);

        // Make it move 10 meters per second instead of 10 meters per frame...
        // translation *= Time.deltaTime;
        // rotation *= Time.deltaTime;
        // // Move translation along the object's z-axis
        // transform.Translate(0, 0, translation);
        // _currentSpeed = translation;
        //
        // // Rotate around our y-axis
        // transform.Rotate(0, rotation, 0);

    }

    public void HandleMove(InputAction.CallbackContext ctx)
    {
        _inputMove = ctx.ReadValue<float>();
        Debug.Log("Input event [Move] = " + _inputMove); 
    }
    public void HandleTurn(InputAction.CallbackContext ctx)
    {
        _inputTurn = ctx.ReadValue<float>();
        Debug.Log("Input event [Move] = " + _inputTurn);
    }

}
