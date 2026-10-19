using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject robotEnemy;
    [SerializeField] private GameObject corruptedRobot;
    [SerializeField] private GameObject reptileEnemy;

    private int num;
    void Start()
    {
        num = Random.Range(0, 2);
        //Debug.Log(num);
        switch (num)
        {
            case 0:
                robotEnemy.SetActive(true);
                corruptedRobot.SetActive(false);
                reptileEnemy.SetActive(false);
                break;
            case 1:
                robotEnemy.SetActive(false);
                corruptedRobot.SetActive(true);
                reptileEnemy.SetActive(false);
                break;
            case 2:
                robotEnemy.SetActive(false);
                corruptedRobot.SetActive(false);
                reptileEnemy.SetActive(true);
                break;
        }
    }


}
