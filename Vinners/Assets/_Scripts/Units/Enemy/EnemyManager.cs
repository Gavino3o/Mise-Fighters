using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private int enemyCount = 1;

    //TODO: Find uses for tracking active enemies
    private List<Enemy> activeEnemies = new List<Enemy>();

    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
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
