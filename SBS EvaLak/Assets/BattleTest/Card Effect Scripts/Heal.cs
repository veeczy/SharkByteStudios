using UnityEngine;

public class Heal : CardEffect
{
    public int healValue = 50;
    public bool targetPlayer;
    public override void Activate()
    {
        PlayModifiers mod = GameObject.FindWithTag("Modifiers").GetComponent<PlayModifiers>();
        Healthbar playerHP = GameObject.FindWithTag("Player").GetComponent<Healthbar>();
        Healthbar enemyHP = GameObject.FindWithTag("Enemy").GetComponent<Healthbar>();

        if (!targetPlayer) //if not target player (default)
        { playerHP.health = Mathf.Min(playerHP.health + healValue, playerHP.maxHealth); }
        if (targetPlayer) 
        { enemyHP.health = Mathf.Min(enemyHP.health + healValue, enemyHP.maxHealth); }

        mod.combo = 0;
    }
}