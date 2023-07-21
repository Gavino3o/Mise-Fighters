using FishNet.Object;
using FishNet.Connection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using FishNet.Object.Synchronizing;
using System.IO;

public class EnemyAI : Unit
{
   
    [SerializeField] private float attackRange;
    [SerializeField] private float maxScoreBonus;
    [SerializeField] private bool isBoss;
    [SerializeField] private bool canTeleport;
    protected Rigidbody2D rigidBody;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject scorePopUp;
    protected EnemyMovementController enemyMovementController;
    protected PlayerTargeter playerTargeter;

    private void Start()
    {
        enemyMovementController = GetComponent<EnemyMovementController>();
        playerTargeter = GetComponent<PlayerTargeter>();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;
        GetComponentInChildren<Collider2D>().isTrigger = true;


        enemyMovementController.StartAstarMovement();
    }

    private void FixedUpdate()
    {
        enemyMovementController.SetMaxMovementSpeed(currMoveSpeed);
    }


    public void TauntedBy(GameObject obj, float duration)
    {
        playerTargeter.ChangeTargetDuration(obj, duration);
    }

    public void EnemyTakeDamage(float dmg, GameObject player)
    {
        base.TakeDamage(dmg);
        if (currHealth <= 0)
        {
            OnDeath();
        }
        playerTargeter.ChangeTargetPlayer(player);
    }

    // TODO: Implement Score Pop Up and Death Effect
    public override void OnDeath()
    {
        if (!IsServer) return;
        //GameObject newPopUp = Instantiate(scorePopUp, transform.position, Quaternion.identity);
        //Spawn(newPopUp);
        //int randScoreBonus = Random.Range(1, maxScoreBonus);
        //ScoreManager.IncreaseScore(randScoreBonus);

        //var newDeathEffect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Spawn(newDeathEffect);

        enemyMovementController.StopAstarMovement();
        EnemyManager.Instance.IncrementDeathCount();
        Despawn(gameObject);
    }

    public bool IsInAttackRange()
    {
        var playerTransform = playerTargeter.GetCurrentTargetPlayer().transform.position;
        return Vector2.Distance(transform.position, playerTransform) < attackRange;
    }



    // The following methods are kept for testing purposes only

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        EnemyManager.DecrementCounter();
    //        this.Despawn();
    //    }
    //}
}
