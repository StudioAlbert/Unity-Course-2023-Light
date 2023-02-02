using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{

    [SerializeField] private RectTransform _reticlePosition;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _shootingPoint;
    
     
    [Tooltip("Fire rate in seconds")]
    [SerializeField] private float _shootingRate;        
    //[SerializeField] private Transform _shootingTarget;
    [SerializeField] private Transform _aimingTarget;
    //[SerializeField] private Transform _shootingLine;

    [SerializeField] private LayerMask _layermask;
    
    [SerializeField] private Event _actions;

    private TPS_Character_InputsWrapper _inputs;
    private Camera _mainCamera;
    private IEnumerator _shootCO;
    
    int idxLaunch = 0;


    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _inputs = GetComponent<TPS_Character_InputsWrapper>();
        if(_inputs != null)
            _inputs.ShootEvent += InputsOnShootEvent;

        _shootCO = null;
    }

    private void Update()
    {
        //Debug.Log("NB SHOOTING : " + idxLaunch);

        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(_reticlePosition.pivot.x, _reticlePosition.pivot.y,
            _mainCamera.farClipPlane));
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        
        if (Physics.Raycast(ray, out var _raycastInfo, _mainCamera.farClipPlane, _layermask.value))
        {
            //Debug.Log("Shooting on something");
            _aimingTarget.position = _raycastInfo.point;
        }        else
        {
            //Debug.Log("Shooting far far away");
            _aimingTarget.position = _mainCamera.ViewportToWorldPoint(new Vector3(_reticlePosition.pivot.x, _reticlePosition.pivot.y, _mainCamera.farClipPlane));
        }
    }

    private void InputsOnShootEvent(bool _isShooting)
    {
        
        //Debug.Log("Shooting event invoked : " + _isShooting);

        if (_isShooting)
        {
            if (_shootCO != null)
                StopCoroutine(_shootCO);
            
            _shootCO = Shoot();
            StartCoroutine(_shootCO);
            idxLaunch++;

        }
        else
        {
            StopCoroutine(_shootCO);
            idxLaunch--;
        }    
        
    }

    // Update is called once per frame
    private IEnumerator Shoot()
    {
            
        do
        {
            
            if(_inputs.IsAiming)
            {
                Quaternion _shootingTargetRotation = Quaternion.identity;
                _shootingTargetRotation = Quaternion.LookRotation(_aimingTarget.position - _shootingPoint.position);
                Instantiate(_projectile, _shootingPoint.position, _shootingTargetRotation);
                yield return new WaitForSeconds(_shootingRate);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
            
        } while (true);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
    }

}
