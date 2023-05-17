using FishNet.Object;
using FishNet.Connection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : NetworkBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    public float _speed;
    private Transform _target;

    public int _damage;
    public int _health;
    public GameObject _deathEffect;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public GameObject _scorePopUp;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

        if(_health <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        GameObject instance = Instantiate(_scorePopUp, transform.position, Quaternion.identity);

        int randScoreBonus = Random.Range(1, 6);
        //target.GetComponent<Player>().score += randScoreBonus;

        Instantiate(_deathEffect, transform.position, Quaternion.identity);
        
        EnemyManager.RemoveActiveEnemy(this);
        EnemyManager.DecrementCounter();

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        EnemyManager.AddActiveEnemy(this);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        EnemyManager.RemoveActiveEnemy(this);
    }

    
}
