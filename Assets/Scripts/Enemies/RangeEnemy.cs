using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RangeEnemy : EnemyBase
{
    [Header("Shoot")]
    [SerializeField] private float shootRange;
    [SerializeField] private float shootCooldown = 2;
    private float _shootCooldown = 0;
    [SerializeField] protected AudioClip shot;

    private Vector3 _shotDirection;
    private bool _isSawPlayer = false;
    
    private Transform _player;
    [Inject]
    private void Construct(Player player)
    {
        _player = player.transform;
    }

    public override void Attack()
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
        var playerDir = (_player.position - transform.position).normalized;
        Physics.Raycast(transform.position, playerDir,
            out var hit, invokeRange);
        if(hit.transform && hit.transform.gameObject.TryGetComponent<Player>(out var player))
        {
            if(!_isSawPlayer) source.PlayOneShot(sight);
            _isSawPlayer = true;
            if (Vector3.Distance(player.transform.position, transform.position) <= shootRange && _shootCooldown <= 0)
            {
                _shootCooldown = shootCooldown;
                _canMove = false;
                _rigidbody.velocity = Vector3.zero;
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
