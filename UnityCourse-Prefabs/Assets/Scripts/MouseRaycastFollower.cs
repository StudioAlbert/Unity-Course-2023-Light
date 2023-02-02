using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycastFollower : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                transform.position = hit.point;
            }
        }
        

    }
}
