using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviourRatio
{
    public class SB_Evade : SteeringBehaviour
    {
        [Header("Evadeing")]
        // the target
        // The steered object will follow evade, follow, evade from, hide from the target
        [SerializeField]
        private Target _targetEvadeFrom;

        [SerializeField] private float _evadeSpeed;
        [SerializeField] private float _minDistance;

        private Vector3 _targetForwardPoint;

        protected override void UpdateSteering()
        {
            Debug.Log("Bot evadeing");

            _targetForwardPoint = _targetEvadeFrom.transform.position + _targetEvadeFrom.CalculatedVelocity;

            if (Vector3.Distance(_targetForwardPoint, transform.position) < _minDistance)
                // Not close enough, evade
                DesiredVelocity = (transform.position - _targetForwardPoint).normalized * _evadeSpeed;
            else
                DesiredVelocity = Vector3.zero;

        }

        protected override void DrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(_targetForwardPoint, new Vector3(0.5f, 0.5f, 0.5f));
            // Gizmos.DrawLine(_targetForwardPoint, new Vector3(0.5f, 0.5f, 0.5f));
        }

    }

}