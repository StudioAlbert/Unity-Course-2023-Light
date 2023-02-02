using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace SteeringBehaviourClassic
{
    public class Bot : MonoBehaviour
    {

        private NavMeshAgent _navMeshAgent;
        private SteeringBehaviour _steeringBehaviour;

        private void Start()
        {
            // DisableSteeringBehaviors();
        }

        private void DisableSteeringBehaviors()
        {
            foreach (var steeringBehavior in GetComponents<SteeringBehaviour>())
            {
                steeringBehavior.enabled = false;
            }
        }

        private void EnableSteeringBehaviour<T>() where T : SteeringBehaviour
        {
            if (TryGetComponent<T>(out T behaviour))
            {
                _steeringBehaviour = behaviour;
                _steeringBehaviour.enabled = true;
            }
        }

        // private void Update()
        // {
        //     if (_steeringBehaviour != null)
        //         _steeringBehaviour.UpdateSteering();
        // }

        public void OnSeek(InputAction.CallbackContext ctxKey)
        {
            if (ctxKey.ReadValueAsButton())
            {
                DisableSteeringBehaviors();
                EnableSteeringBehaviour<SB_Seek>();
            }
        }

        public void OnFlee(InputAction.CallbackContext ctxKey)
        {
            if (ctxKey.ReadValueAsButton())
            {
                DisableSteeringBehaviors();
                EnableSteeringBehaviour<SB_Flee>();
            }
        }

    }

}