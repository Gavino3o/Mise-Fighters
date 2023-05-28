using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using Unity.VisualScripting;

public class MeleeEnemyMovementNetwork : NetworkBehaviour
{

    [SerializeField] private float minumumDistance;
    [SerializeField] private float speed;
    [SerializeField] private GameObject players;
    private Transform target;

    // DO A* STAR PATHFINDING.
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    [ObserversRpc]
    public void MoveTowardsPlayer()
    {
        if (Vector2.Distance(transform.position, target.position) > minumumDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    [ServerRpc]
    public void MoveTowardsPlayserServer()
    {

    }
}
