using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class TPS_Character_InputsWrapper : MonoBehaviour
{
    private Vector2 _look;
    private Vector2 _move;
    private bool _isRunning = false;
    private bool _isAiming;

    public Vector2 Look => _look;
    public Vector2 Move => _move;
    public bool IsRunning => _isRunning;
    public bool IsAiming => _isAiming;

    public event Action<bool> ShootEvent; 

    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
        // if(ctx.performed)
        //     _move = ctx.ReadValue<Vector2>().normalized;
        // else if(ctx.canceled)
        //     _move = Vector2.zero;
    }

    public void OnRun(InputValue value)
    {
        _isRunning = value.Get<float>() > Mathf.Epsilon ? true : false;
        
        // if (ctx.performed)
        //     _isRunning = true;
        // else if (ctx.canceled)
        //     _isRunning = false;
    }

    public void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
        
        // if(ctx.performed)
        //     _look = ctx.ReadValue<Vector2>();
        // else if(ctx.canceled)
        //     _look = Vector2.zero;
    }

    public void OnAim(InputValue value)
    {
        _isAiming = value.Get<float>() > Mathf.Epsilon ? true : false;

        // if (ctx.performed)
        //     _isAiming = true;
        // else if (ctx.canceled)
        //     _isAiming = false;

    }

    public void OnShoot(InputValue value)
    {
        // if (value.Get<float>() > Mathf.Epsilon)
        // {
        //     ShootEvent.Invoke(true);    
        // }
        // else
        // {
        //     ShootEvent.Invoke(false);
        // }
        
        if(value.isPressed)
            ShootEvent.Invoke(true);
        else
            ShootEvent.Invoke(false);
        
    }
    
}
