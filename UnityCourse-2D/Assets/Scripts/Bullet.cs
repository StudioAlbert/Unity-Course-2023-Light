using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter2D
{
    public class Bullet : MonoBehaviour
    {

        [SerializeField]
        private float bulletSpeed;
        private Vector2 bulletMovementVelocity;

        private Rigidbody2D rb;
        private Camera camera;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            bulletMovementVelocity = new Vector2(0, bulletSpeed);
            if (camera == null)
            {
                camera = Camera.main;
            }

        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = bulletMovementVelocity;

            if (OffTheLimits(transform.position))
            {
                Destroy(gameObject);
            }
        }

        private bool OffTheLimits(Vector3 position)
        {
            Vector3 pos = camera.WorldToViewportPoint(position);

            if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}