using TMPro;
using UnityEngine;

public class NumCardsInDeck : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buffText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI repairText;

    [SerializeField] private TextMeshProUGUI DeckSizeText;
    [SerializeField] private TextMeshProUGUI DuplicatesText;
    [SerializeField] private TextMeshProUGUI ScrapValueText;

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
                buffText.text = "Num Buffs: " + buffNum;
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
                damageText.text = "Num Basics: " + damageNum;
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
                comboText.text = "Num Combos: " + comboNum;
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
                repairText.text = "Num Repairs: " + repairNum;
            }
        }
    }

    private int _DeckSize;
    public int DeckSize
    {
        get { return _DeckSize; }
        set
        {
            _DeckSize = value;
            if (DeckSizeText.text != null)
            {
                int fullDeckLength = inventory.deckSize;
                string tempString = "/" + fullDeckLength;
                DeckSizeText.text = ($"Deck Size: {DeckSize}/{fullDeckLength}");
            }
        }
    }

    private int _duplicates;
    public int Duplicates
    {
        get { return _duplicates; }
        set
        {
            _duplicates = value;
            if (DuplicatesText.text != null)
            {
                DuplicatesText.text = ($"Duplicates: {Duplicates}");
            }
        }
    }

    private int _scrapValue;
    public int ScrapValue
    {
        get { return _scrapValue; }
        set
        {
            _scrapValue = value;
            if (ScrapValueText.text != null)
            {
                ScrapValueText.text = ($"Scrap: {ScrapValue}");
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
        DeckSize = inventory.deckList.Count;
        Duplicates = inventory.duplicateCards;
        ScrapValue = inventory.totalScrap;
    }

    public void AddCards(string cardType)
    {
        switch (cardType)
        {
            case "CBuffMultCard-":
            case "RBuffMultCard-":
            case "LBuffMultCard-":
                buffNum++;
                inventory.numMult++;
                DeckSize++;
                break;

            case "CDamageCard-":
            case "RDamageCard-":
            case "LDamageCard-":
                damageNum++;
                inventory.numDamage++;
                DeckSize++;
                break;

            case "CRepairCard-":
            case "RRepairCard-":
            case "LRepairCard-":
                repairNum++;
                inventory.numRepair++;
                DeckSize++;
                break;

            case "CComboCard-":
            case "RComboCard-":
            case "LComboCard-":
                comboNum++;
                inventory.numCombo++;
                DeckSize++;
                break;

        }
    }

    public void RemoveCards(string cardType)
    {
        switch (cardType)
        {
            case "CBuffMultCard-":
            case "RBuffMultCard-":
            case "LBuffMultCard-":
                buffNum--;
                inventory.numMult--;
                DeckSize--;
                break;

            case "CDamageCard-":
            case "RDamageCard-":
            case "LDamageCard-":
                damageNum--;
                inventory.numDamage--;
                DeckSize--;
                break;

            case "CRepairCard-":
            case "RRepairCard-":
            case "LRepairCard-":
                repairNum--;
                inventory.numRepair--;
                DeckSize--;
                break;

            case "CComboCard-":
            case "RComboCard-":
            case "LComboCard-":
                comboNum--;
                inventory.numCombo--;
                DeckSize--;
                break;

        }
    }

}
