using FishNet.Example.Scened;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyArcProjectile : EnemyProjectile
{
    public Vector3 _startPosition;
    public Vector3 _targetPosition;
    public float arcHeight;
    public AutoTimer _timer;
    [SerializeField] double _maxTimeActive;
   
    public override void OnStartServer()
    {
        base.OnStartServer();

        _startPosition = transform.position;
        _startPosition.z = 0;
        _targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    void Update()
    {
        if (!IsServer) return;
        MoveToTargetLocation();
    }

    public void MoveToTargetLocation()
    {
        float x1 = _startPosition.x;
        float x2 = _targetPosition.x;
        float distance = x2 - x1;
        float nextX = Mathf.MoveTowards(transform.position.x, x2, speed * Time.deltaTime);
        float baseY = Mathf.Lerp(_startPosition.y, _targetPosition.y, (nextX - x1) / distance);
        float arc = arcHeight * (nextX - x1) * (nextX - x2) / (-0.25f * distance * distance);
        Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);

        // Rotate to face the next position, and then move there
        transform.rotation = LookAt2D(nextPosition - transform.position);
        transform.position = nextPosition;

        if (Vector3.Distance(transform.position, _targetPosition) < 1f)
        {
            Arrived();
        }
    }

    // TODO: 
    // Implement events to handle when a projectile lands on target position.
    // 1. Create an area that damages the player consistently over a period of time.
    // 2. A chuck of AOE Damage, like a bomb.
    private void Arrived()
    {
        this.Despawn();
    }

    public Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Arrived();
        }
    }
}