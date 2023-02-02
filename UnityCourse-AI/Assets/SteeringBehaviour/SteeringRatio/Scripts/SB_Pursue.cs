using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviourRatio
{
    public class SB_Pursue : SteeringBehaviour
    {
        [Header("Pursueing")]
        // the target
        // The steered object will follow pursue, follow, evade from, hide from the target
        [SerializeField]
        private Target _targetToPursue;

        [SerializeField] private float _pursueSpeed;
        [SerializeField] private float _maxDistance;

        private Vector3 _targetForwardPoint;

        protected override void UpdateSteering()
        {
            Debug.Log("Bot pursueing");

            _targetForwardPoint = _targetToPursue.transform.position + _targetToPursue.CalculatedVelocity;

            if (Vector3.Distance(_targetForwardPoint, transform.position) > _maxDistance)
                // Not close enough, pursue
                DesiredVelocity = (_targetForwardPoint - transform.position).normalized * _pursueSpeed;
            else
                DesiredVelocity = Vector3.zero;

        }

        protected override void DrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(_targetForwardPoint, new Vector3(0.5f, 0.5f, 0.5f));
        }

    }

}