using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    /*
     * Denotes the maximum number of spawn point in current wave.
     */
    public int _maxSpawnPoints;
    public List<SpawnPoint> _spawnPoints;
    private List<SpawnPoint> _activatedSpawnPoints;

    public SpawnManager(int maxSpawnPoints, List<SpawnPoint> spawnPoints)
    {
        _maxSpawnPoints = maxSpawnPoints;
        _spawnPoints = spawnPoints;
    }


    public void ActivateMaxSpawners() 
    {
        int count = 0;
        while (count < _maxSpawnPoints)
        {
            this.ActivateRandomSpawnPoint();
        }
    }

    public void ActivateRandomSpawnPoint()
    {
        SpawnPoint chosenSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        chosenSpawnPoint.ActivateSpawnPoint();
        _activatedSpawnPoints.Add(chosenSpawnPoint);
    }

    public void DeactivateAllSpawnPoints()
    {
        foreach(var spawnPoint in _activatedSpawnPoints)
        {
            spawnPoint.DeactivateSpawnPoint();
        }
    }

}
