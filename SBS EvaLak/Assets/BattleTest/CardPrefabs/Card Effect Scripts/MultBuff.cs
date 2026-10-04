using UnityEngine;

public class MultBuff : CardEffect
{
    /* 
    Important: a multIncrease of 1 will result in 2x damage, because the base mult is 1, and it's increased by 1,
    which results in a total mult of 2. Likewise, playing 2 of these cards increases mult to 3, tripling damage, etc.
    */
    public int multIncrease = 1;

    public override void Activate()
    {
        PlayModifiers mod = GameObject.FindWithTag("Modifiers").GetComponent<PlayModifiers>();

        mod.damageMult += 1;
    }
}