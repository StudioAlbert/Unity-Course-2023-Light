using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private GameObject targetPosition;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _wpDistance = 3f;
    [SerializeField] private float _turnSpeed = 0;
    
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        if(camera == null)
        {
            camera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Define a ray
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            // Draw it in the editor
            Debug.DrawRay(ray.origin, ray.direction * 1000.0f, Color.green);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.CompareTag("Ground"))
                {
                    targetPosition.transform.position = hit.point;
                } 
            }
        }

        if (Vector3.Distance(transform.position, targetPosition.transform.position) > _wpDistance)
        {
            Quaternion lookAt = Quaternion.LookRotation(targetPosition.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, _turnSpeed * Time.deltaTime);
        
            transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed);

        }



    }
}
