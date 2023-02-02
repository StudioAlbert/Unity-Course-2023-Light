using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement_SimpleJump : MonoBehaviour
{

    private PlayerInputWrapper _inputs;
    private Rigidbody2D _rb;
    private Vector2 _velocity;
    private bool _isGrounded;

    [Header("Horizontal")] [SerializeField]
    private float _walkSpeed = 5;

    // [SerializeField] private float _runSpeed = 10;

    [Header("Vertical")] [SerializeField] private float _basicGravity = -9.81F;
    [SerializeField] private float _groundDistance = 0.05f;
    [SerializeField] private float _simpleJumpHeight = 4;
    [SerializeField] private float _simpleJumpTime = 0.5f;

    private float _jumpTimeMeasure = 0.0f;
    private float _jumpHeightMeasure = 0.0f;

    private float _timeToApex;
    private float _jumpGravity;
    private float _initialVelocity;
    private float _previousVelocity;

    private float _appliedGravity = 0;

    [SerializeField] private TextMeshProUGUI _timerText;
    private bool _isJumping = false;
    private int _idxJump;
    private SO_JumpSettings _currentJump;

    private void Awake()
    {
        _inputs = GetComponent<PlayerInputWrapper>();
        _rb = GetComponent<Rigidbody2D>();

        float timeToApex = _simpleJumpTime / 2;
        _jumpGravity = (-2.0f * _simpleJumpHeight) / (timeToApex * timeToApex);
        _initialVelocity = (2.0f * _simpleJumpHeight) / timeToApex;

    }

    // Update is called once per frame
    private void Update()
    {
        HandleMove();
    }

    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();

        HandleGravity();
        HandleJump();

        _rb.velocity = _velocity;

    }

    private void HandleMove()
    {
        if(!_isJumping)
        {
            _velocity.x = Mathf.Lerp(_velocity.x, _inputs.MoveX * _walkSpeed, 0.4f);
        }

    }

    private void HandleJump()
    {

        if (!_isJumping && _isGrounded && _inputs.JumpPressed)
        {
            _isJumping = true;
            // Grounded can jump
            // Calculation of gravity
            _velocity.y = _initialVelocity;
            
            _jumpTimeMeasure = 0;
            _jumpHeightMeasure = 0;
        }
        else if (_isJumping && _isGrounded && !_inputs.JumpPressed)
        {
            _isJumping = false;
        }

        if (!_isGrounded)
        {
            _jumpTimeMeasure += Time.deltaTime;
            _jumpHeightMeasure = Mathf.Max(transform.position.y, _jumpHeightMeasure);
        }

        _timerText.text = "time=" + _jumpTimeMeasure.ToString() + " height=" + _jumpHeightMeasure.ToString();
        
    }

    private void HandleGravity()
    {

        if (_isGrounded)
        {
            _velocity.y = 0;
        }
        else
        {
            // Final set velocity
            if (_isJumping)
                _appliedGravity = _jumpGravity;
            else
                _appliedGravity = _basicGravity;

            // Verlet integration ------------------------------------------------
            // float previousVelocity = _velocity.y;
            float newVelocity = _appliedGravity * Time.deltaTime;
            float verletVelocity = _velocity.y + (newVelocity + _previousVelocity) * 0.5f;
            _previousVelocity = newVelocity;

            // Euler integration ---------------------------------------
            float eulerVelocity = _velocity.y + _appliedGravity * Time.deltaTime;

            _velocity.y = verletVelocity;

        }


    }

    private bool IsGrounded()
    {
        Ray groundedRay = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(groundedRay.origin, groundedRay.direction);

        if (Physics2D.Raycast(groundedRay.origin, groundedRay.direction, 0.5f + _groundDistance,
                LayerMask.GetMask("Ground")))
            return true;
        else
            return false;
    }

}
