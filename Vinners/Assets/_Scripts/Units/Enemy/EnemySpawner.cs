using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.VisualScripting;

public class EnemySpawner : NetworkBehaviour 
{
    [SerializeField] private EnemySpawnerData enemySpawnerData;
    [SyncVar] private int spawnCount = 0;
    [SyncVar] private bool isActive;

    public override void OnStartServer()
    {
        base.OnStartServer();
        isActive = false;

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
                Debug.Log("Deactivating Spawner: " + isActive.ToString());
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

            var newBoss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            ServerManager.Spawn(newBoss);
            spawnCount++;
        }
        EnemyManager.Instance.SetBossAliveStatus(true);
        DeactivateSpawnner();
        Debug.Log("Deactivating Spawner: " + isActive.ToString());
    }

    public void ActivateSpawnner()
    {
        if (!IsServer) return;
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
        if (!IsServer) return;
        this.isActive = false;
    }

    public void UpdateSpawnerData(EnemySpawnerData data)
    {
        if (!IsServer) return;
        enemySpawnerData = data;
    }

    public int GetMaxSpawnCount()
    {
        return enemySpawnerData.enemiesToSpawn;
    }

    public bool isSpawnerActive()
    {
        return this.isActive;
    }
}
