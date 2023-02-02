using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    [SerializeField] private int _nbFramesVelocityCalculation = 1;
    private Vector3 _calculatedVelocity = Vector3.zero;
    // private float _calculatedTime;
    private Vector3 _oldPosition = Vector3.zero;
    private Queue<Tuple<Vector3, float>> _velocities = new Queue<Tuple<Vector3, float>>();

    private Rigidbody _rb;

    #region Properties

    public Vector3 CalculatedVelocity => _calculatedVelocity;

    // Encapuslate Rigidbody data (Velocity)
    public float CurrentSpeed
    {
        get
        {
            if (_rb != null)
                return _rb.velocity.magnitude;
            else
                return 0f;

        }
    }
    // Encapuslate Rigidbody data (Velocity)
    public Vector3 Velocity
    {
        get
        {
            if (_rb != null)
                return _rb.velocity;
            else
                return Vector3.zero;

        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if(!TryGetComponent<Rigidbody>(out _rb))
            Debug.LogError("Rigidbody component missing !");
    }

    private void Update()
    {
        if(_oldPosition != Vector3.zero)
        {
            // Calcultion based on movement
            Vector3 instantVelocity = transform.position - _oldPosition;
            
            // Collect velocities
            _velocities.Enqueue(new Tuple<Vector3, float>(instantVelocity, Time.deltaTime));
            if (_velocities.Count > _nbFramesVelocityCalculation)
                _velocities.Dequeue();
            
            // Sum velocities
            Vector3 tempCalculatedVelocity = Vector3.zero;
            float tempCalculatedTime = 0;
            
            foreach (Tuple<Vector3, float> velocity in _velocities)
            {
                tempCalculatedVelocity += velocity.Item1;
                tempCalculatedTime += velocity.Item2;
            }
            
            // Average velocities
            if(tempCalculatedTime >= 0.001f)
                tempCalculatedVelocity /= tempCalculatedTime;
            else
                tempCalculatedVelocity = Vector3.zero;
            
            
            _calculatedVelocity = Vector3.Lerp(_calculatedVelocity, tempCalculatedVelocity, 0.75f);

        }        
        
        _oldPosition = transform.position;
        
    }

}
