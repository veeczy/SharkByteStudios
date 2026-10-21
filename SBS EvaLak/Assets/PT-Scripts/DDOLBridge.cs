using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class DDOLBridge : MonoBehaviour
{
    private GameObject DDOL;
    [SerializeField] private Inventory inventory;

    [SerializeField] private GameObject addRemoveRef;
    [SerializeField] private AddRemoveCardScript addRemoveScript;
    [SerializeField] private ShopCardsShown shopCardsShown;

    public GameObject myButtonGameObject;
    public Button myButton;
    [SerializeField] private string cardType;

    [SerializeField] private int rareCost = 2;
    [SerializeField] private int legendaryCost = 4;

    private string sceneName;

    void Start()
    {
        //Debug.Log("DDOLBRIDGE START");
        Init();
    }

    public void AddRemoveCard()
    {
        Debug.Log("BUTTON CLICKED");

        myButtonGameObject = EventSystem.current.currentSelectedGameObject;
        if (myButtonGameObject == null) { Debug.Log("Unable to find GameObject."); }
        else { myButton = myButtonGameObject.GetComponent<Button>(); cardType = myButtonGameObject.name; Debug.Log("Selected " + cardType); }

        if (cardType == "") { Debug.Log("No card type found!"); }

        if (inventory == null)
        {
            Debug.Log("Inventory not found");
            return;
        }
        if (addRemoveScript.isAdd)
        {
            inventory.AddCardByType(cardType);
            return;
        }
        else
        {
            inventory.RemoveCardByType(cardType);
            return;
        }
    }

    public void PurchaseCard()
    {
        //inventory = DDOL.GetComponent<Inventory>();

        myButtonGameObject = EventSystem.current.currentSelectedGameObject;
        if (myButtonGameObject == null) { Debug.Log("Unable to find GameObject."); }
        else { myButton = myButtonGameObject.GetComponent<Button>(); cardType = myButtonGameObject.name; Debug.Log("Selected " + cardType); }

        if (cardType == "") { Debug.Log("No card type found!"); }

        if (inventory == null)
        {
            Debug.Log("Inventory not found");
            return;
        }
        switch (cardType)
        {
            case "LDamageCard-":
                if (inventory.totalScrap >= legendaryCost) { UpdateAndUnlock("LDamageCard-2-A"); inventory.RemoveScrap(legendaryCost); }

                break;
            case "LRepairCard-":
                if (inventory.totalScrap >= legendaryCost) { UpdateAndUnlock("LRepairCard-2-A"); inventory.RemoveScrap(legendaryCost); }
                break;
            case "LBuffMultCard-":
                if (inventory.totalScrap >= legendaryCost) { UpdateAndUnlock("LBuffMultCard-2"); inventory.RemoveScrap(legendaryCost); }
                break;
            case "RDamageCard-":
                if (inventory.totalScrap >= rareCost) { UpdateAndUnlock("RDamageCard-0-A"); inventory.RemoveScrap(rareCost); }
                break;
            case "RRepairCard-":
                if (inventory.totalScrap >= rareCost) { UpdateAndUnlock("RRepairCard-0-A"); inventory.RemoveScrap(rareCost); }
                break;
            case "RBuffMultCard-":
                if (inventory.totalScrap >= rareCost) { UpdateAndUnlock("RBuffMultCard-0-A"); inventory.RemoveScrap(rareCost); }
                break;
            case "RComboCard-":
                if (inventory.totalScrap >= rareCost) { UpdateAndUnlock("RComboCard-0-A"); inventory.RemoveScrap(rareCost); }
                break;
        }

        //if (cardType == "LDamageCard-" || cardType == "LRepairCard-" || cardType == "LBuffMultCard-")
        //{
        //    inventory.unlockedDeckList.Add(cardType);
        //    inventory.UpdateLockedCards();
        //    shopCardsShown.HideUnlockedCards();
        //}

        //if (cardType == "RDamageCard-" || cardType == "RRepairCard-" || cardType == "RBuffMultCard-" || cardType == "RComboCard-")
        //{
        //    inventory.unlockedDeckList.Add(cardType);
        //    inventory.UpdateLockedCards();
        //    shopCardsShown.HideUnlockedCards();
        //}
    }

    void Init()
    {


        if (DDOL == null)
        {
            DDOL = GameObject.Find("DDOL");
        }


        if (inventory == null)
        {
            inventory = DDOL.GetComponent<Inventory>();
        }

        sceneName = inventory.sceneName;

        if (addRemoveRef == null && sceneName == "PT_Deck_Build")
        {
            addRemoveScript = addRemoveRef.GetComponent<AddRemoveCardScript>();
        }

    }

    private void UpdateAndUnlock(string cardType)
    {
        inventory.unlockedDeckList.Add(cardType);
        inventory.UpdateLockedCards();
        shopCardsShown.HideUnlockedCards();
    }
}
