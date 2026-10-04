using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int attackDamage = 100;

    private Healthbar playerHP;

    private int tracker = 0;

    void Start()
    {
        playerHP = GameObject.FindWithTag("Player").GetComponent<Healthbar>();
    }

    public void Attack()
    {
        if (tracker < 2)
            playerHP.health -= Random.Range(75, 125);
        else
        {
            playerHP.health -= Random.Range(75, 125) * 2;
            tracker = 0;
        }
            
        tracker++;
    }
}
