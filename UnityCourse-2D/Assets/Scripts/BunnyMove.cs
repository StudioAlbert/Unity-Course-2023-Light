using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer2D
{
    public class BunnyMove : MonoBehaviour
    {
        // Setting values
        [SerializeField]
        private float moveAmplitude = 1f;
        [SerializeField]
        private float jumpAmplitude = 50f;

        // Component(s)
        private Rigidbody2D rb;
        
        // Imputs
        private float moveInput;
        private bool doJump = false;
        private bool isGrounded;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isGrounded)
            {
                Vector2 velocity = new Vector2(moveInput * moveAmplitude, 0);
                rb.AddForce(velocity, ForceMode2D.Force);
            }

            if (doJump)
            {
                if (isGrounded)
                {
                    Vector2 jumpForce = new Vector2(0, jumpAmplitude);
                    rb.AddForce(jumpForce, ForceMode2D.Impulse);
                }
                doJump = false;
            }

        }

        public void MoveInput(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<float>();
        }
        public void JumpInput(InputAction.CallbackContext ctx)
        {
            doJump = ctx.ReadValueAsButton();
/*            if (ctx.canceled)
            {
                doJump = false;
            }*/
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<BunnyGround>())
            {
                isGrounded = true;
            }
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<BunnyGround>())
            {
                isGrounded = false;
            }
        }
    }
}