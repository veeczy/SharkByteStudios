using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float enemyMaxHealth = 750;
    public Healthbar enemyHP;

    void Start()
    {
        enemyHP = GameObject.FindWithTag("Enemy").GetComponent<Healthbar>();
    }
}
