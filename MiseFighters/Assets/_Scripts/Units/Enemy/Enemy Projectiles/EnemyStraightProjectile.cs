using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStraightProjectile : EnemyProjectile
{
    private CharacterDamager characterDamager;
    //public GameObject effect;

    private Vector3 movementDirection;

    void Start()
    {
        if (!IsServer) return;
        characterDamager = gameObject.GetComponent<CharacterDamager>();
        characterDamager.damage = damage;
        gameObject.GetComponent<Lifetime>().lifetime = maxLifeTime;
        movementDirection = (targetPosition - transform.position).normalized;
    }

    void FixedUpdate()
    {
        if (!IsServer) return;
        MoveToTargetLocation();
    }

    public void MoveToTargetLocation()
    {
        transform.position += speed * Time.deltaTime * (Vector3)movementDirection;

        //transform.position = Vector2.MoveTowards(transform.position, _endPosition, speed * Time.deltaTime);
        // transform.position += speed * Time.deltaTime * (targetPosition - transform.position).normalized;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnHit();
        } 
    }

    private void OnHit()
    {    
        if (!IsServer) return;
        //var effect = Instantiate(_effect, transform.position, Quaternion.identity);
        //Spawn(effect);
        //this.Despawn();
    }
}
