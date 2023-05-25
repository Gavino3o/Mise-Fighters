using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

public sealed class EnemyManager : NetworkBehaviour
{
    [SyncVar] private int enemyCount;
    private static List<GameObject> activeEnemies;
    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        enemyCount = 0;
    }

    public static void IncrementCounter(int increment)
    {
        EnemyManager.Instance.enemyCount += increment;
    }
  
    public static void IncrementCounter()
    {
        EnemyManager.IncrementCounter(1);
        Debug.Log(GetEnemyCount());
    }
 
    public static void DecrementCounter(int decrement)
    {
        EnemyManager.Instance.enemyCount -= decrement;
    }

    public static void DecrementCounter()
    {
        EnemyManager.DecrementCounter(1);
        Debug.Log(GetEnemyCount());
    }

    public static int GetEnemyCount()
    {
        return EnemyManager.Instance.enemyCount;
    }
 
    public static void AddActiveEnemy(GameObject enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            EnemyManager.activeEnemies.Add(enemy);
            IncrementCounter();
        }
    }

   public static void RemoveActiveEnemy(GameObject enemy)
    {
        if (enemy.CompareTag("Enemy"))
        {
            EnemyManager.activeEnemies.Remove(enemy);
            DecrementCounter();
        }
    }
    
}
