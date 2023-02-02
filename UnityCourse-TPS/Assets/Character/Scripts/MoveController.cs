using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class MoveController : MonoBehaviour
{
    
    [Header("Movement")]
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;

    private CharacterController _controller;
    private Vector3 _lookTargetPosition = Vector3.zero;
    private TPS_Character_InputsWrapper _inputs;
    
    public Vector3 Velocity
    {
        get { return _controller.velocity; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Component initialization
        _controller = GetComponent<CharacterController>();
        _inputs = GetComponent<TPS_Character_InputsWrapper>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float trueSpeed = _inputs.Move.magnitude * (_inputs.IsRunning ? _runSpeed : _walkSpeed);

        if (!_inputs.IsAiming)
        {
            _lookTargetPosition = transform.position + CameraController.CameraToWorld(new Vector3(_inputs.Move.x, 0, _inputs.Move.y) * trueSpeed);

            // Not aiming, turn and forward move
            if (_inputs.Move.sqrMagnitude > Mathf.Epsilon)
            {
                Quaternion lookRotation = Quaternion.LookRotation(_lookTargetPosition - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
            }
            _controller.Move(transform.forward * (trueSpeed * Time.deltaTime));
        }
        else
        {
            _lookTargetPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.farClipPlane));
            _lookTargetPosition.y = transform.position.y;
           
            Quaternion lookRotation = Quaternion.LookRotation(_lookTargetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
            
            // aiming : forward and side moves
            Vector3 side = transform.right * _inputs.Move.x * trueSpeed * Time.deltaTime;
            Vector3 forward = transform.forward * _inputs.Move.y * trueSpeed * Time.deltaTime;
            
            _controller.Move(CameraController.CameraToWorld(side + forward));
            
        }

        //_velocity = _controller.velocity;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_lookTargetPosition, 0.5f);
    }
    
    
}
