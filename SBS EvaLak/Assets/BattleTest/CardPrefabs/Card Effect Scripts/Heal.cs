using UnityEngine;

public class Heal : CardEffect
{
    public int healValue = 50;

    public override void Activate()
    {
        PlayModifiers mod = GameObject.FindWithTag("Modifiers").GetComponent<PlayModifiers>();
        Healthbar playerHP = GameObject.FindWithTag("Player").GetComponent<Healthbar>();

        playerHP.health = Mathf.Min(playerHP.health + healValue, playerHP.maxHealth);

        mod.combo = 0;
    }
}