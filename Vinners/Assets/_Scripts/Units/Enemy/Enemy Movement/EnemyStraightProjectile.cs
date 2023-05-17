using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStraightProjectile : MonoBehaviour
{
    public float _speed;

    private Transform _playerPosition;
    private Vector2 target;
    //public GameObject effect;

    void Start()
    {
        _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        target = _playerPosition.position;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            //Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // collision.GetComponent<Player>().TakeDamage(1);       -- Call Damage Source
            // Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    
}
