using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public float NearOrder(GameObject player)
    {
        Vector3 playerDirection = transform.position - player.transform.position;
        
        float dotProduct = Vector3.Dot(player.transform.forward, playerDirection.normalized);
        float sqrMagnitude = playerDirection.magnitude;

        float nearOrder = 0f;
        
        if (sqrMagnitude < Mathf.Epsilon)
            nearOrder = Mathf.Infinity;
        else
            nearOrder = (1 - dotProduct) * sqrMagnitude;

        // Debug.Log("NEAR ORDER : " + this.name + " sqMagn=" + sqrMagnitude + " dot=" + dotProduct + " result=" + nearOrder);
        
        return nearOrder;
        
    }

}
