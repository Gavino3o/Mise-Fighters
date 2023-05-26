using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FishNet.Object;

public sealed class WaveManager : NetworkBehaviour
{
    // Reference to the WaveData scriptable objects for the current wave
    public WaveData[] waveDatas;
    private WaveData currentWaveData;
    private int currentWaveIndex;
    private List<GameObject> activeSpawners = new List<GameObject>();

    public static WaveManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        if (waveDatas != null && waveDatas.Length >= 1)
        {
            Debug.Log("Activate Spawners");
            currentWaveData = waveDatas[0];
            ActivateAllSpawners();
        }        
    }

    private void Update()
    {
        if (!IsServer) return;
    }

    private void StartNextWave()
    {
        if (!IsServer) return;

        currentWaveIndex++;

        if (currentWaveIndex < waveDatas.Length && !currentWaveData.isLastWave)
        {
            // Set the current wave to the next wave in the list
            currentWaveData = waveDatas[currentWaveIndex];

            //Delay according to delay in current wave data.
            Invoke(nameof(ActivateAllSpawners), currentWaveData.waveDelay);

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

        Debug.Log("Instantiate Spawners");
        foreach (GameObject enemySpawner in currentWaveData.enemySpawnerPrefabs)
        {
            var newSpawner = Instantiate(enemySpawner, enemySpawner.transform.position, Quaternion.identity);
            ServerManager.Spawn(newSpawner);
            newSpawner.GetComponent<EnemySpawner>().ActivateSpawnner();
            activeSpawners.Add(newSpawner);
            Debug.Log("SpawnSpawners");
        }
    }

    public void SpawnerComplete(GameObject spawner)
    {
        if (!IsServer) return;

        if (spawner.GetComponent<EnemySpawner>() != null)
        {
            activeSpawners.Remove(spawner);

            if (activeSpawners.Count <= 0 && EnemyManager.GetEnemyCount() <= 0)
            {
                StartNextWave();
            }
        }
    }

}

