using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "WaveData")]
public class WaveData : ScriptableObject
{
    public float waveDelay;
    public GameObject enemySpawnerPrefab;
    public bool isBossWave;
}
