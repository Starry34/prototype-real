using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float throwForce = 5.0f;
    private Rigidbody rb;

    //explosion 

    public float radius = 10.0f;
    public float power = 500.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 forceDirection = (transform.up + transform.forward) * throwForce;
        rb.AddForce(forceDirection, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("explode");
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null) rb.AddExplosionForce(power, explosionPos, radius, 3.0f);

            }
            Destroy(gameObject);
        }
    }
}
