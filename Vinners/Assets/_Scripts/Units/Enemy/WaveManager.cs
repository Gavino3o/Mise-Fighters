using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class WaveManager : MonoBehaviour
{
    // Reference to the WaveData scriptable objects for the current wave
    public WaveData[] waveDatas;
    private WaveData currentWaveData;
    private int currentWaveIndex;
    private List<EnemySpawner> activeSpawners = new List<EnemySpawner>();

    public static WaveManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Called when the wave manager is started
    private void Start()
    {
        if (waveDatas != null && waveDatas.Length > 1)
        {
            currentWaveData = waveDatas[0];
            ActivateAllSpawners();
        }
          
    }

    private void StartNextWave()
    {
        currentWaveIndex++;

        if (currentWaveIndex < waveDatas.Length && !currentWaveData.isLastWave)
        {
            // Set the current wave to the next wave in the list
            currentWaveData = waveDatas[currentWaveIndex];

            //Delay according to delay in current wave Data.
            Invoke(nameof(ActivateAllSpawners), currentWaveData.waveDelay);

        }
        else
        {
            // All waves have been spawned, end the game or restart from the beginning
            // Use Event. ie: public event OnWaveEnd()
        }
    }

    public void ActivateAllSpawners()
    {
        foreach (EnemySpawner enemySpawner in currentWaveData.enemySpawners)
        {
            enemySpawner.ActivateSpawnner();
            activeSpawners.Add(enemySpawner);
        }
    }

    public void SpawnerComplete(EnemySpawner spawner)
    {
        // Remove the completed spawner from the list of active spawners
        activeSpawners.Remove(spawner);

        // Check if all spawners have completed
        if (activeSpawners.Count == 0 && EnemyManager.GetEnemyCount() == 0)
        {
            // Start the next wave
            StartNextWave();
        }
    }

}

