using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SteeringBehaviourRatio
{
    public abstract class SteeringBehaviour : MonoBehaviour
    {
        [Header("Steering")]
        [SerializeField] protected float _maxForce;
        [SerializeField] protected float _maxSpeed;

        private float _ratio;
        
        private Rigidbody _rigidbody;

        private Vector3 _desiredVelocity = Vector3.zero;
        private Vector3 _steeringForce;

        public float Ratio { get => _ratio; set => _ratio = value; }
        public Vector3 SteeringForce { get => _steeringForce; set => _steeringForce = value; }
        public Vector3 DesiredVelocity { get => _desiredVelocity; set => _desiredVelocity = value; }
        public Vector3 Velocity
        {
            get
            {
                if (_rigidbody != null)
                    return _rigidbody.velocity;
                else
                    return Vector3.zero;
            }
        }

        
        // Method filled with
        protected abstract void UpdateSteering();

        // Draw some helper
        protected abstract void DrawGizmos();

        private void Start()
        {
            if (!TryGetComponent<Rigidbody>(out _rigidbody))
                Debug.LogWarning("No rigid body. No Steering");
        }

        private void FixedUpdate()
        {
            UpdateSteering();

            // Ratiotize the velocity
            _desiredVelocity *= _ratio;
            
            _steeringForce = _desiredVelocity - _rigidbody.velocity;
            if (_steeringForce.magnitude > _maxForce)
            {
                Debug.Log("Normalizing force : " + _steeringForce.magnitude);
                _steeringForce = _steeringForce.normalized * _maxForce;
            }

            _rigidbody.AddForce(_steeringForce);

            if (_rigidbody.velocity.magnitude > _maxSpeed)
            {
                Debug.Log("Maximizing Speed : " + _rigidbody.velocity);
                _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
            }

        }

        private void OnDrawGizmos()
        {
            if (enabled)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, _steeringForce);

                Gizmos.color = Color.blue;
                Gizmos.DrawRay(transform.position, Velocity);

                DrawGizmos();
            }
        }

    }

}