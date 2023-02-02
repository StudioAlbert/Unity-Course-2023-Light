using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace SteeringBehaviourRatio
{
    public class Bot : MonoBehaviour
    {

        private Rigidbody _rigidbody;
        
        [Header("Behaviours")]
        [SerializeField] private SB_Seek _seekCop1;
        [SerializeField] private SB_Seek _seekCop2;
        [SerializeField] private SB_Wander _idle;
        
        [Header("ratios")]
        [SerializeField][Range(0, 1)] private float _ratio_seekCop1;
        [SerializeField][Range(0, 1)] private float _ratio_seekCop2;
        [SerializeField][Range(0, 1)] private float _ratio_idle;
        
        private void Start()
        {
            if (!TryGetComponent<Rigidbody>(out _rigidbody))
                Debug.LogWarning("No rigid body. No Steering");
        }

        private void Update()
        {
            // ---------------------------------------------------------------------------
            _seekCop1.Ratio = _ratio_seekCop1;
            _seekCop2.Ratio = _ratio_seekCop2;
            _idle.Ratio = _ratio_idle;
        }
        
    }
}