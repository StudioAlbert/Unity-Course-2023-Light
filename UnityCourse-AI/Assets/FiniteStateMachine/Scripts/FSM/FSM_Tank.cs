using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace FSM
{
    public class FSM_Tank : MonoBehaviour
    { 
        [SerializeField] private WaypointsManager _waypointsManager;
        [SerializeField] private GameObject _turret;
        [SerializeField] private Animator _animator;        
        [SerializeField] private GameObject _bulletModel;
        [SerializeField] private Transform _firePoint;

        private NavMeshAgent _navMeshAgent;
        private float _startSpeed;

        private FSM_StateMachine _stateMachine;

        private FSM_State_Chase _chase;
        private FSM_State_Patrol _patrol;
        private FSM_State_Shoot _shoot;

        private GameObject myTarget;
        private FSM_TargetTank _target = null;

        private const float TARGETTING_DISTANCE = 75f;
        private const float SHOOTING_DISTANCE = 10f;
        private const float FIRE_AMPLITUDE = 10f;
        private const float HIDING_TIME = 2f;
        private const float FIRE_RATE = .35f;

        public  FSM_TargetTank Target { get => _target; set => _target = value; }
        public Transform TurretTransform => _turret.transform;
        public float Speed { get => _navMeshAgent.speed; set => _navMeshAgent.speed = value; }
        public void StartMovement() { _navMeshAgent.isStopped = false; }
        public void StopMovement() { _navMeshAgent.isStopped = true; }
        public Vector3 Destination { get => _navMeshAgent.destination; set => _navMeshAgent.SetDestination(value); }

        // Start is called before the first frame update
        void Awake()
        {
            
            if(!TryGetComponent<NavMeshAgent>(out _navMeshAgent))
                Debug.LogError("No nav mesh agent on current tank");
            
            if(!TryGetComponent<Animator>(out _animator))
                Debug.LogError("No animator");

            _chase = new FSM_State_Chase(this, _animator);
            _patrol = new FSM_State_Patrol(this, _waypointsManager, _animator);
            _shoot = new FSM_State_Shoot(this, _animator);
        
            _stateMachine = new FSM_StateMachine();
            
            _stateMachine.AddTransition(_patrol, _chase, () => Target != null);
            _stateMachine.AddTransition(_chase, _patrol, () => Target == null);
            
            _stateMachine.AddTransition(_chase, _shoot,() => Vector3.Distance(transform.position, Target.transform.position) <= SHOOTING_DISTANCE);
            _stateMachine.AddTransition(_shoot, _chase, () =>
            {
                if (Target != null)
                {
                    return Vector3.Distance(transform.position, Target.transform.position) > SHOOTING_DISTANCE;
                }
                else
                {
                    return true;
                }
            });
            
            _stateMachine.AddTransition(_shoot, _patrol, () => Target == null);
            
            _stateMachine.SetState(_patrol);
            
        }

        // Update is called once per frame
        void Update()
        {
            _stateMachine.OnUpdate();
        }
        
        
        // Old change states --------------------------------------------------------------------------------
        public void SetChase()
        {
            _stateMachine.SetState(_chase);
        }
        public void SetPatrol()
        {
            _stateMachine.SetState(_patrol);
        }
        public void SetShoot()
        {
            _stateMachine.SetState(_shoot);
        }

        public void StartTargetting()
        {
            StartCoroutine("CheckTargetting");
        }
        public void StopTargetting()
        {
            StopCoroutine("CheckTargetting");
        }
        IEnumerator CheckTargetting()
        {

            float timeSpotting = 0f;
            
            do
            {
                
                Ray targetRay = new Ray(TurretTransform.position, TurretTransform.forward);
                Debug.DrawRay(targetRay.origin, targetRay.direction * TARGETTING_DISTANCE, Color.red);

                RaycastHit targetHit;
                if (Physics.Raycast(targetRay, out targetHit, TARGETTING_DISTANCE))
                {
                    if (targetHit.collider.CompareTag("Player"))
                    {
                        myTarget = targetHit.collider.gameObject;
                    }
                    
                    // Debug.Log("Targetting something : " + targetHit.collider.gameObject.name);
                    FSM_TargetTank target;
                    if (targetHit.collider.gameObject.TryGetComponent<FSM_TargetTank>(out target))
                    {
                        Debug.Log("Targetting a tank : " + targetHit.collider.gameObject.name);
                        _target = target;
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

                yield return null;

            } while (true);
            
        }

        public void StartShooting()
        {
            StartCoroutine("DoShoot");
        }
        public void StopShooting()
        {
            StopCoroutine("DoShoot");
        }
        IEnumerator DoShoot()
        {

            do
            {
                GameObject bulletLaunched = Instantiate(_bulletModel, _firePoint.position, _firePoint.rotation);

                Rigidbody rb = bulletLaunched.GetComponent<Rigidbody>();
                rb.AddRelativeForce(Vector3.forward * FIRE_AMPLITUDE, ForceMode.Impulse);
                
                yield return new WaitForSeconds(FIRE_RATE);

            } while (true);
            
        }

        
    }
}
