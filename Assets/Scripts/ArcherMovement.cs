using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ArcherMovement : MonoBehaviour
{
    Rigidbody _rigidbody;
    Vector3 _movement;
    bool _isGrounded = true;

    [SerializeField]
    float jumpForce = 10;

    [SerializeField]
    float multVelocity = 3;

    private bool _stop;


    public int HorizontalRaw => (int) Input.GetAxisRaw("Horizontal");

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (_stop)
            return;
        
        _movement = new Vector3(0, _rigidbody.velocity.y, HorizontalRaw * multVelocity);

        if (Input.GetButton("Fire1") && _isGrounded)
            Jump();

        switch (HorizontalRaw)
        {
            case -1:
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    180,
                    transform.eulerAngles.z
                );
                break;
            case 1:
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    0,
                    transform.eulerAngles.z
                );
                break;
        }
    }

    void FixedUpdate()
    {
        if (!_stop)
            _rigidbody.velocity = _movement;
    }

    void Jump()
    {
        _rigidbody.velocity = new Vector3(0, jumpForce, _rigidbody.velocity.z);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
            _isGrounded = true;
    }
    
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
            _isGrounded = false;
    }

    public void LockMovement() => _stop = true;
    public void UnlockMovement() => _stop = false;
}