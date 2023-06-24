using FishNet.Object;
using FishNet.Object.Synchronizing;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : NetworkBehaviour
{
    public GameObject projectile;
    public float speed;
    public float attackRange;
    public float timeBetweenShots;
    public float nextShotTime;
    public float minDistanceFromPlayer;

    public PlayerTargeter playerTargeter;

    public void Start()
    {
        if (!IsServer) return;
        playerTargeter = gameObject.GetComponent<PlayerTargeter>();
    }


    void Update()
    {
        if (!IsServer) return;
        ShootProjectile();
        RetreatFromPlayer();
    }

    private void ShootProjectile()
    {
        if (Time.time > nextShotTime && IsInAttackRange())
        {
            var projectile = Instantiate(this.projectile, transform.position, Quaternion.identity);
            Spawn(projectile);
            nextShotTime = Time.time + timeBetweenShots;
        }
    }

    private void RetreatFromPlayer()
    {
        var playerTransform = playerTargeter.GetCurrentTargetPlayer().transform.position;
        if (Vector2.Distance(transform.position, playerTransform) < minDistanceFromPlayer)
        {
            gameObject.transform.position = Vector2.MoveTowards(transform.position, playerTransform, -speed * Time.deltaTime);
        }
    }

    public bool IsInAttackRange()
    {
        var playerTransform = playerTargeter.GetCurrentTargetPlayer().transform.position;
        return Vector2.Distance(transform.position, playerTransform) < attackRange;
    }

    // The following methods are kept for testing purposes only.

    /*    
     private void MoveToAttackRange()
     {
        if (!IsInRange())
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
     }
    */


}
