using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    [Header("Inputs")]
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Transform _aimingTarget;

    // cinemachine
    [Header("Settings")]
    public float _cinemachineTargetYaw;
    public float _cinemachineTargetPitch;
    
    [SerializeField] private float _sensitivity = Mathf.Epsilon;
    [SerializeField] private float _cameraRotateSpeed = 300;
    [SerializeField] private float _cameraAngleOverride = 50f;
    [SerializeField] private float _bottomClamp = -30f;
    [SerializeField] private float _topClamp = 70f;
    [SerializeField] private CinemachineVirtualCamera _aimingCamera;

    private PlayerInput _playerInput;
    private static Camera _mainCamera;
    private TPS_Character_InputsWrapper _inputs;    

    private bool IsCurrentDeviceMouse
    {
        get
        {
            return _playerInput.currentControlScheme == "KeyboardMouse";
        }
    }

    public bool LockCameraPosition
    {
        get{ return false; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _mainCamera = Camera.main;
        _inputs = GetComponent<TPS_Character_InputsWrapper>();
    }

    // Update is called once per frame
    void LateUpdate()
    {

        _aimingCamera.enabled = _inputs.IsAiming;

        // if there is an input and camera position is not fixed
        if (_inputs.Look.sqrMagnitude >= _sensitivity && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
        
            _cinemachineTargetYaw += _inputs.Look.x * _cameraRotateSpeed * deltaTimeMultiplier;
            _cinemachineTargetPitch += _inputs.Look.y * _cameraRotateSpeed * deltaTimeMultiplier;
        }
        
        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
        
        // Cinemachine will follow this target
        _cameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + _cameraAngleOverride, _cinemachineTargetYaw, 0.0f);

    }
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


    public static Vector3 CameraToWorld(Vector3 input)
    {
        Transform cameraTransform = _mainCamera.transform;
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        
        forward.y = input.y;
        right.y = input.y;
        
        forward.Normalize();
        right.Normalize();
        
        return forward * input.z + right * input.x;
        
    }


    


}
