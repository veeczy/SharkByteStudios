using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;

    private GameObject currentEnemy;

    void Awake()
    {
        SpawnEnemy(Random.Range(0, enemyPrefabs.Count));
    }

    public void SpawnEnemy(int index)
    {
        if (currentEnemy != null) Destroy(currentEnemy);

        currentEnemy = Instantiate(enemyPrefabs[index], transform);
    }
}