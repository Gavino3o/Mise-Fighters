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
    [SerializeField] public float maxHealth;
    [SyncVar] private Transform target; //Replace with player targetter

    public bool isBoss;
    public bool canTeleport;

    protected Rigidbody2D rigidBody;
    public GameObject deathEffect;
    public GameObject scorePopUp;
    
    //TODO: Enemy Astar Pathfinding

    private void Start()
    { 
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rigidBody = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;

        if (!IsServer)
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponentInChildren<Collider2D>().isTrigger = true;
        }
    }

    private void Update()
    {
        if (!IsServer) return;

        if(currHealth <= 0)
        {
            Die();
        }
    }

    public double GetCurrentHeath()
    {
        return currHealth;
    }


    // TODO: Implement Score Pop Up and Death Effect
    public override void Die()
    {
        if (!IsServer) return;

        //GameObject newPopUp = Instantiate(scorePopUp, transform.position, Quaternion.identity);
        //Spawn(newPopUp);
        //int randScoreBonus = Random.Range(1, 6);
        //Add Score Bonus to Team Score.

        //var newDeathEffect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Spawn(newDeathEffect);

        EnemyManager.DecrementCounter();

        Despawn(gameObject);
    }
}
