using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class FSM_State_Shoot : FSM_IState
    {
        private FSM_Tank _tank;
        private Animator _animator;
        
        private const float TURN_SPEED = 10f;

        public FSM_State_Shoot(FSM_Tank tank, Animator animator)
        {
            _tank = tank;
            _animator = animator;
        }

        public void OnEnter()
        {
            Debug.Log("Shoot enter");
            _tank.StartShooting();
            _tank.StartTargetting();
            _tank.StopMovement();
            _animator.enabled = false;
        }

        public void OnExit()
        {
            Debug.Log("Shoot exit");
            _tank.StopShooting();
            _tank.StopTargetting();
            _tank.StartMovement();
            _animator.enabled = true;
        }

        public void OnUpdate()
        {
            Debug.Log("Shoot tick");
            
            Quaternion lookAt = Quaternion.LookRotation(_tank.Target.transform.position - _tank.TurretTransform.position);
            _tank.TurretTransform.rotation = Quaternion.Slerp(_tank.TurretTransform.rotation, lookAt, TURN_SPEED * Time.deltaTime);

        }
        
    }
}
