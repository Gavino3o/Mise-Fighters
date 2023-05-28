using FishNet.Object;
using FishNet.Object.Synchronizing;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : NetworkBehaviour
{
    public float speed;
    public float minimumDistance;
    public float attackRange;
    public float movementUpdateTime;

    public GameObject projectile;
    public float timeBetweenShots;
    public float nextShotTime;

    public EnemyMovementController enemyMovementController;
    public PlayerTargeter playerTargeter;
    public Transform playerTransform;

    private void Start()
    {
        playerTargeter = gameObject.GetComponent<PlayerTargeter>();
        playerTransform = playerTargeter.GetCurrentTargetPlayer().transform;
        enemyMovementController = gameObject.GetComponent<EnemyMovementController>();
        StartCoroutine(nameof(MovementSwitchCoroutine));
    }


    void Update()
    {
        if (!IsServer) return;
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
        if (Vector2.Distance(transform.position, playerTransform.position) < minimumDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, -speed * Time.deltaTime);
        }
    }

    public IEnumerable MovementSwitchCoroutine()
    {
        if (!IsInRange() && !enemyMovementController.IsActive())
        {
            enemyMovementController.StartAstarMovement();
        }
        else if (IsInRange() && enemyMovementController.IsActive())
        {
            enemyMovementController.StopAstarMovement();
        }
        else { }

        yield return new WaitForSeconds(movementUpdateTime);
    }

    private bool IsInRange()
    {
        return Vector2.Distance(transform.position, playerTransform.position) < attackRange;
    }


    // The following methods are kept for testing purposes only.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EnemyManager.DecrementCounter();
            this.Despawn();
        }
    }

    private void MoveToAttackRange()
    {
        if (!IsInRange())
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
    }
}
