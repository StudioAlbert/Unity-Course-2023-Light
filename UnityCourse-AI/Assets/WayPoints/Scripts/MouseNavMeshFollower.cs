using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseNavMeshFollower : MonoBehaviour
{
    [SerializeField] private GameObject targetPosition;
    // [SerializeField] private float _moveSpeed;
    // [SerializeField] private float _wpDistance = 3f;
    // [SerializeField] private float _turnSpeed = 0;

    private NavMeshAgent _navMeshAgent;
    
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        if(camera == null)
        {
            camera = Camera.main;
        }
        
        if(TryGetComponent<NavMeshAgent>(out _navMeshAgent) == false)
            Debug.LogError("No navmesh agent...");
        
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
                    _navMeshAgent.SetDestination(hit.point);
                } 
            }
        }

    }
}
