using FishNet.Object;
using FishNet.Object.Synchronizing;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;

public class EnemyAutoAttack : NetworkBehaviour
{
    [SerializeField] private GameObject autoAttackPrefab;
    [SerializeField] private float stepbackSpeed;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float minDistanceFromPlayer;
    [SerializeField] private float attackLifetime;
    private float prevAttackTime;
    public EnemyType enemyType;

    private PlayerTargeter playerTargeter;
    private EnemyMovementController enemyMovementController;
    private EnemyAI enemyAI;
    private Rigidbody2D rb;

     void Start()
    {
        if (!IsServer) return;
        enemyAI = gameObject.GetComponent<EnemyAI>();
        playerTargeter = gameObject.GetComponent<PlayerTargeter>();
        enemyMovementController = gameObject.GetComponent<EnemyMovementController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (!IsServer) return;

        if (enemyAI.IsInAttackRange())
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

        if (Time.time - prevAttackTime < timeBetweenAttacks || !enemyAI.IsInAttackRange()) return;
        prevAttackTime = Time.time;
        AutoAttack(enemyType);
    }

    private void AutoAttack(EnemyType enemyType)
    {
        if (!IsServer) return;
        var attackPrefab = Instantiate(this.autoAttackPrefab, transform.position, Quaternion.identity);

        switch (enemyType)
        {
            case EnemyType.MeleeAttacker:
                var attackDirection = (playerTargeter.GetCurrentTargetPlayer().transform.position - gameObject.transform.position).normalized;
                attackPrefab.transform.rotation.SetLookRotation(attackDirection);
                break;
            case EnemyType.ArcShooter:
                if (attackPrefab.GetComponent<EnemyArcProjectile>() != null)
                {
                    attackPrefab.GetComponent<EnemyArcProjectile>().targetPosition = playerTargeter.GetCurrentTargetPlayer().transform.position;
                }
                break;
            case EnemyType.StraightShooter:
                if (attackPrefab.GetComponent<EnemyStraightProjectile>() != null)
                {
                    attackPrefab.GetComponent<EnemyStraightProjectile>().targetPosition = playerTargeter.GetCurrentTargetPlayer().transform.position;
                }
                break;
        }

        attackPrefab.GetComponent<CharacterDamager>().damage = enemyAI.currAttack;
        attackPrefab.GetComponent<Lifetime>().lifetime = attackLifetime;
        ServerManager.Spawn(attackPrefab);
    }

    public enum EnemyType
    {
       ArcShooter = 1,
       StraightShooter = 2,
       MeleeAttacker = 3
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

    /*private void RetreatFromPlayer()
    {
        var playerTransform = playerTargeter.GetCurrentTargetPlayer().transform.position;
        if (Vector2.Distance(transform.position, playerTransform) < minDistanceFromPlayer)
        {
            //gameObject.transform.position = Vector2.MoveTowards(transform.position, playerTransform, -speed * Time.deltaTime);
            rb.velocity = (transform.position - playerTransform).normalized * stepbackSpeed;
        }
    }*/
}
