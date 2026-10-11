using TMPro;
using UnityEngine;

public class NumCardsInDeck : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buffText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI repairText;
    private Inventory inventory;

    private int _buffNum;
    public int buffNum
    {
        get { return _buffNum; }
        set
        {
            _buffNum = value;
            if (buffText.text != null)
            {
                buffText.text = "Num Buffs: " + buffNum.ToString();
            }
        }
    }
    private int _damageNum;
    public int damageNum
    {
        get { return _damageNum; }
        set
        {
            _damageNum = value;
            if (damageText.text != null)
            {
                damageText.text = "Num Basics: " + damageNum.ToString();
            }
        }
    }
    private int _comboNum;
    public int comboNum
    {
        get { return _comboNum; }
        set
        {
            _comboNum = value;
            if (comboText.text != null)
            {
                comboText.text = "Num Combos: " + comboNum.ToString();
            }
        }
    }
    private int _repairNum;
    public int repairNum
    {
        get { return _repairNum; }
        set
        {
            _repairNum = value;
            if (repairText.text != null)
            {
                repairText.text = "Num Repairs: " + repairNum.ToString();
            }
        }
    }

    void Start()
    {
        GameObject DDOL = GameObject.Find("DDOL");
        inventory = DDOL.GetComponent<Inventory>();

        buffNum = inventory.numMult;
        damageNum = inventory.numDamage;
        comboNum = inventory.numCombo;
        repairNum = inventory.numRepair;
    }

    public void AddCards(string cardType)
    {
        switch (cardType)
        {
            case "BuffMultCard-":
                buffNum++;
                inventory.numMult++;
                break;

            case "DamageCard-":
                damageNum++;
                inventory.numDamage++;
                break;

            case "RepairCard-":
                repairNum++;
                inventory.numRepair++;
                break;

            case "ComboCard-":
                comboNum++;
                inventory.numCombo++;
                break;

        }
    }

    public void RemoveCards(string cardType)
    {
        switch (cardType)
        {
            case "BuffMultCard-":
                buffNum--;
                inventory.numMult--;
                break;

            case "DamageCard-":
                damageNum--;
                inventory.numDamage--;
                break;

            case "RepairCard-":
                repairNum--;
                inventory.numRepair--;
                break;

            case "ComboCard-":
                comboNum--;
                inventory.numCombo--;
                break;

        }
    }

}
