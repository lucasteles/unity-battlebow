using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    readonly int _walk = Animator.StringToHash("Walk");
    readonly int _attack = Animator.StringToHash("Attack");
    readonly int _dead = Animator.StringToHash("Dead");
    Animator _animator;
    Rigidbody _rb;
    ArcherMovement _movement;
    Transform _respawn;
    PhotonView _photonView;

    public bool IsMine => _photonView != null && _photonView.IsMine;
    
    [SerializeField]
    GameObject arrowPrefab;
    [SerializeField]
    Transform arrowSpawn;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<ArcherMovement>();
        _rb = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
        _respawn = GameObject.Find("RespawnPoint").transform;
    }

    void Update()
    {
        if (!IsMine)
            return;
        
        _animator.SetBool(_walk, _movement.HorizontalRaw != 0);
        
        if (Input.GetButton("Fire2"))
        {
            Attack();
        }
    }

    public void Attack()
    {
        _animator.SetBool(_attack, true);
        _movement.LockMovement();
    }
    
    public void Die()
    {
        _movement.LockMovement();
        _animator.SetBool(_dead, true);
        Invoke(nameof(Respawn), 3f);
    }
    public void Respawn()
    {
        _animator.SetBool(_dead, false);
        transform.position = _respawn.position;
        _rb.velocity = Vector3.zero;
        _movement.UnlockMovement();
    }

    void OnCollisionEnter(Collision other)
    {
        var otherObject = other.gameObject;
        if (otherObject.CompareTag("Shot"))
            Die();
    }


    void ForceUnlock()
    {
        if (!_movement.locked) return;
        
        _movement.UnlockMovement();
        _animator.SetBool(_attack, false);
    }
    
    public void ArrowTrigger()
    {
        Instantiate(arrowPrefab, arrowSpawn.position, transform.rotation);
        Invoke(nameof(ForceUnlock),.3f);
    }
    
    public void EndAttack()
    {
        _animator.SetBool(_attack, false);
        _movement.UnlockMovement();
    }
    
}