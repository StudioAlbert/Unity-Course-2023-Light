using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private Transform _shootPoint;

    [SerializeField] private float _launchIntensity = 5;

    [SerializeField] private float _fireRate = 0.05f;

    
    private Rigidbody _tankRb;

    private void Start()
    {
        _tankRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine("ShootRate");
        
        if (Input.GetKeyUp(KeyCode.Space))
            StopCoroutine("ShootRate");

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
            StartCoroutine("ShootRate");
        
        if (Input.GetMouseButtonUp((int)MouseButton.Left))
            StopCoroutine("ShootRate");

    }

    private void Shoot()
    {
        GameObject bulletInstance = Instantiate(_bulletPrefab, _shootPoint.position, _shootPoint.rotation);

        if (bulletInstance.TryGetComponent(out Rigidbody rb))
        {
            rb.velocity = _tankRb.velocity;
            rb.AddRelativeForce(Vector3.forward * _launchIntensity);
        }

    }

    IEnumerator ShootRate()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(_fireRate);
        }

    }
}
