using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement_ComboJump : MonoBehaviour
{

    #region Serialized

    [Header("Horizontal")] [SerializeField]
    private float _walkSpeed = 5;
    // [SerializeField] private float _runSpeed = 10;

    [Header("Vertical")] [SerializeField] private float _basicGravity = -9.81F;
    [SerializeField] private float _groundDistance = 0.05f;
    [SerializeField] private float _timeoutJumpCombo = 0.15f;
    [SerializeField] private List<SO_JumpSettings> _jumps;

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private SpriteRenderer _sprite;

    #endregion

    #region Internals

    private PlayerInputWrapper _inputs;
    private Rigidbody2D _rb;
    private Vector2 _velocity;

    private bool _isGrounded;
    private bool _isJumping = false;

    private float _jumpTimeMeasure = 0.0f;
    private float _jumpHeightMeasure = 0.0f;
    private float _previousVelocity;
    private float _appliedGravity = 0;

    private int _idxJump;
    private SO_JumpSettings _currentJump;
    private Coroutine _co_comboJumpReset = null;

    #endregion

    private void Awake()
    {
        _inputs = GetComponent<PlayerInputWrapper>();
        _rb = GetComponent<Rigidbody2D>();

        _currentJump = _jumps[0];

    }

    // Update is called once per frame
    private void Update()
    {
        _sprite.color = _currentJump.ColorJump;

        IsGrounded();

        HandleMove();
        HandleGravity();
        HandleJump();

        // _rb.velocity = _velocity;

    }

    private void FixedUpdate()
    {
        _rb.velocity = _velocity;
    }

    private void HandleMove()
    {
        if (!_isJumping)
        {
            _velocity.x = Mathf.Lerp(_velocity.x, _inputs.MoveX * _walkSpeed, 0.4f);
        }
    }

    private void HandleJump()
    {
        // float jumpGravity = 0;

        if (_inputs.JumpPressed)
            Debug.Log("Ready to jump ? States : _isJumping=" + _isJumping + " _isGrounded=" + _isGrounded);

        if (!_isJumping && _isGrounded && _inputs.JumpPressed)
        {
            _isJumping = true;
            _isGrounded = false;
            // Grounded can jump
            // Calculation of gravity
            IncreaseJumpCombo();
            _velocity.y = _currentJump.InitialVelocity;

            if (_co_comboJumpReset != null)
                StopCoroutine(_co_comboJumpReset);

            _co_comboJumpReset = StartCoroutine(CO_TimeOutJumpCombo());

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
        bool isFalling = (_velocity.y <= 0 || _inputs.JumpPressed == false);

        // Final set velocity
        if (_isJumping)
        {
            if (isFalling)
                _appliedGravity = _currentJump.JumpGravity * _currentJump.FallFactor;
            else
                _appliedGravity = _currentJump.JumpGravity;

        }
        else
        {
            _appliedGravity = _basicGravity;
        }

        // Verlet integration ------------------------------------------------
        float newVelocity = _appliedGravity * Time.deltaTime;
        float verletVelocity = _velocity.y + (newVelocity + _previousVelocity) * 0.5f;
        _previousVelocity = newVelocity;

        // Euler integration ---------------------------------------
        //float eulerVelocity = _velocity.y + _appliedGravity * Time.deltaTime;

        _velocity.y = Mathf.Max(verletVelocity, -20);

    }

    private float groundedTime = 0;
    bool _oldGrounded = false;

    private void IsGrounded()
    {

        Ray groundedRay = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(groundedRay.origin, groundedRay.direction);

        if (Physics2D.Raycast(groundedRay.origin, groundedRay.direction, _groundDistance,
                LayerMask.GetMask("Ground")))
            _isGrounded = true;
        else
            _isGrounded = false;
        
    }

    private void IncreaseJumpCombo()
    {
        _currentJump = _jumps[_idxJump];
        _idxJump = Mathf.Min(_idxJump + 1, _jumps.Count - 1);
    }

    private void ResetJumpCombo()
    {
        _idxJump = 0;
        _currentJump = _jumps[0];
    }

    private IEnumerator CO_TimeOutJumpCombo()
    {
        yield return new WaitForSeconds(_currentJump.NextJumpDelay);
        Debug.Log("Reset jump combo");
        ResetJumpCombo();
    }
}
