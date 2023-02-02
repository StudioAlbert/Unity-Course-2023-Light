using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class Tank_SimpleFSM : MonoBehaviour
{
    enum MachineState
    {
        PATROL,
        CHASE,
        SHOOT
    }
    private MachineState _state;

    [SerializeField] private WaypointsManager _waypointsManager;
    [SerializeField] private GameObject _turret;
    [SerializeField] private GameObject _bulletModel;
    [SerializeField] private Transform _firePoint;
    
    [Header("Fire Settings")]
    [SerializeField] private float _fireAmplitude = 30f;
    [SerializeField] private float _fireDistance = 25f;
    [SerializeField] private float _fireRate = .35f;

    private Animator _tankAnimator;
    private NavMeshAgent _navMeshAgent;
    
    private GameObject _target;
    private Vector3 _patrolDestination;
    
    private float timeSpotting = 0f;

    private const float TURN_SPEED = 200f;    
    private const float MAX_DISTANCE = 2f;
    private const float HIDING_TIME = 5f;
    private const float CHASE_DISTANCE = 75f;

    // Start is called before the first frame update
    void Start()
    {
        _state = MachineState.PATROL;
        _tankAnimator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _patrolDestination = _waypointsManager.GetCurrentDestination();

        StartCoroutine("ShootProcedure");
    }

    // Update is called once per frame
    void Update()
    {
        _state = ManageTransitions();

        ManageActions();
    }
    
    public void GoToStatePatrol()
    {
        _state = MachineState.PATROL;
    }    
    public void GoToStateChase()
    {
        _state = MachineState.CHASE;
    }    
    public void GoToStateShoot()
    {
        _state = MachineState.SHOOT;
    }
    private void ManageActions()
    {
        switch (_state)
        {
            case MachineState.CHASE:
                _tankAnimator.enabled = false;                
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_target.transform.position);
                CheckTargetting();
                LookAtTarget();
                break;
            
            case MachineState.SHOOT:
                _tankAnimator.enabled = false;
                _navMeshAgent.isStopped = true;
                CheckTargetting();
                LookAtTarget();
                break;
            
            case MachineState.PATROL:
                _tankAnimator.enabled = true;          
                _navMeshAgent.isStopped = false;
                CheckTargetting();
                CheckPatrol();
                break;
            
            default:
                break;
        }
        
    }

    private MachineState ManageTransitions()
    {

        switch (_state)
        {
            case MachineState.CHASE:
                // CHASE GO TO PATROL if there is no target left
                if (_target == null)
                    return MachineState.PATROL;
                
                // CHASE GO TO SHOOT if the target is close enough
                if(Vector3.Distance(_target.transform.position, transform.position) <= _fireDistance)
                    return MachineState.SHOOT;
                
                break;
            
            case MachineState.SHOOT:
                // SHOOT GO TO PATROL if there is the target is NOT close enough
                if(Vector3.Distance(_target.transform.position, transform.position) > _fireDistance)
                    return MachineState.CHASE;
                
                // SHOOT GO TO PATROL if there is no target left
                if (_target == null)
                    return MachineState.PATROL;
                
                break;
            
            case MachineState.PATROL:
                // PATROL GO TO CHASE if there is a target 
                if(_target != null)
                     return MachineState.CHASE;
                
                break;
            
            default:
                Debug.LogError("Illegal state, so go back patrolling");
                break;
        }

        // If no changes, return current state
        return _state;

    }

    private void LookAtTarget()
    {
        if (_target != null)
        {
            Quaternion lookAt = Quaternion.LookRotation(_target.transform.position - _turret.transform.position);
            _turret.transform.rotation = Quaternion.Slerp(_turret.transform.rotation, lookAt, TURN_SPEED * Time.deltaTime);
        }

    }
    
    public void CheckPatrol()
    {
        Debug.Log("Patrol tick");

        if (Vector3.Distance(transform.position, _patrolDestination) <= MAX_DISTANCE)
        {
            _patrolDestination = _waypointsManager.GetNextPatrolDestination();
        }
        _navMeshAgent.SetDestination(_patrolDestination);

    }

    public void CheckTargetting()
    {

            Ray targetRay = new Ray(_turret.transform.position, _turret.transform.forward);
            Debug.DrawRay(targetRay.origin, targetRay.direction * CHASE_DISTANCE, Color.red);

            RaycastHit targetHit;
            if (Physics.Raycast(targetRay, out targetHit, CHASE_DISTANCE))
            {
                // Debug.Log("Targetting something : " + targetHit.collider.gameObject.name);
                FSM_TargetTank target;
                if (targetHit.collider.CompareTag("Target"))
                {
                    Debug.Log("Targetting a tank : " + targetHit.collider.gameObject.name);
                    _target = targetHit.collider.gameObject;
                    timeSpotting = 0f;
                }
                else
                {
                    timeSpotting += Time.deltaTime;
                    if (timeSpotting >= HIDING_TIME)
                    {
                        _target = null;
                    }
                }

            }
            
    }

    IEnumerator ShootProcedure()
    {

        do
        {
            if (_state == MachineState.SHOOT)
            {
                GameObject bulletLaunched = Instantiate(_bulletModel, _firePoint.position, _firePoint.rotation);

                Rigidbody rb = bulletLaunched.GetComponent<Rigidbody>();
                rb.AddRelativeForce(Vector3.forward * _fireAmplitude, ForceMode.Impulse);
            }
                
            yield return new WaitForSeconds(_fireRate);

        } while (true);
            
    }

    
}
