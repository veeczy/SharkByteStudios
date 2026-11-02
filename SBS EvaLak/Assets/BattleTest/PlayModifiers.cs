using UnityEngine;

public class PlayModifiers : MonoBehaviour
{
    /* 
    -- CLASS THAT STORES VALUES CARRIED ACROSS CARDS --
    This is so cards can affect future played cards, like if you play a card for 2x damage mult on next attack,
    this script keeps track of values carried across cards like that
    */

    public float playerMaxHealth = 500;

    [HideInInspector] public float enemyMaxHealth;

    [SerializeField] public float damageMult = 1;
    public float basedamageMult = 1;

    [SerializeField] public int combo = 0;

    private Inventory inventory;
    private GameObject DDOL;
    [SerializeField] private string HeadSlot;
    [SerializeField] private string ArmsSlot;
    [SerializeField] private string LegsSlot;
    [SerializeField] private int HeadValue;
    [SerializeField] private int ArmsValue;
    [SerializeField] private int LegsValue;

    private void Start()
    {
        if (DDOL == null) { DDOL = GameObject.Find("DDOL"); inventory = DDOL.GetComponent<Inventory>(); }
        if (inventory != null)
        {
            HeadSlot = inventory.HeadCardSlot;
            ArmsSlot = inventory.ArmsCardSlot;
            LegsSlot = inventory.LegsCardSlot;

            HeadValue = inventory.HeadCardSlotValue;
            ArmsValue = inventory.ArmsCardSlotValue;
            LegsValue = inventory.LegsCardSlotValue;
        }

        CheckMech();
        damageMult = basedamageMult;
    }

    public void EndHand()
    {
        combo = 0;
    }

    void CheckMech()
    {
        float addDamage;
        if(HeadSlot == "damage")
        {
            addDamage = basedamageMult * IntPercentage(HeadValue);
            Debug.Log(addDamage);
            basedamageMult = addDamage;
        }

        if (ArmsSlot == "damage")
        {
            addDamage = basedamageMult * IntPercentage(HeadValue);
            basedamageMult = addDamage;
        }

        if (LegsSlot == "damage")
        {
            addDamage = basedamageMult * IntPercentage(HeadValue);
            basedamageMult = addDamage;
        }
    }

    float IntPercentage(int value)
    {
        float floatvalue = ((float)value) / 100; //make percentage
        floatvalue = floatvalue + 1; //plus 1 so its instead of 5% -> 105% 
        return floatvalue;
    }
}
