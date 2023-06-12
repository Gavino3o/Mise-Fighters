using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class DestroyOnCollision : NetworkBehaviour
{

    private void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("CollisionEnter");
        if (collision.collider.CompareTag("Enemy"))
        {
            //EnemyManager.RemoveActiveEnemy(collision.gameObject);
            EnemyManager.DecrementCounter();
            ServerManager.Despawn(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TriggerEnter");
        if (other.CompareTag("Player"))
        {
            //EnemyManager.RemoveActiveEnemy(other.gameObject);
            EnemyManager.DecrementCounter();
            ServerManager.Despawn(gameObject);
        }
    }
}
