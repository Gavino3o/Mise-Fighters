using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using Unity.VisualScripting;

public class MeleeEnemyMovementNetwork : NetworkBehaviour
{

    [SerializeField] private float _minumumDistance;
    [SerializeField] private float _speed;
    private Transform _target;
    [SerializeField] private GameObject _players;

    // DO A* STAR PATHFINDING.
    private void Update()
    {
        if (Vector2.Distance(transform.position, _target.position) > _minumumDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

    }

    [ObserversRpc]
    public void MoveTowardsPlayer()
    {

    }

    [ServerRpc]
    public void MoveTowardsPlayserServer()
    {

    }
}
