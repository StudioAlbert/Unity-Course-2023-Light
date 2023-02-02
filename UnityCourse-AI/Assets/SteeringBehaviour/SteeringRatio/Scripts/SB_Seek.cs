using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviourRatio
{
    public class SB_Seek : SteeringBehaviour
    {

        [Header("Seeking")]
        // the target
        // The steered object will follow seek, follow, evade from, hide from the target
        [SerializeField]
        private Target _targetToSeek;

        [SerializeField] private float _seekSpeed;
        [SerializeField] private float _maxDistance;

        protected override void UpdateSteering()
        {
            Debug.Log("Bot seeking");

            if (Vector3.Distance(_targetToSeek.transform.position, transform.position) > _maxDistance)
                // Not close enough, seek
                DesiredVelocity = (_targetToSeek.transform.position - transform.position).normalized * _seekSpeed;
            else
                DesiredVelocity = Vector3.zero;

        }

        protected override void DrawGizmos()
        {
        }

    }

}