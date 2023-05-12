using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveTowardsPlayer : MonoBehaviour
{
    // Replace with tag later
    public GameObject _player;
    public float _movementSpeed;
    private float _distanceFromPlayer;
    private Vector2 _direction;

    void Start()
    {
        
    }

    void Update()
    {
        _distanceFromPlayer = Vector2.Distance(transform.position, _player.transform.position);
        _direction = _player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(this.transform.position, _player.transform.position, _movementSpeed * Time.deltaTime);
    }
}