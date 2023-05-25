using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "WaveData")]
public class WaveData : ScriptableObject
{
    public int waveNumber;
    public int totalEnemies;
    public float waveDelay;
    public bool isLastWave;
    public GameObject[] enemySpawnerPrefabs;
}
