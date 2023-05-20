using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : NetworkBehaviour
{
    public float _speed;
    public Transform _player;
    public float _minimumDistance;
    public float _attackRange;

    public GameObject _projectile;
    public float _timeBetweenShots;
    public float _nextShotTime;

    private void Start()
    {
        
    }


    void Update()
    {
        if (!IsServer) return;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        MoveToAttackRange();
        ShootProjectile();
        RetreatFromPlayer();
    }

    private void ShootProjectile()
    {
        
        if (Time.time > _nextShotTime && IsInRange())
        {
            var projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
            Spawn(projectile);
            _nextShotTime = Time.time + _timeBetweenShots;
        }
    }

    private void RetreatFromPlayer()
    {
        if (Vector2.Distance(transform.position, _player.position) < _minimumDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.position, -_speed * Time.deltaTime);
        }
    }

    private void MoveToAttackRange()
    {
        if  (!IsInRange())
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.position, _speed * Time.deltaTime);
        }
    }

    private bool IsInRange()
    {
        return Vector2.Distance(transform.position, _player.position) < _attackRange;
    }


}
