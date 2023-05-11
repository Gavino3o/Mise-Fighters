using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _commonEnemy;
    [SerializeField]
    private GameObject _enemyContainer;

    private int _enemyCount;
    public float spawnRangeAbsolute = 8f;

    public void SpawnEnemy(Vector3 spawnPosition)
    {
        GameObject newEnemy = Instantiate(_commonEnemy, spawnPosition, Quaternion.identity);
        newEnemy.transform.parent = _enemyContainer.transform;
        _enemyCount++;
    }

}
