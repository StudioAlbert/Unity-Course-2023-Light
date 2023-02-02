using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviourRatio
{
    public class SB_Flee : SteeringBehaviour
    {
        [Header("Feeeling")]
        // the target
        // The steered object will follow seek, follow, evade from, hide from the target
        [SerializeField]
        private Target _ennemyToFlee;

        [SerializeField] private float _fleeSpeed;
        [SerializeField] private float _minDistance;


        protected override void UpdateSteering()
        {
            Debug.Log("Bot Fleeing");

            if (Vector3.Distance(_ennemyToFlee.transform.position, transform.position) < _minDistance)
                // Too Close, flee
                DesiredVelocity = (transform.position - _ennemyToFlee.transform.position).normalized * _fleeSpeed;
            else
                DesiredVelocity = Vector3.zero;

        }

        protected override void DrawGizmos()
        {
        }



    }

}