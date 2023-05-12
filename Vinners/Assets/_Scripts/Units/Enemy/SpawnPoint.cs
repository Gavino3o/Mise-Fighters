using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * The Spawn Point class represents a location in the game where enemies can spawn.
 * It contains information about the position, rotation, and scale of the spawn point.
 * (Potentially renamed to Spawner Class)
 */
public class SpawnPoint : MonoBehaviour
{
    public GameObject _enemyPrefab;
    private Vector3 _spawnPosition;
    private float _spawnDelay;
    private bool _isActive = false;
    private EnemyManager _enemyManager;

    // Let wave manager or spawn manager decide spawner position and period.
    SpawnPoint(float spawnDelay, Vector3 spawnPosition, EnemyManager enemyManager)
    {
        _spawnDelay = spawnDelay;
        _spawnPosition = spawnPosition;
        _enemyManager = enemyManager;
    }

    void Start()
    {
        transform.position = _spawnPosition;
    }

    // Discuss with Alvin regarding varying spawn rates.
    // TODO: Decide how to start coroutine
    IEnumerator SpawnRoutine()
    {
        while (_isActive)
        {
            _enemyManager.SpawnEnemy(_spawnPosition);
            yield return new WaitForSeconds(5.0f);
        }
    }

    public void SpawnEnemy()
    {
        // Replace with EnemyManager methods.
        Instantiate(_enemyPrefab, _spawnPosition, Quaternion.identity);
    }

    public void ActivateSpawnPoint()
    {
        this._isActive = true;
    }

    public void DeactivateSpawnPoint()
    {
        this._isActive = false;
    }

}
