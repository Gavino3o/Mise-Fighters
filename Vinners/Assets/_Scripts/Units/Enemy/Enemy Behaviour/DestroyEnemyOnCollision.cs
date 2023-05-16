using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class DestroyEnemyOnCollision : MonoBehaviour
{
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Collider2D>().CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
