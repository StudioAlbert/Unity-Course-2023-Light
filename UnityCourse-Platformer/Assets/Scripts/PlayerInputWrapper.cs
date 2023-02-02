using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputWrapper : MonoBehaviour
{
    private float _moveX;
    public float MoveX => _moveX;
    
    private bool _jumpPressed;
    public bool JumpPressed => _jumpPressed;
    //
    // void OnJumpEvent(InputAction.CallbackContext context)
    // {
    //     
    // }
    
    void OnJump(InputValue value)
    {
        _jumpPressed = value.Get<float>() >= Mathf.Epsilon ? true : false;
    }

    void OnMove(InputValue value)
    {
        _moveX = value.Get<float>();
    }
    
}
