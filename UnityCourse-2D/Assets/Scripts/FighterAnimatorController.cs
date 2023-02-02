using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FighterAnimatorController : MonoBehaviour
{
    private Animator _animator;
    private Vector2 _velocity;
    public Vector2 Velocity
    {
        set
        {
            _velocity = value;
        }
    }

    private bool _isAttacking;
    public bool IsAttacking
    {
        set
        {
            _isAttacking = value;
        }
    }
    
    private static readonly int IS_MOVING = Animator.StringToHash("IsMoving");
    private static readonly int VEL_X = Animator.StringToHash("velX");
    private static readonly int VEL_Y = Animator.StringToHash("velY");

    private static readonly int ATTACK_SIDE_X = Animator.StringToHash("attackSideX");
    private static readonly int ATTACK_SIDE_Y = Animator.StringToHash("attackSideY");
    private static readonly int IS_ATTACKING = Animator.StringToHash("IsAttacking");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAttacking)
        {
            if(_velocity.magnitude > Mathf.Epsilon)
            {
                _animator.SetBool(IS_MOVING, true);
            }
            else
            {
                _animator.SetBool(IS_MOVING, false);
            }
        
            _animator.SetFloat(VEL_X, _velocity.x);
            _animator.SetFloat(VEL_Y, _velocity.y);
        }
        else
        {
            _animator.SetFloat(ATTACK_SIDE_X, _velocity.x);
            _animator.SetFloat(ATTACK_SIDE_Y, _velocity.y);

        }
        
        _animator.SetBool(IS_ATTACKING, _isAttacking);

    }



}
