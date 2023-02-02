using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 50;
    [SerializeField] private float _lifeTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TimeOut");
    }

    private void OnDestroy()
    {
        StopCoroutine("TimeOut");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
}
