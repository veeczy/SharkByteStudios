using UnityEngine;

public class Damage : CardEffect
{
    public int damage = 50;

    public override void Activate()
    {
        PlayModifiers mod = GameObject.FindWithTag("Modifiers").GetComponent<PlayModifiers>();

        Healthbar enemyHP = GameObject.FindWithTag("Enemy").GetComponent<Healthbar>();

        enemyHP.health -= damage * mod.damageMult;

        mod.damageMult = 1;
    }
}