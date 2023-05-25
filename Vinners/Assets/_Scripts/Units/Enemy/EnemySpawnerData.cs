using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerData", menuName = "EnemySpawnerData")]
public class EnemySpawnerData : ScriptableObject
{
    public GameObject[] enemyPrefabs;
    public GameObject[] bossPrefabs;
    public float spawnRate;
    public Transform[] spawnLocations;
    public int maxEnemies;
    public bool isBossSpawner;
}
