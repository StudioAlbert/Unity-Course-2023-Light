using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement_Workshop : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float _speedX = 12;
    [SerializeField] private float _baseGravity = -9.81f;
    
    [Header("Detectors")]
    [SerializeField] private float _groundedDepth = 1f;
    [SerializeField] private float _walledDepth = 1f;
    
    [Header("Jumps")]
    [SerializeField] private List<SO_JumpSettings> _jumps;

    // [SerializeField] private float _simpleJumpTime = 0.5f;
    // [SerializeField] private float _simpleJumpHeight = 2;
    // [SerializeField] private float _fallFactor = 2.0f;
    //
    [Header("Counter")]
    [SerializeField] private TextMeshProUGUI _timerText;

    private PlayerInputWrapper _inputs;
    private Rigidbody2D _rb;
    private Vector2 _velocity;
    private bool _isGrounded = false;
    private bool _isWalledLeft = false;
    private bool _isWalledRight = false;
    private float _jumpTimeMeasure;
    private float _jumpHeightMeasure;

    // private float _initialVelocity = 10;
    private float _previousVelocity;
    // private float _jumpGravity;
    private bool _isJumping = false;

    private int _idxJump;
    private SO_JumpSettings _currentJump;
    private Coroutine _co_comboJumpReset = null;
    
    // Start is called before the first frame update
    void Start()
    {
        _inputs = GetComponent<PlayerInputWrapper>();
        _rb = GetComponent<Rigidbody2D>();

        _currentJump = _jumps[0];
        
    }

    // private void CalculateJumpFactor()
    // {
    //
    //     float timeToApex = _simpleJumpTime / 2f;
    //     _jumpGravity = (-2.0f * _simpleJumpHeight) / (timeToApex * timeToApex);
    //     _initialVelocity = (2.0f * _simpleJumpHeight) / timeToApex;
    // }

    // Update is called once per frame
    void Update()
    {
        IsGrounded();
        IsWalled();
        
        // CalculateJumpFactor();

        HandleMoveHorizontal();
        HandleGravity();
        HandleJump();

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y - 0.5f * _groundedDepth,
                transform.position.z),
            new Vector3(1, _groundedDepth, 1));

        Gizmos.color = new Color(255, 100, 0);
        Gizmos.DrawWireCube(new Vector3(transform.position.x + 0.5f * (1 + _walledDepth), transform.position.y + 0.5f,
                transform.position.z),
            new Vector3(_walledDepth, 1, 1));

        Gizmos.DrawWireCube(new Vector3(transform.position.x - 0.5f * (1 + _walledDepth), transform.position.y + 0.5f,
                transform.position.z),
            new Vector3(_walledDepth, 1, 1));

    }

    private void HandleJump()
    {

        if (_isJumping == false && _isGrounded && _inputs.JumpPressed)
        {
            Debug.Log("Jump !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            _isJumping = true;
            _isGrounded = false;

            IncreaseJumpCombo();
            
            _velocity.y = _currentJump.InitialVelocity;
            _previousVelocity = 0;
            
            _jumpTimeMeasure = 0;
            _jumpHeightMeasure = 0;

        }
        else if (_isJumping && _isGrounded && !_inputs.JumpPressed)
        {
            _isJumping = false;
        }

        // ------------------------------------------------
        if (!_isGrounded)
        {
            _jumpTimeMeasure += Time.deltaTime;
            _jumpHeightMeasure = Mathf.Max(transform.position.y, _jumpHeightMeasure);
        }
        _timerText.text = "time=" + _jumpTimeMeasure.ToString() + " height=" + _jumpHeightMeasure.ToString();

    }

    private void HandleGravity()
    {

        float appliedGravity;
        bool isFalling = (_velocity.y <= 0 || !_inputs.JumpPressed);

        if (_isJumping)
            if (isFalling)
                appliedGravity = _currentJump.JumpGravity * _currentJump.FallFactor;
            else
                appliedGravity = _currentJump.JumpGravity;
        else
            appliedGravity = _baseGravity;

        // Verlet integration ------------------------------------------------
        // float _previousVelocity = _velocity.y;
        float newVelocity = appliedGravity * Time.deltaTime;
        float verletVelocity = _velocity.y + (newVelocity + _previousVelocity) * 0.5f;
        _velocity.y = verletVelocity;
        _previousVelocity = newVelocity;
        
    }

    private void HandleMoveHorizontal()
    {
        _velocity.x = _inputs.MoveX * _speedX;
    }

    private void IsGrounded()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, _groundedDepth, LayerMask.GetMask("Ground")))
            _isGrounded = true;
        else
            _isGrounded = false;

    }

    private void IsWalled()
    {
        if (Physics2D.Raycast(transform.position, Vector2.left, 0.5f * _walledDepth, LayerMask.GetMask("Ground")))
            _isWalledLeft = true;
        else
            _isWalledLeft = false;

        if (Physics2D.Raycast(transform.position, Vector2.right, 0.5f * _walledDepth, LayerMask.GetMask("Ground")))
            _isWalledRight = true;
        else
            _isWalledRight = false;

    }

    private void FixedUpdate()
    {
        _rb.velocity = _velocity;
    }

    #region Combo Jump
    private void IncreaseJumpCombo()
    {
        _currentJump = _jumps[_idxJump];
        _idxJump = Mathf.Min(_idxJump + 1, _jumps.Count - 1);
        
        if (_co_comboJumpReset != null)
            StopCoroutine(_co_comboJumpReset);

        _co_comboJumpReset = StartCoroutine(CO_TimeOutJumpCombo());
        
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
    #endregion
}
