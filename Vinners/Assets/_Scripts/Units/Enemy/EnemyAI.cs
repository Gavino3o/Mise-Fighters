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
    [SerializeField] 
    public float maxHealth;
    [SerializeField]
    public float maxScoreBonus;
    public bool isBoss;
    public bool canTeleport;
    protected Rigidbody2D rigidBody;
    public GameObject deathEffect;
    public GameObject scorePopUp;
    private EnemyMovementController enemyMovementController;
    private PlayerTargeter playerTargeter;


    private void Start()
    {
        currHealth = maxHealth;
        enemyMovementController = GetComponent<EnemyMovementController>();
        playerTargeter = GetComponent<PlayerTargeter>();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;
        GetComponentInChildren<Collider2D>().isTrigger = true;
        enemyMovementController.SetMaxMovementSpeed(currMoveSpeed);

        enemyMovementController.StartAstarMovement();
    }

    private void Update()
    {
        if (!IsServer) return;
    }

    public void EnemyTakeDamage(float dmg, GameObject player)
    {
        base.TakeDamage(dmg);

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
        EnemyManager.DecrementCounter();
        Despawn(gameObject);
    }
}
