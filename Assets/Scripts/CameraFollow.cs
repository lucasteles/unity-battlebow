using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform _camera;
    Transform _player;
    float smoothTime = 0.2F;
    Vector3 velocity = Vector3.zero;
    void Start()
    {
        _camera = Camera.main.transform;
        _player = FindObjectsOfType<ArcherController>().FirstOrDefault(x => x.isMine)?.gameObject.transform;

    }

    void Update()
    {
        var pPosition = _player.position;
        var cPosition = _camera.position;
        var newPosition = new Vector3(cPosition.x, Math.Max(pPosition.y, 7)  , pPosition.z);
        
        cPosition = Vector3.SmoothDamp(cPosition, newPosition, ref velocity, smoothTime);
        _camera.position = cPosition;
    }
}
