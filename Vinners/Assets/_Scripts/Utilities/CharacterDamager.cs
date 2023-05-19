using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet;

public class CharacterDamager : NetworkBehaviour
{ 
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    [SerializeField] private float projectileSpeed;
    [SyncVar] private Vector2 movementDirection = Vector2.zero;


    public void SetDirection(Vector2 dir)
    {
        movementDirection = dir.normalized;
    }

    private void Update()
    {
        if (lifetime <= 0) Destroy(gameObject);
        
        lifetime -= Time.deltaTime;
        GetComponent<Rigidbody2D>().velocity = movementDirection * projectileSpeed;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character unit = other.gameObject.GetComponent<Character>();
        if (unit != null)
        {
            unit.TakeDamage(damage);
            Debug.Log($"{gameObject} dealt {damage} damage to {other.gameObject}!");
        }
    }

}
