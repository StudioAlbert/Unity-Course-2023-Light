using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SteeringBehaviourClassic
{
    public class SB_ObstacleAvoidance : SteeringBehaviour
    {

        [SerializeField] private List<Obstacle> _obstacles;
        [SerializeField] private float _obstacleRadius = 10f;
        [SerializeField] private float _forceAmplitude = 1f;
       
        private Obstacle _nearestObstacle;
        private Vector3 _tempForce;
        private float _perfectAlignAvoidness = 0f;

        private void Start()
        {
            Debug.Log("Obstacle avoidance Start !");
            base.Start();
        }

        protected override void SetDesiredVelocity()
        {
            // throw new System.NotImplementedException();
        }

        protected void FixedUpdate()
        {
                
            SteeringForce = Vector3.zero;
            
            if (FindNearest(_obstacles, _obstacleRadius, out _nearestObstacle))
            {
                Vector3 reversedObstacleDirection = transform.position - _nearestObstacle.transform.position;
                if (reversedObstacleDirection.magnitude < _obstacleRadius)
                {
                    reversedObstacleDirection.Normalize();
                    float dotProduct = Vector3.Dot(reversedObstacleDirection, Velocity.normalized);
                    float sign;

                    if (dotProduct != 0f)
                        sign = Mathf.Sign(dotProduct);
                    else
                        sign = Mathf.Sign(Random.Range(-1, 1));
                    
                    _tempForce = new Vector3(reversedObstacleDirection.z * -1f, 0,
                        reversedObstacleDirection.x); 
                    
                    _tempForce = _tempForce.normalized * sign * _forceAmplitude;

                    // turn the force
                    DesiredVelocity = Velocity - _tempForce;
                    SteeringForce = _tempForce;
                }
                
                Apply();
            }
            
        }

        private bool FindNearest(List<Obstacle> obstacles, float obstacleRadius, out Obstacle nearestObstacle)
        {

            bool foundNearest = false;
            float nearestSqMagnitude = Mathf.Infinity;

            nearestObstacle = new Obstacle();
            
            foreach (var obstacle in obstacles)
            {
                Vector3 obstacleDirection = obstacle.transform.position - transform.position;
                if (Vector3.Dot((obstacle.transform.position - transform.position).normalized, Velocity) >
                    0.707f)
                    // dot = 0.707 => Angle = +/- 45°
                {

                    float localSqMagnitude = obstacleDirection.sqrMagnitude;
                    if(localSqMagnitude < nearestSqMagnitude)
                    {
                        foundNearest = true;
                        nearestSqMagnitude = localSqMagnitude;
                        nearestObstacle = obstacle;
                    }
                }
                
            }
            
            return foundNearest;

        }

        protected override void DrawGizmos()
        {
            if (_nearestObstacle != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(_nearestObstacle.transform.position, _obstacleRadius);
                Gizmos.DrawRay(_nearestObstacle.transform.position, _tempForce);
                Gizmos.DrawSphere(_nearestObstacle.transform.position + _tempForce, 1f);

            }
        }
        
    }
}
