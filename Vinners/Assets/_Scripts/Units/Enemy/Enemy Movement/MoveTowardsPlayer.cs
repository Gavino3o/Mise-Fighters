using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class MoveTowardsPlayer : NetworkBehaviour
{
    // Replace with tag later
    public GameObject player;
    public float movementSpeed;
    private float distanceFromPlayer;
    private Vector2 direction;

    void Start()
    {
        if (!IsServer) return;

        player = GameObject.FindGameObjectWithTag("Player"); 
    }

    void Update()
    {
        if (!IsServer) return;
        player = GameObject.FindGameObjectWithTag("Player");
        distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);
        direction = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, movementSpeed * Time.deltaTime);
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
