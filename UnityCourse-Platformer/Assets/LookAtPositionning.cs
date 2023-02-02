using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPositionning : MonoBehaviour
{

    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;

    [SerializeField] private PlayerInputWrapper _input;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs(_input.MoveX) > Mathf.Epsilon)
        {
            float x = Mathf.Sign(_input.MoveX) * Mathf.Lerp(_minDistance, _maxDistance, Mathf.Abs(_input.MoveX));
            x = Mathf.Lerp(transform.localPosition.x, x, 0.5f);
            
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }
        
    }
}
