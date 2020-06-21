using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimation : MonoBehaviour
{
    readonly int _walk = Animator.StringToHash("Walk");
    readonly int _attack = Animator.StringToHash("Attack");
    Animator _animator;
    ArcherMovement _movement;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<ArcherMovement>();
    }

    void Update()
    {
        _animator.SetBool(_walk, _movement.HorizontalRaw != 0);
    }

    public void Attack()
    {
        _animator.SetBool(_attack, true);
        _movement.LockMovement();
    }
    
    
    public void ArrowTrigger()
    {
        print(1);
    }
    
    public void EndAttack()
    {
        print(2);
        _animator.SetBool(_attack, false);
        _movement.UnlockMovement();
    }
    
}