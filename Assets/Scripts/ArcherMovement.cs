using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ArcherMovement : MonoBehaviour
{
    Rigidbody _rigidbody;
    Vector3 _movement;
    ArcherController _controller;
    
    
    bool _isGrounded = true;

    [SerializeField]
    float jumpForce = 10;

    [SerializeField]
    float multVelocity = 3;

    public bool locked;

    public bool Fliped = false;

    public int MovimentDirection()
    {
        var v = Math.Ceiling(_rigidbody.velocity.z);
        if (v > -2 && v < 2) return 0;
        if (v < -0.01) return -1;
        if (v > 0.01) return 1;
        return 0;
    }

    int horizontalRaw => (int) Input.GetAxisRaw("Horizontal");

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<ArcherController>();
    }

    void Update()
    {
        if (!locked && _controller.IsMine)
            _movement = new Vector3(0, _rigidbody.velocity.y, horizontalRaw * multVelocity);

        var v = MovimentDirection();
        if (v < 0)
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                180,
                transform.eulerAngles.z
            );

        if (v > 0)
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                0,
                transform.eulerAngles.z
            );
    }

    void FixedUpdate()
    {
        if (locked || !_controller.IsMine)
            return;
        _rigidbody.velocity = _movement;
        if (Input.GetButton("Fire1") && _isGrounded)
            Jump();
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


    public void LockMovement() => locked = true;
    public void UnlockMovement() => locked = false;
}