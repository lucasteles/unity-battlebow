using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private Rigidbody rb;
    public ArcherController Parent { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 120, ForceMode.Impulse);
        SoundManager.Instance.PlayShot();
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlayHitChatacter();
            Destroy(gameObject);
        }
        else
        {
            SoundManager.Instance.PlayHitOther();
            rb.isKinematic = true;
            rb.useGravity = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 3);
        }
    }
}