using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class DestroyEnemyOnCollision : NetworkBehaviour
{
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Despawn(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Collider2D>().CompareTag("Enemy"))
        {
            Despawn(other.gameObject);
        }
    }
}
