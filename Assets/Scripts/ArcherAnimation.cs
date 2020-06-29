using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class ArcherAnimation : MonoBehaviour
{
    readonly int _walk = Animator.StringToHash("Walk");
    readonly int _attack = Animator.StringToHash("Attack");
    readonly int _dead = Animator.StringToHash("Dead");
    
    ArcherController _controller;
    Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<ArcherController>();
    }
    
    
    public void Die()
    {
        _animator.SetBool(_dead, true);
    }

    public void ArrowTrigger()
    {
        _controller.ShotArrow();
    }

    public void EndAttack()
    {
        _animator.SetBool(_attack, false);
        _animator.SetBool(_walk, false);
        _controller.ReleaseAttack();
    }

    public void Waking(bool isWaking)
    {
        _animator.SetBool(_walk, isWaking);
    }

    public void Attack()
    {
        _animator.SetBool(_walk, false);
        _animator.SetBool(_attack, true);
    }

    public void Reset()
    {
        _animator.SetBool(_dead, false);
        _animator.SetBool(_attack, false);
        _animator.SetBool(_walk, false);
    }
}