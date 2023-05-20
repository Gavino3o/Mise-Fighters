using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class MoveTowardsPlayer : NetworkBehaviour
{
    // Replace with tag later
    public GameObject _player;
    public float _movementSpeed;
    private float _distanceFromPlayer;
    private Vector2 _direction;

    void Start()
    {
        if (!IsServer) return;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!IsServer) return;
        _distanceFromPlayer = Vector2.Distance(transform.position, _player.transform.position);
        _direction = _player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(this.transform.position, _player.transform.position, _movementSpeed * Time.deltaTime);
    }
}
