using UnityEngine;

public class ComboDamage : CardEffect
{
    public int damage = 50;

    public bool useDamageRange = false;
    public int minDamage = 20;
    public int maxDamage = 30;

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

        // Each combo card played before this one doubles the damage: 1x, 2x, 4x, 8x...
        int comboMult = 1 << mod.combo;

        temporaryHealth = enemyHP.health;

        temporaryHealth -= finalDamage * mod.damageMult * comboMult;

        enemyHP.health = Mathf.RoundToInt(temporaryHealth);

        mod.damageMult = 1;
        mod.combo++;
    }
}