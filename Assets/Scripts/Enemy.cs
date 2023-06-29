using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    [Header("Movement")]
    [SerializeField] private float invokeRange;
    [SerializeField] private float speed;
    private bool _canMove = true;
    
    [Header("Shoot")]
    [SerializeField] private float shootRange;
    [SerializeField] private float shootCooldown = 2;
    private float _shootCooldown = 0;
    
    [Header("Sounds")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip sight;
    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip damaged;
    [SerializeField] private AudioClip dead;
    
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private HasHealth _health;
    private Vector3 _shotDirection;
    private bool _isSawPlayer = false;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _health = GetComponent<HasHealth>();
        _health.damaged.AddListener(Hurt);
        _health.dead.AddListener(Death);
    }

    private void Death(GameObject _)
    {
        animator.SetTrigger("Dead");
        _rigidbody.isKinematic = true;
        _health.enabled = false;
        _collider.height = 0;
        _collider.center = new Vector3(0, -0.5f, 0);
        // Destroy(gameObject);
        source.PlayOneShot(dead);
    }
    private void Hurt(float damage)
    {
        animator.SetTrigger("Damaged");
        _canMove = true;
        source.PlayOneShot(damaged);
        // Destroy(gameObject);
    }
    public void Shoot()
    {
        Physics.Raycast(transform.position, _shotDirection,
            out var hit, invokeRange);
        _canMove = true;
        source.PlayOneShot(shot);
        if(!hit.transform || !hit.transform.gameObject.TryGetComponent<Player>(out var player)) return;
        player.health.TakeDamage(10);
    }

    private void Update()
    {
        var playerDir = (Player.Instance.transform.position - transform.position).normalized;
        Physics.Raycast(transform.position, playerDir,
            out var hit, invokeRange);
        if(hit.transform.gameObject.TryGetComponent<Player>(out var player))
        {
            if(!_isSawPlayer) source.PlayOneShot(sight);
            _isSawPlayer = true;
            if (Vector3.Distance(player.transform.position, transform.position) <= shootRange && _shootCooldown <= 0)
            {
                _shootCooldown = shootCooldown;
                _canMove = false;
                animator.SetTrigger("Shoot");
                _shotDirection = playerDir;
            }
            if(_canMove) _rigidbody.velocity = playerDir * speed;
        }
        _shootCooldown -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, invokeRange);
    }
}
