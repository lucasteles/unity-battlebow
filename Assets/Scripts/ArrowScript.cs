using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 120, ForceMode.Impulse);
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            Destroy(gameObject, 3);
        }
        
    }
}