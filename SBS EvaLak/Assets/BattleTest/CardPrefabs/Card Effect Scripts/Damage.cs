using UnityEngine;

public class Damage : CardEffect
{
    public int damage = 50;

    public bool useDamageRange = false;
    public int minDamage = 25;
    public int maxDamage = 75;

    private float temporaryHealth;

    public override void Activate()
    {
        PlayModifiers mod = GameObject.FindWithTag("Modifiers").GetComponent<PlayModifiers>();
        Healthbar enemyHP = GameObject.FindWithTag("Enemy").GetComponent<Healthbar>();

        int finalDamage = damage;

        if (useDamageRange)
        {
            finalDamage = Random.Range(minDamage, maxDamage + 1);
        }

        temporaryHealth = enemyHP.health; 

        temporaryHealth -= finalDamage * mod.damageMult;

        enemyHP.health = Mathf.RoundToInt(temporaryHealth);

        mod.damageMult = 1;
        mod.combo = 0;
    }
}