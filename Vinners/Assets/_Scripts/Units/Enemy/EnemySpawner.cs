using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.VisualScripting;

public class EnemySpawner : NetworkBehaviour 
{
    [SerializeField] public EnemySpawnerData enemySpawnerData;
    [SerializeField] private bool isActive = false;
    [SyncVar] private int spawnCount = 0;
    
    public override void OnStartServer()
    {
        base.OnStartServer();

        if (enemySpawnerData.isBossSpawner)
        {
            SpawnBosses();
        } 
        else
        {
            StartCoroutine(SpawnEnemiesRoutine());
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsServer) return;
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        if (!IsServer) yield break;

        while (isActive)
        {
            if (spawnCount >= enemySpawnerData.enemiesToSpawn)
            {
                DeactivateSpawnner();
            }

            int randomIndex = Random.Range(0, enemySpawnerData.enemyPrefabs.Length);
            int randomSpawnIndex = Random.Range(0, enemySpawnerData.spawnLocations.Length);
            GameObject enemyPrefab = enemySpawnerData.enemyPrefabs[randomIndex];
            Vector3 spawnPosition = enemySpawnerData.spawnLocations[randomSpawnIndex].position;

            var newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            Spawn(newEnemy);
            spawnCount++;

            yield return new WaitForSeconds(enemySpawnerData.spawnRate);
        }
    }

    public void SpawnBosses()
    {
        int numOfBosses = enemySpawnerData.bossPrefabs.Length;

        for (int i = 0; i < numOfBosses; i++)
        {
            Transform bossSpawnPoint = enemySpawnerData.spawnLocations[i];
            GameObject bossPrefab = enemySpawnerData.bossPrefabs[i];

            var newBoss = Instantiate(bossPrefab, bossSpawnPoint);
            Spawn(newBoss);
            spawnCount++;
        }
    }

    public void ActivateSpawnner()
    {
        this.isActive = true;
        if (enemySpawnerData.isBossSpawner)
        {
            SpawnBosses();
        }
        else
        {
            StartCoroutine(SpawnEnemiesRoutine());
        }
    }

    public void DeactivateSpawnner()
    {
        this.isActive = false;
    }

    public void UpdateSpawnerData(EnemySpawnerData data)
    {
        enemySpawnerData = data;
    }
}
