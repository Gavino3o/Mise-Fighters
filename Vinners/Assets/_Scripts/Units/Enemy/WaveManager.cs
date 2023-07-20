using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FishNet.Object;

public sealed class WaveManager : NetworkBehaviour
{
    [SerializeField] private WaveData[] waveDatas;
    private WaveData currentWaveData;
    private int currentWaveIndex;
    private int enemyCountBuffer = 3;
    private GameObject currentSpawnerPrefab;
    private EnemySpawner currentEnemySpawner;
    

    public static WaveManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartInitialWave();
    }

    private void Update()
    {
        if (!IsServer) return;
        if (CanStartNextWave())
        {
            Debug.Log("INITIATE CHANGE WAVE");
            StartNextWave();
        }     
    }

    public void StartInitialWave()
    {
        if (waveDatas != null && waveDatas.Length > 0)
        {
            Debug.Log("Activating all spawners");
            currentWaveIndex = 0;
            currentWaveData = waveDatas[0];
            currentSpawnerPrefab = currentWaveData.enemySpawnerPrefab;
            currentEnemySpawner = currentSpawnerPrefab.GetComponent<EnemySpawner>();
            ActivateSpawner();
        } 
        else
        {
            Debug.Log("No wave datas found");
            return;
        }
    }

    private void StartNextWave()
    {
        if (!IsServer) return;

        if (waveDatas == null && waveDatas.Length <= 0)
        {
            Debug.Log("No wave datas found");
            return;
        } 
        else
        {
            currentWaveIndex++;

            if (currentWaveIndex < waveDatas.Length)
            {
                EnemyManager.Instance.ResetDeathCount();
                EnemyManager.Instance.SetBossAliveStatus(true);
                currentWaveData = waveDatas[currentWaveIndex];
                currentSpawnerPrefab = currentWaveData.enemySpawnerPrefab;
                currentEnemySpawner = currentSpawnerPrefab.GetComponent<EnemySpawner>();
                ActivateSpawner();
                Debug.Log("Next Wave Started");
            }
            else
            {
                Debug.Log("Waves progression ended successfully");
                var remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (var enemy in remainingEnemies)
                {
                    Despawn(enemy);
                }
                GameManager.Instance.StageClear();
            }
        }
    }

    public void ActivateSpawner()
    {
        if (!IsServer) return;
        var newSpawner = Instantiate(currentSpawnerPrefab, currentSpawnerPrefab.transform.position, Quaternion.identity);
        ServerManager.Spawn(newSpawner);
        newSpawner.GetComponent<EnemySpawner>().ActivateSpawnner();
        Debug.Log("Spawner created and activated succesfully");
    }

    private bool CanStartNextWave()
    {
        if (!currentWaveData.isBossWave)
        {
            bool cond1 = EnemyManager.Instance.GetEnemyDeathCount() >= currentEnemySpawner.GetMaxSpawnCount();
            bool cond2 = !currentEnemySpawner.IsSpawnerActive();
            return cond1 && cond2;
        } 
        else
        {
            bool cond1 = EnemyManager.Instance.GetEnemyDeathCount() >= currentEnemySpawner.GetMaxSpawnCount();
            bool cond2 = !currentEnemySpawner.IsSpawnerActive();
            bool cond3 = !EnemyManager.Instance.isWaveBossAlive();

            Debug.Log("Condition 3 hit? : " + cond3.ToString());
            return cond1 && cond2 && cond3;
        }

    }


    // The following methods are kept for testing purposes only


    // private List<GameObject> activeSpawners = new List<GameObject>();

    //public void SpawnerComplete(GameObject spawner)
    //{
    //    if (!IsServer) return;

    //    if (spawner.GetComponent<EnemySpawner>() != null)
    //    {
    //        activeSpawners.Remove(spawner);

    //        if (activeSpawners.Count <= 0 && EnemyManager.GetEnemyDeathCount() <= 0)
    //        {
    //            StartNextWave();
    //        }
    //    }
    //}

}

