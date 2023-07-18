using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

public sealed class EnemyManager : NetworkBehaviour
{
    public int enemyDeathCount;
   
    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        enemyDeathCount = 0;
    }

    public void IncrementDeathCount()
    {
        if (!IsServer) return;
        enemyDeathCount++;
        Debug.Log(enemyDeathCount.ToString() + " has died.");
    }

    public int GetEnemyDeathCount()
    {
        return EnemyManager.Instance.enemyDeathCount;
    }

    public void ResetDeathCount()
    {
        if (!IsServer) return;
        enemyDeathCount = 0;
        Debug.Log("Reset death count to 0.");
    }

    // The following methods are kept for testing

    //private static List<GameObject> activeEnemies;

    // public static void AddActiveEnemy(GameObject enemy)
    // {
    //     if (enemy.CompareTag("Enemy"))
    //     {
    //         EnemyManager.activeEnemies.Add(enemy);
    //         IncrementCounter();
    //     }
    // }

    //public static void RemoveActiveEnemy(GameObject enemy)
    // {
    //     if (enemy.CompareTag("Enemy"))
    //     {
    //         EnemyManager.activeEnemies.Remove(enemy);
    //         DecrementCounter();
    //     }
    // }

}
