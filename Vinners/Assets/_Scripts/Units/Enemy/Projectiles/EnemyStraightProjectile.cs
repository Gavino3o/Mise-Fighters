using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStraightProjectile : EnemyProjectile
{
    private Transform _targetPosition;
    private Vector2 _endPosition;
    //public GameObject _effect;

    void Start()
    {
        if (!IsServer) return;
        _targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
        _endPosition = _targetPosition.position;
    }

    void Update()
    {
        if (!IsServer) return;
        MoveToTargetLocation();
    }

    public void MoveToTargetLocation()
    {
        transform.position = Vector2.MoveTowards(transform.position, _endPosition, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, _endPosition) < 10f)
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
        this.Despawn();
    }
    
}
