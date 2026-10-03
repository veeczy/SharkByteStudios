using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int attackDamage = 100;

    public void Attack()
    {
        Healthbar playerHP = GameObject.FindWithTag("Player").GetComponent<Healthbar>();

        playerHP.health -= attackDamage;
    }
}
