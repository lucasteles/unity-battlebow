using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class RespawnOnFall : MonoBehaviour
{
    [SerializeField]
    Transform respawnPoint;

    void OnCollisionEnter(Collision other)
    {
        var obj = other.gameObject;
        if (obj.CompareTag("Player"))
        {
            var controller = obj.GetComponent<ArcherController>();
            controller.Respawn();
        }
        else
        {
            Destroy(obj);
        }
    }
}