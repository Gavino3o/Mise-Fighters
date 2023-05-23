using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.VisualScripting;

public class EnemySpawner : NetworkBehaviour 
{
    [SerializeField] private EnemySpawnerData _enemySpawnerData;
    [SerializeField] private GameObject[] _bossPrefabs;
    [SerializeField] private bool _isActive = false;
 
    [SyncVar] private int _spawnCount = 0;
    private int _maxSpawnCount;
    
    public override void OnStartServer()
    {
        base.OnStartServer();

        _maxSpawnCount = _enemySpawnerData.maxEnemies;
        StartCoroutine(SpawnEnemiesRoutine());
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsServer) return;
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        if (!IsServer) yield break;

        while (_isActive)
        {
            // Notify Wave Manager this spawner has completed spawnning its enemies.
            if (_spawnCount >= _enemySpawnerData.maxEnemies)
            {
                DeactivateSpawnner();
                WaveManager.Instance.SpawnerComplete(this);
                yield break;
            }

            int randomIndex = Random.Range(0, _enemySpawnerData.enemyPrefabs.Length);
            int randomSpawnIndex = Random.Range(0, _enemySpawnerData.spawnLocations.Length);
            GameObject enemyPrefab = _enemySpawnerData.enemyPrefabs[randomIndex];
            Transform spawnPosition = _enemySpawnerData.spawnLocations[randomSpawnIndex];

            // Consider letting EnemyManager spawn enemies. Include Object Pooling.
            var newEnemy = Instantiate(enemyPrefab, spawnPosition.position, Quaternion.identity);
            Spawn(newEnemy);
            EnemyManager.IncrementCounter();

            _spawnCount++;
            yield return new WaitForSeconds(_enemySpawnerData.spawnRate);
        }
    }

    public void SpawnBosses()
    {

    }

    public void ActivateSpawnner()
    {
        this._isActive = true;
    }

    public void DeactivateSpawnner()
    {
        this._isActive = false;
    }

    public int GetMaxEnemyCount()
    {
        return _maxSpawnCount;
    }
}
