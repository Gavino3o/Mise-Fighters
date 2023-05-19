using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStraightProjectile : EnemyProjectile
{
    private Transform _playerPosition;
    private Vector2 _target;
    //public GameObject _effect;

    void Start()
    {
        //_playerPosition = GameObject.GetComponent<Player>().transform;
        _target = _playerPosition.position;
    }

    void Update()
    {
        if (!IsServer) return;
        MoveToTargetLocation();
    }

    public void MoveToTargetLocation()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, _target) < 0.1f)
        {
            OnHit();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // TODO: Take damage from DamageSource
            // collision.GetComponent<Player>().TakeDamage();
            OnHit();
        }
    }

    private void OnHit()
    {
        if (!IsServer) return;
        //var effect = Instantiate(_effect, transform.position, Quaternion.identity);
        //Spawn(effect);
        OnTimeOut();
    }

    private void OnTimeOut()
    {
        if(!IsServer) return;
        Despawn(gameObject);
    }
    
}
