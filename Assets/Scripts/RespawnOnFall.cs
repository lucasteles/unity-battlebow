using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOnFall : MonoBehaviour
{
    [SerializeField]
    Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject;
        var rb = other.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        player.transform.position = respawnPoint.position;
    }
}
