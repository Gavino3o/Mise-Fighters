using FishNet.Object;
using FishNet.Object.Synchronizing;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyShooter : NetworkBehaviour
{
    public GameObject projectile;
    public float stepbackSpeed;
    public float attackRange;
    public float timeBetweenShots;
    private float nextShotTime;
    public float minDistanceFromPlayer;
    public float lifetime = 8;

    public PlayerTargeter playerTargeter;
    public EnemyMovementController enemyMovementController;
    public EnemyAI enemyAi;
    public Rigidbody2D rb;

     void Start()
    {
        if (!IsServer) return;
        playerTargeter = gameObject.GetComponent<PlayerTargeter>();
        enemyMovementController = gameObject.GetComponent<EnemyMovementController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (!IsServer) return;

        if (IsInAttackRange())
        {
            enemyMovementController.StopAstarMovement();
            //RetreatFromPlayer();
            Debug.Log("A* Movement Stopped by Enemy Shooter");
        } 
        else
        {
            enemyMovementController.StartAstarMovement();
            Debug.Log("A* Movement Started by Enemy Shooter");
        }

        if (Time.time >= nextShotTime && IsInAttackRange())
        {
            ShootProjectile();
            nextShotTime = Time.time + timeBetweenShots;
        }
            
    }

    private void ShootProjectile()
    {
        var projectile = Instantiate(this.projectile, transform.position, Quaternion.identity);
        if (projectile.GetComponent<EnemyArcProjectile>() != null)
        {
            projectile.GetComponent<EnemyArcProjectile>().targetPosition = playerTargeter.GetCurrentTargetPlayer().transform.position;
        }
        projectile.GetComponent<Lifetime>().lifetime = lifetime;
        Spawn(projectile);
        
        
    }

    private void RetreatFromPlayer()
    {
        var playerTransform = playerTargeter.GetCurrentTargetPlayer().transform.position;
        if (Vector2.Distance(transform.position, playerTransform) < minDistanceFromPlayer)
        {
            //gameObject.transform.position = Vector2.MoveTowards(transform.position, playerTransform, -speed * Time.deltaTime);
            rb.velocity = (transform.position - playerTransform).normalized * stepbackSpeed;
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
