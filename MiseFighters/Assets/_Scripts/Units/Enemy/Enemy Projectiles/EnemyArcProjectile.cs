using FishNet.Example.Scened;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyArcProjectile : EnemyProjectile
{
    public Vector3 startPosition;
    [SerializeField] private float arcHeight;
    private CharacterDamager characterDamager;

    public override void OnStartServer()
    {
        base.OnStartServer();

        startPosition = transform.position;
        startPosition.z = 0;
        characterDamager = gameObject.GetComponent<CharacterDamager>();
        characterDamager.damage = damage;
        gameObject.GetComponent<Lifetime>().lifetime = maxLifeTime;
    }

    void FixedUpdate()
    {
        if (!IsServer) return;
        MoveToTargetLocation();
    }

    public void MoveToTargetLocation()
    {
        float x1 = startPosition.x;
        float x2 = targetPosition.x;
        float distance = x2 - x1;
        float nextX = Mathf.MoveTowards(transform.position.x, x2, speed * Time.deltaTime);
        float baseY = Mathf.Lerp(startPosition.y, targetPosition.y, (nextX - x1) / distance);
        float arc = arcHeight * (nextX - x1) * (nextX - x2) / (-0.25f * distance * distance);
        Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);

        // Rotate to face the next position, and then move there
        transform.rotation = LookAt2D(nextPosition - transform.position);
        transform.position = nextPosition;

        if (Vector3.Distance(transform.position, targetPosition) < 1f)
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


    // The following methods are kept for testing purposes only

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        Arrived();
    //    }
    //}
}
