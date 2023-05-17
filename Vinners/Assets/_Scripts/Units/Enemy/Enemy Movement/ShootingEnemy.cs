using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
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
        MoveToAttackRange();
        ShootProjectile();
        RetreatFromPlayer();
    }

    private void ShootProjectile()
    {
        if (Time.time > _nextShotTime)
        {
            Instantiate(_projectile, transform.position, Quaternion.identity);
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
        if  (Vector2.Distance(transform.position, _player.position) > _attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.position, _speed * Time.deltaTime);
        }
    }
}
