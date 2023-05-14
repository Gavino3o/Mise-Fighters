using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnerData _enemySpawnerData;
    [SerializeField] public bool _isActive { get; private set; } = false;

    private float _timeSinceLastSpawn; // The time elapsed since the last enemy was spawned
    private int _spawnCount = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
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
            Vector3 spawnPosition = _enemySpawnerData.spawnLocations[randomSpawnIndex].position;

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
