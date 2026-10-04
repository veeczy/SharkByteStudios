using UnityEngine;

public class PlayModifiers : MonoBehaviour
{
    /* 
    -- CLASS THAT STORES VALUES CARRIED ACROSS CARDS --
    This is so cards can affect future played cards, like if you play a card for 2x damage mult on next attack,
    this script keeps track of values carried across cards like that
    */

    [HideInInspector] public int damageMult = 1;
}
