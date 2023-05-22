using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

public sealed class EnemyManager : NetworkBehaviour
{

    [SyncVar] private int enemyCount = 0;
    private List<EnemyAI> activeEnemies = new List<EnemyAI>();

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

    public static void AddActiveEnemy(EnemyAI enemy)
    {
        EnemyManager.Instance.activeEnemies.Add(enemy);
        IncrementCounter();
    }

    public static void RemoveActiveEnemy(EnemyAI enemy)
    {
        EnemyManager.Instance.activeEnemies.Remove(enemy);
        DecrementCounter(); 
    }

    public static void IncrementCounter(int increment)
    {
        EnemyManager.Instance.enemyCount += increment;
    }

    public static void IncrementCounter()
    {
        EnemyManager.IncrementCounter(1);
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
