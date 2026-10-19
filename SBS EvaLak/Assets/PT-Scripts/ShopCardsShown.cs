using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopCardsShown : MonoBehaviour
{
    private Inventory inventory;
    private bool isLocked = false;

    private GameObject tempGameObject;
    public GameObject CDamageCard, RDamageCard, LDamageCard;
    public GameObject CRepairCard, RRepairCard, LRepairCard;
    public GameObject CBuffMultCard, RBuffMultCard, LBuffMultCard;
    public GameObject CComboCard, RComboCard;

    public GameObject CDamageCardButton, RDamageCardButton, LDamageCardButton;
    public GameObject CRepairCardButton, RRepairCardButton, LRepairCardButton;
    public GameObject CBuffMultCardButton, RBuffMultCardButton, LBuffMultCardButton;
    public GameObject CComboCardButton, RComboCardButton;

    public TextMeshProUGUI scrapTotalMessage;

    public List<GameObject> CardGameObjects = new List<GameObject>();
    public List<GameObject> ButtonGameObjects = new List<GameObject>();
    public List<bool> checkLockCards = new List<bool>();
    void Start()
    {
        GameObject DDOL = GameObject.Find("DDOL");
        inventory = DDOL.GetComponent<Inventory>();
        if (inventory != null) { checkLockCards = inventory.checkLockList; }
        InitializeGameObjects();
        HideUnlockedCards();
    }

    void Update()
    {
        scrapTotalMessage.text = "Scrap: " + inventory.totalScrap;
    }
    public void HideUnlockedCards()
    {
        //get info from inventory and lock game objects if it is considered locked
        //currently this is set to what the index of the card name is in fulldecklist ... will need to change this to be more accepting of change in future in case of user input error when adding cards
        if (checkLockCards[0] == true || checkLockCards[1] == true || checkLockCards[2] == true) { Hide(RDamageCard); Hide(RDamageCardButton); }
        if (checkLockCards[3] == true || checkLockCards[4] == true || checkLockCards[5] == true) { Hide(CDamageCard); Hide(CDamageCardButton); }
        if (checkLockCards[6] == true || checkLockCards[7] == true || checkLockCards[8] == true) { Hide(LDamageCard); Hide(LDamageCardButton); }

        if (checkLockCards[9] == true || checkLockCards[10] == true || checkLockCards[11] == true) { Hide(RRepairCard); Hide(RRepairCardButton); }
        if (checkLockCards[12] == true || checkLockCards[13] == true || checkLockCards[14] == true) { Hide(CRepairCard); Hide(CRepairCardButton); }
        if (checkLockCards[15] == true || checkLockCards[16] == true) { Hide(LRepairCard); Hide(LRepairCardButton); }

        if (checkLockCards[17] == true) { Hide(RBuffMultCard); Hide(RBuffMultCardButton); }
        if (checkLockCards[18] == true || checkLockCards[19] == true && checkLockCards[20] == true) { Hide(CBuffMultCard); Hide(CBuffMultCardButton); }
        if (checkLockCards[21] == true) { Hide(LBuffMultCard); Hide(LBuffMultCardButton); }

        if (checkLockCards[22] == true || checkLockCards[23] == true || checkLockCards[24] == true || checkLockCards[25] == true || checkLockCards[26] == true || checkLockCards[27] == true || checkLockCards[28] == true) { Hide(RComboCard); Hide(RComboCardButton); }
        if (checkLockCards[29] == true || checkLockCards[30] == true || checkLockCards[31] == true || checkLockCards[32] == true || checkLockCards[33] == true || checkLockCards[34] == true || checkLockCards[35] == true) { Hide(CComboCard); Hide(CComboCardButton); }
    }

    public void Hide(GameObject GameObjectName)
    {
        GameObjectName.SetActive(false);
    }

    public void Show(GameObject GameObjectName) { GameObjectName.SetActive(true); }

    public void InitializeGameObjects()
    {
        //DAMAGE CARDS + THEIR BUTTONS
        if (CDamageCard == null)
        {
            CDamageCard = GameObject.Find("DamageCard-1-A");
        }
        if (RDamageCard == null)
        {
            RDamageCard = GameObject.Find("DamageCard-0-A");
        }
        if (LDamageCard == null)
        {
            LDamageCard = GameObject.Find("DamageCard-2-A");
        }

        if (CDamageCardButton == null)
        {
            CDamageCardButton = GameObject.Find("CDamageCard-");
        }
        if (RDamageCardButton == null)
        {
            RDamageCardButton = GameObject.Find("RDamageCard-");
        }
        if (LDamageCardButton == null)
        {
            LDamageCardButton = GameObject.Find("LDamageCard-");
        }


        //REPAIR CARDS + THEIR BUTTONS
        if (CRepairCard == null)
        {
            CRepairCard = GameObject.Find("RepairCard-1-A");
        }
        if (RRepairCard == null)
        {
            RRepairCard = GameObject.Find("RepairCard-0-A");
        }
        if (LRepairCard == null)
        {
            LRepairCard = GameObject.Find("RepairCard-2-A");
        }

        if (CRepairCardButton == null)
        {
            CRepairCardButton = GameObject.Find("CRepairCard-");
        }
        if (RRepairCardButton == null)
        {
            RRepairCardButton = GameObject.Find("RRepairCard-");
        }
        if (LRepairCardButton == null)
        {
            LRepairCardButton = GameObject.Find("LRepairCard-");
        }


        //BUFFMULT CARDS + THEIR BUTTONS
        if (CBuffMultCard == null)
        {
            CBuffMultCard = GameObject.Find("BuffMultCard-1-A");
        }
        if (RBuffMultCard == null)
        {
            RBuffMultCard = GameObject.Find("BuffMultCard-0");
        }
        if (LBuffMultCard == null)
        {
            LBuffMultCard = GameObject.Find("BuffMultCard-2");
        }

        if (CBuffMultCardButton == null)
        {
            CBuffMultCardButton = GameObject.Find("CBuffMultCard-");
        }
        if (RBuffMultCardButton == null)
        {
            RBuffMultCardButton = GameObject.Find("RBuffMultCard-");
        }
        if (LBuffMultCardButton == null)
        {
            LBuffMultCardButton = GameObject.Find("LBuffMultCard-");
        }


        //COMBO CARDS + THEIR BUTTONS
        if (CComboCard == null)
        {
            CComboCard = GameObject.Find("ComboCard-1-A");
        }
        if (RComboCard == null)
        {
            RComboCard = GameObject.Find("ComboCard-0-A");
        }

        if (CComboCardButton == null)
        {
            CComboCardButton = GameObject.Find("CComboCard-");
        }
        if (RComboCardButton == null)
        {
            RComboCardButton = GameObject.Find("RComboCard-");
        }
    }

    public void AddScrap()
    {
        inventory.AddScrap();
    }
}
