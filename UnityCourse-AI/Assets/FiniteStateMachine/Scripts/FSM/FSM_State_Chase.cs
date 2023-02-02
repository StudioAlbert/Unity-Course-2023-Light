using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FSM
{
    public class FSM_State_Chase : FSM_IState
    {

        private FSM_Tank _tank;

        // private readonly NavMeshAgent _navMeshAgent;
        private readonly Animator _tankAnimator;
        
        private const float TURN_SPEED = 10f;
        private const float CHASE_SPEED = 6f;

        public FSM_State_Chase(FSM_Tank tank, Animator tankAnimator)
        {
            _tank = tank;
            // _navMeshAgent = navMeshAgent;
            _tankAnimator = tankAnimator;
        }

        public void OnEnter()
        {
            Debug.Log("Chase enter");
            _tank.Speed = CHASE_SPEED;
            _tankAnimator.enabled = false;
            _tank.StartTargetting();
        }

        public void OnExit()
        {
            Debug.Log("Chase exit");
            _tankAnimator.enabled = true;
            _tank.StopTargetting();
        }

        public void OnUpdate()
        {
            Debug.Log("Chase tick");
            
            _tank.Destination = _tank.Target.transform.position;
            
            Quaternion lookAt = Quaternion.LookRotation(_tank.Target.transform.position - _tank.TurretTransform.position);
            _tank.TurretTransform.rotation = Quaternion.Slerp(_tank.TurretTransform.rotation, lookAt, TURN_SPEED * Time.deltaTime);
            
            // _tank.CheckTargetting();

        }
    }
}
