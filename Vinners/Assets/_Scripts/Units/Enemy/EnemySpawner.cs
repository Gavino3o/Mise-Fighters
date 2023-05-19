using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class EnemySpawner : NetworkBehaviour 
{
    [SerializeField] private EnemySpawnerData _enemySpawnerData;
    [SerializeField] private bool _isActive { get; set; } = false;

    private float _timeSinceLastSpawn; // The time elapsed since the last enemy was spawned
    private int _spawnCount = 0;
    private List<GameObject> _currentActiveEnemies;

    [SyncVar] private bool _spawningComplete = false;

    AutoTimer countdownTimer;

    public override void OnStartServer()
    {
        base.OnStartServer();

        StartCoroutine(SpawnEnemiesRoutine());
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsServer)
            return;
        foreach (AutoTimer time in GetComponents<AutoTimer>())
        {
            time.StopTimer();
        }
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (_isActive)
        {
            if (_spawnCount >= _enemySpawnerData.maxEnemies)
            {
                DeactivateSpawnner();
                WaveManager.Instance.SpawnerComplete(this);
                yield return new WaitForSeconds(0f);
            }

            int randomIndex = Random.Range(0, _enemySpawnerData.enemyPrefabs.Length);
            GameObject enemyPrefab = _enemySpawnerData.enemyPrefabs[randomIndex];
            int randomSpawnIndex = Random.Range(0, _enemySpawnerData.spawnLocations.Length);
            Vector3 spawnPosition = _enemySpawnerData.spawnLocations[randomSpawnIndex];

            var newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            Spawn(newEnemy);
            EnemyManager.IncrementCounter();

            _spawnCount++;
            yield return new WaitForSeconds(_enemySpawnerData.spawnRate);
        }
    }

    public void ActivateSpawnner()
    {
        this._isActive = true;
    }

    public void DeactivateSpawnner()
    {
        this._isActive = false;
    }

}
