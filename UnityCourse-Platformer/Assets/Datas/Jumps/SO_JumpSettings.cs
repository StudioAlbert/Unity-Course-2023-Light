using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Plaformer/Jump settings", fileName = "new Jump")]
public class SO_JumpSettings : ScriptableObject
{
    
    [SerializeField] private float _jumpHeight = 4;
    [SerializeField] private float _jumpTime = 0.5f;
    [SerializeField] private float _nextJumpDelay = 0.5f;
    [SerializeField] private float _fallFactor = 2f;
    [SerializeField] private Color _colorJump = Color.white;

    [SerializeField] private Texture _sprite;
    
    private float _jumpGravity = 12;
    private float _initialVelocity = 13;

    public float JumpGravity
    {
        get
        {
            setValues();
            return _jumpGravity;
        }
    }

    public float InitialVelocity
    {
        get
        {
            setValues();
            return _initialVelocity;
        }
    }

    public Color ColorJump => _colorJump;
    public float FallFactor => _fallFactor;
    //public float JumpTime => _jumpTime;
    public float NextJumpDelay => _nextJumpDelay;

    private void setValues()
    {
        float timeToApex = _jumpTime / 2;
        _jumpGravity = (-2.0f * _jumpHeight) / (timeToApex * timeToApex);
        _initialVelocity = (2.0f * _jumpHeight) / timeToApex;
    }

}
