using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class ArcherController : MonoBehaviour, IPunObservable
{
    readonly int _walk = Animator.StringToHash("Walk");
    readonly int _attack = Animator.StringToHash("Attack");
    readonly int _dead = Animator.StringToHash("Dead");
    Animator _animator;
    Rigidbody _rb;
    ArcherMovement _movement;
    Transform _respawn;
    PhotonView _photonView;
    bool airAttack = false;

    public TextMesh PlayerName;
    public GameObject Crown;
    public bool IsMine => _photonView != null && _photonView.IsMine;
    public string Id => _photonView.Owner.NickName;

    [SerializeField]
    public int score;

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
        GameManager.Instance.AddPlayer(this);
    }

    void Update()
    {
        PlayerName.transform.forward = (PlayerName.transform.position - Camera.main.transform.position); //billboard
        // PlayerName.text = score.ToString();

        
        _animator.SetBool(_walk, _movement.MovimentDirection() != 0);
        Crown.SetActive(Id != null && GameManager.Instance.winningPlayer == Id);

        if (!IsMine) return;
        
        if (Input.GetKey(KeyCode.Escape))
            Respawn();
        
        if (Input.GetButton("Fire2"))
            _photonView.RPC(nameof(Attack), RpcTarget.All);
    }

    private void FixedUpdate()
    {
        if (_movement.IsGrounded)
            airAttack = false;
    }

    [PunRPC]
    public void Attack()
    {
        if (_movement.locked || airAttack)
            return;

        airAttack = true;
        _movement.LockMovement();
        _animator.SetBool(_walk, false);
        _animator.SetBool(_attack, true);
        _rb.velocity = Vector3.zero;
        _rb.useGravity = false;
        // Invoke(nameof(ForceUnlock), .5f);
    }
    
    public void Die()
    {
        _movement.LockMovement();
        SoundManager.Instance.PlayDie();
        _animator.SetBool(_dead, true);
        Invoke(nameof(Respawn), 3f);
    }

    public void Respawn()
    {
        score = 0;
        airAttack = false;
        _rb.useGravity = true;
        _animator.SetBool(_dead, false);
        _animator.SetBool(_attack, false);
        transform.position = _respawn.position;
        _rb.velocity = Vector3.zero;
        _movement.UnlockMovement();
    }

    void OnCollisionEnter(Collision other)
    {
        var otherObject = other.gameObject;
        if (otherObject.CompareTag("Shot") && other.collider.enabled)
        {
            var parent = otherObject.GetComponent<ArrowScript>()?.Parent;
            if (parent != null)
                parent.score++;
                
            Die();
        }
    }

    public void ArrowTrigger()
    {
        var arrow = Instantiate(arrowPrefab, arrowSpawn.position, transform.rotation);
        arrow.GetComponent<ArrowScript>().Parent = this;
    }

    public void EndAttack()
    {
        _animator.SetBool(_attack, false);
        _animator.SetBool(_walk, false);
        _rb.useGravity = true;
        _movement.UnlockMovement();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(score);
        }
        else
        {
            score = (int) stream.ReceiveNext();
        }
    }
}