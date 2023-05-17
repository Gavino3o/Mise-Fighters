using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;

public class CharacterDamager : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    private void Update()
    {
        if (lifetime <= 0) Destroy(gameObject);
        lifetime -= Time.deltaTime;
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
