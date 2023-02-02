using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class FighterController : MonoBehaviour
{
    [SerializeField] private float _speed = 5;

    private Rigidbody2D _rigidbody2D;
    private FighterAnimatorController _fighterAnimatorController;
    private bool _isAttacking;

    private Vector2 _moveInput;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _fighterAnimatorController = GetComponent<FighterAnimatorController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isAttacking)
        {
            _rigidbody2D.velocity = _moveInput * _speed;
        }
        else
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
        
        _fighterAnimatorController.Velocity = _rigidbody2D.velocity;
        _fighterAnimatorController.IsAttacking = _isAttacking;
        
   }
    
    public void HandleMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }
    
    public void HandleShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _isAttacking = true;
        }
        if (ctx.canceled)
        {
            _isAttacking = false;
        }
    }

    public void IsDead()
    {
        Debug.Log("Really dead");
        Destroy(gameObject);
    }

}
