using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerData", menuName = "EnemySpawnerData")]
public class EnemySpawnerData : ScriptableObject
{
    public GameObject[] enemyPrefabs;
    public GameObject[] bossPrefabs;
    public float spawnRate;
    public Vector3[] spawnLocations;
    public int maxEnemies;
}
