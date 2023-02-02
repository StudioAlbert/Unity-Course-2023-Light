using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideByScript : MonoBehaviour
{

    [SerializeField] private float _colliderRadius = 1.5f;

    // Update is called once per frame
    void Update()
    {
        var hit2Ds = Physics2D.CircleCastAll(transform.position, 1.5f, Vector2.zero);

        foreach (RaycastHit2D raycastHit2D in hit2Ds)
        {
            
            Debug.Log("Collinding who ? : " + raycastHit2D.collider.gameObject.name);
            
            if (raycastHit2D.collider.gameObject.CompareTag("Ennemy"))
            {
                Debug.Log("Ennemy colliding.............");
            }
            
        }
        

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _colliderRadius);

    }

}
