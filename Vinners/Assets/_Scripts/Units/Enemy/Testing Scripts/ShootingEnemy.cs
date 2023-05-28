using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : NetworkBehaviour
{
    public Transform player;
    public float speed;
    public float minimumDistance;
    public float attackRange;

    public GameObject projectile;
    public float timeBetweenShots;
    public float nextShotTime;

    public override void OnStartServer()
    { 
        base.OnStartServer();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        if (!IsServer) return;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MoveToAttackRange();
        ShootProjectile();
        RetreatFromPlayer();
    }

    private void ShootProjectile()
    {
        
        if (Time.time > nextShotTime && IsInRange())
        {
            var projectile = Instantiate(this.projectile, transform.position, Quaternion.identity);
            Spawn(projectile);
            nextShotTime = Time.time + timeBetweenShots;
        }
    }

    private void RetreatFromPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < minimumDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
    }

    private void MoveToAttackRange()
    {
        if  (!IsInRange())
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private bool IsInRange()
    {
        return Vector2.Distance(transform.position, player.position) < attackRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EnemyManager.DecrementCounter();
            this.Despawn();
        }
    }
}
