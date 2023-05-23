using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FishNet.Object;

public sealed class WaveManager : NetworkBehaviour
{
    // Reference to the WaveData scriptable objects for the current wave
    public WaveData[] _waveDatas;
    private WaveData _currentWaveData;
    private int _currentWaveIndex;
    private List<EnemySpawner> _activeSpawners = new List<EnemySpawner>();

    public static WaveManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Called when the wave manager is started
    private void Start()
    {
        if (!IsServer) return;

        if (_waveDatas != null && _waveDatas.Length > 1)
        {
            _currentWaveData = _waveDatas[0];
            ActivateAllSpawners();
        }
          
    }

    private void StartNextWave()
    {
        if (!IsServer) return;

        _currentWaveIndex++;

        if (_currentWaveIndex < _waveDatas.Length && !_currentWaveData.isLastWave)
        {
            // Set the current wave to the next wave in the list
            _currentWaveData = _waveDatas[_currentWaveIndex];

            //Delay according to delay in current wave Data.
            Invoke(nameof(ActivateAllSpawners), _currentWaveData.waveDelay);

        }
        else
        {
            // All waves have been spawned, end the game or restart from the beginning
            // Use Event. ie: public event OnWaveEnd()
            Debug.Log("Waves progression ended successfully");
        }
    }

    public void ActivateAllSpawners()
    {
        if (!IsServer) return;

        foreach (EnemySpawner enemySpawner in _currentWaveData.enemySpawners)
        {
            
            EnemyManager.IncrementCounter(enemySpawner.GetMaxEnemyCount());
            Instantiate(enemySpawner, enemySpawner.transform.position, Quaternion.identity);
            enemySpawner.ActivateSpawnner();
            _activeSpawners.Add(enemySpawner);
        }
    }

    public void SpawnerComplete(EnemySpawner spawner)
    {
        if (!IsServer) return;

        // Remove the completed spawner from the list of active spawners
        _activeSpawners.Remove(spawner);

        // Check if all spawners have completed
        if (_activeSpawners.Count <= 0 && EnemyManager.GetEnemyCount() <= 0)
        {
            // Start the next wave
            StartNextWave();
        }
    }

}

