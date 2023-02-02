using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooter2D
{
    public class BulletLauncher : MonoBehaviour
    {
        [SerializeField]
        private Bullet bullet;

        [SerializeField]
        private float shootingRateTime;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FireInput(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                StartCoroutine("Shoot");
            }

            if (ctx.canceled)
            {
                StopCoroutine("Shoot");
            }

        }

        IEnumerator Shoot()
        {
            do
            {
                Instantiate<Bullet>(bullet, transform);
                yield return new WaitForSeconds(shootingRateTime);
            } while (true);
        }
    }
}