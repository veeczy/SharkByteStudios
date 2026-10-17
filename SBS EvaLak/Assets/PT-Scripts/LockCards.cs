using System.Collections.Generic;
using UnityEngine;

public class LockCards : MonoBehaviour
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

    public List<GameObject> CardGameObjects = new List<GameObject>();
    public List<GameObject> ButtonGameObjects = new List<GameObject>();
    public List<bool> checkLockCards = new List<bool>();
    void Start()
    {
        GameObject DDOL = GameObject.Find("DDOL");
        inventory = DDOL.GetComponent<Inventory>();
        if( inventory != null ) { checkLockCards = inventory.checkLockList; }
        InitializeGameObjects();
        HideLockedCards();
    }

    public void HideLockedCards()
    {
        //get info from inventory and lock game objects if it is considered locked
        //currently this is set to what the index of the card name is in fulldecklist ... will need to change this to be more accepting of change in future in case of user input error when adding cards
        if (checkLockCards[0] == false) { Hide(RDamageCard); Hide(RDamageCardButton); }
        if (checkLockCards[3] == false) { Hide(CDamageCard); Hide(CDamageCardButton); }
        if (checkLockCards[6] == false) { Hide(LDamageCard); Hide(LDamageCardButton); }

        if (checkLockCards[9] == false) { Hide(RRepairCard); Hide(RRepairCardButton); }
        if (checkLockCards[12] == false) { Hide(CRepairCard); Hide(CRepairCardButton); }
        if (checkLockCards[15] == false) { Hide(LRepairCard); Hide(LRepairCardButton); }

        if (checkLockCards[17] == false) { Hide(RBuffMultCard); Hide(RBuffMultCardButton); }
        if (checkLockCards[18] == false) { Hide(CBuffMultCard); Hide(CBuffMultCardButton); }
        if (checkLockCards[21] == false) { Hide(LBuffMultCard); Hide(LBuffMultCardButton); }

        if (checkLockCards[22] == false) { Hide(RComboCard); Hide(RComboCardButton); }
        if (checkLockCards[29] == false) { Hide(CComboCard); Hide(CComboCardButton); }
    }

    public void Hide(GameObject GameObjectName)
    {
        GameObjectName.SetActive(false);
    }
    
    public void InitializeGameObjects()
    {
        //DAMAGE CARDS + THEIR BUTTONS
        if(CDamageCard == null)
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
}
