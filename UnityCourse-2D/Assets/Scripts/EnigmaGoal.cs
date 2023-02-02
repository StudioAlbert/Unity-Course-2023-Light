using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaGoal : MonoBehaviour
{

    float goodDistance = 0.1f;
    private void OnTriggerStay2D(Collider2D col)
    {

        float dis = Vector2.Distance(transform.position, col.transform.position);

        if(dis < goodDistance)
        {
            
        }
        
    }

}
