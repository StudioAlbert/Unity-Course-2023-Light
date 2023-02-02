using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SteeringBehaviourClassic
{
    public abstract class SteeringBehaviour : MonoBehaviour
    {
        [Header("Steering")] [SerializeField] protected float _maxForce;
        [SerializeField] protected float _maxSpeed;

        private Vector3 _desiredVelocity = Vector3.zero;
        private Vector3 _steeringForce;
        private Rigidbody _rigidbody;

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
        protected abstract void SetDesiredVelocity();

        // Draw some helper
        protected abstract void DrawGizmos();

        protected void Start()
        {
            if (!TryGetComponent<Rigidbody>(out _rigidbody))
                Debug.LogWarning("No rigid body. No Steering");
            
        }

        protected void FixedUpdate()
        {
            SetDesiredVelocity();

            _steeringForce = _desiredVelocity - _rigidbody.velocity;
            if (_steeringForce.magnitude > _maxForce)
            {
                // Debug.Log("Normalizing force : " + _steeringForce.magnitude);
                _steeringForce = _steeringForce.normalized * _maxForce;
            }

            Apply();

        }

        protected void Apply()
        {

            _rigidbody.AddForce(_steeringForce);

            if (_rigidbody.velocity.magnitude > _maxSpeed)
            {
                // Debug.Log("Maximizing Speed : " + _rigidbody.velocity);
                _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
            }

            transform.LookAt(transform.position + Velocity);
            
        }

        private void OnDrawGizmos()
        {
            if (enabled && _steeringForce.sqrMagnitude > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, _desiredVelocity);
                
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.position, Velocity);
                
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, _steeringForce);

                DrawGizmos();
            }
        }

    }

}