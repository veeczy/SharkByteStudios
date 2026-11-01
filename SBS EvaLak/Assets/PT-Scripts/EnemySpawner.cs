using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;

    private GameObject currentEnemy;

    private GameObject DDOL;
    private Inventory inventory;
    private string planetInfo;

    void Awake()
    {
        DDOL = GameObject.Find("DDOL");
        inventory = DDOL.GetComponent<Inventory>();
        planetInfo = inventory.planetInfo;

        switch (planetInfo)
        {
            case "Nature Planet":
                SpawnEnemy(1);
                break;
            case "Robot Planet":
                SpawnEnemy(2);
                break;
            case "Round Planet":
                SpawnEnemy(0);
                break;
            case "Undead Planet":
                //SpawnEnemy(3);

                //there is no undead enemy model so ill just reuse the default
                SpawnEnemy(Random.Range(0, enemyPrefabs.Count));
                break;
            default: //if cant find planet info
                SpawnEnemy(Random.Range(0, enemyPrefabs.Count));
                break;
        }
    }

    public void SpawnEnemy(int index)
    {
        if (currentEnemy != null) Destroy(currentEnemy);

        currentEnemy = Instantiate(enemyPrefabs[index], transform);
    }
}