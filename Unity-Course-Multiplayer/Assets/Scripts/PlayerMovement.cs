using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _speedFactor = 1;
    [SerializeField] private Animator _animator;
    
    private Rigidbody2D _rb;
    private Vector2 _mvt;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _animator.SetFloat("Speed", _rb.velocity.magnitude);
    }

    void FixedUpdate()
    {
        _rb.velocity = _mvt * _speedFactor;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _mvt = ctx.ReadValue<Vector2>();
    }
}
