using FishNet.Object;
using FishNet.Connection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using FishNet.Object.Synchronizing;
using System.IO;

public class EnemyAI : NetworkBehaviour
{

    [SyncVar] protected double _currentHealth;
    [SerializeField] public double _maxHealth;

    [SyncVar] private Transform _target;
    [SerializeField] public float _speed;
    
    public int _damage;
    public bool _isBoss;
    public bool _canTeleport;

    protected Rigidbody2D _rigidBody;
    //protected AIPathFinder _pathFinder;
    public GameObject _deathEffect;
    public GameObject _scorePopUp;
    

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidBody = GetComponent<Rigidbody2D>();
        _currentHealth = _maxHealth;
        //_pathFinder = GetComponent<AIPathFinder>();

        if (!IsServer)
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponentInChildren<Collider2D>().isTrigger = true;
            //_pathFinder.canMove = false;
            //_pathFinder.isStopped = true;
            //GetComponent<AIDestinationSetter>().enabled = false;
            //GetComponent<Seeker>().enabled = false;
            //_pathFinder.enabled = false;
        }
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

        if(_currentHealth <= 0)
        {
            OnDeath();
        }
    }

    // TODO: Take damage from player projectile or melee attacks
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

    public double GetCurrentHeath()
    {
        return _currentHealth;
    }

    public virtual void takeDamage(double dmg)
    {
        _currentHealth -= dmg;

        if (_currentHealth <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        if (!IsServer) return;

        GameObject scorePopUp = Instantiate(_scorePopUp, transform.position, Quaternion.identity);
        Spawn(scorePopUp);

        int randScoreBonus = Random.Range(1, 6);
        //Add Score Bonus to Team Score.

        var deathEffect = Instantiate(_deathEffect, transform.position, Quaternion.identity);
        Spawn(deathEffect);

        EnemyManager.RemoveActiveEnemy(this);
        EnemyManager.DecrementCounter();

        this.Despawn();
    }
}
