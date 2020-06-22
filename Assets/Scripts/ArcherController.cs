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

    public TextMesh PlayerName;
    public GameObject Crown;
    public bool IsMine => _photonView != null && _photonView.IsMine;
    public string Id => _photonView.Owner.UserId;

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
        Crown.SetActive(false);
    }

    void Start()
    {
        PlayerName.text = _photonView.Owner.NickName;
    }

    void Update()
    {
        PlayerName.transform.forward = (PlayerName.transform.position - Camera.main.transform.position); //billboard
        
        _animator.SetBool(_walk, _movement.MovimentDirection() != 0);

        if (!IsMine) return;
        if (Input.GetButton("Fire2"))
            _photonView.RPC(nameof(Attack), RpcTarget.All);
        
        Crown.SetActive(GameManager.Instance.winningPlayer == Id);
    }

    [PunRPC]
    public void Attack()
    {
        _animator.SetBool(_attack, true);
        _movement.LockMovement();
    }
    
    [PunRPC]
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
        GameManager.Instance.Reset(this);
    }

    void OnCollisionEnter(Collision other)
    {
        var otherObject = other.gameObject;
        if (otherObject.CompareTag("Shot") && other.collider.enabled)
        {
            GameManager.Instance.AddScore(otherObject.GetComponent<ArrowScript>().Parent);
            _photonView.RPC(nameof(Die), RpcTarget.All);
        }
    }

    void ForceUnlock()
    {
        if (!_movement.locked) return;

        _movement.UnlockMovement();
        _animator.SetBool(_attack, false);
    }

    public void ArrowTrigger()
    {
        var arrow =Instantiate(arrowPrefab, arrowSpawn.position, transform.rotation);
        arrow.GetComponent<ArrowScript>().Parent = this;
        Invoke(nameof(ForceUnlock), .3f);
    }

    public void EndAttack()
    {
        _animator.SetBool(_attack, false);
        _movement.UnlockMovement();
    }
}