using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooter2D
{
    public class ShipMove : MonoBehaviour
    {
        [SerializeField]
        private float moveAmplitude = 1.0f;

        private Vector2 moveInput;
        private Rigidbody2D rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

            Vector2 velocity = moveInput * moveAmplitude;
            if (pos.x < 0 && moveInput.x < 0 || pos.x > 1 && moveInput.x > 0)
                velocity.x = 0;

            if (pos.y < 0 && moveInput.y < 0 || pos.y > 1 && moveInput.y > 0)
                velocity.y = 0;

            rb.velocity = velocity;
        }

        public void MoveInput(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
        }
    }
}