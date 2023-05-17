using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

public sealed class EnemyManager : NetworkBehaviour
{

    [SerializeField] private int enemyCount = 1;
    private List<Enemy> activeEnemies = new List<Enemy>();

    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        EnemyManager.Instance = this;
    }

    public static void AddActiveEnemy(Enemy enemy)
    {
        EnemyManager.Instance.activeEnemies.Add(enemy);
        IncrementCounter();
    }
    public static void RemoveActiveEnemy(Enemy enemy)
    {
        EnemyManager.Instance.activeEnemies.Remove(enemy);
        DecrementCounter(); 
    }
    public static void IncrementCounter()
    {
        EnemyManager.Instance.enemyCount++;
    }
    public static void DecrementCounter()
    {
        EnemyManager.Instance.enemyCount--;
    }
    public static int GetEnemyCount()
    {
        return EnemyManager.Instance.enemyCount;
    }
}
