using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MechUpgradeManager : MonoBehaviour
{
    [Header("Init")]
    private Inventory inventory;
    private GameObject DDOL;
    [SerializeField] private CameraControl cameraControl;
    [SerializeField] private GameObject mainCamera;

    [Header("Card Information")]
    [SerializeField] private string cardName;
    [SerializeField] private List<string> mechDeckList; //list of mech cards unlocked by player
    [SerializeField] private List<string> fullmechDeckList; //full mech deck, stored in inventory so rewards can access it
    private string cardChange; //shorthand what does card change
    public enum CardType { head, arms, legs }
    public CardType type;
    [SerializeField] private List<CardType> mechCardType; //list of all locations for full mech deck

    [SerializeField] private List<string> cardEffectList; //list of all possible effects for full mech deck
    private string cardEffect;

    public List<string> cardEffectsActive; //list of active effects from the cards in the mech upgrade script, to send to inventory

    [SerializeField] private List<int> cardEffectValueList; //these are the values for the effects of each card, so if damage is 5 then that should be 5 at the index for that card here


    [Header("UI Setup")]
    public List<GameObject> UICardObjectList;
    [SerializeField] private GameObject SortCards;
    //data stored for button press

    [SerializeField] private Color HeadColor;
    [SerializeField] private Color ArmsColor;
    [SerializeField] private Color LegsColor;

    [SerializeField] private Color tempcolor;
    [SerializeField] private Image tempimage;
    [SerializeField] private GameObject temp;
    [SerializeField] private int index;
    [SerializeField] private CardType SlotType;

    //buttons from Camera Control
    [SerializeField] private GameObject upHeadButton;
    [SerializeField] private GameObject upArmsButton;
    [SerializeField] private GameObject downArmsButton;
    [SerializeField] private GameObject downLegsButton;

    private TextMeshProUGUI cardNameText;
    [SerializeField] private GameObject currentButton;
    [SerializeField] private GameObject lastButton;

    [Header("Send To Inventory")]
    [SerializeField] private string HeadCardSlot;
    [SerializeField] private string ArmsCardSlot;
    [SerializeField] private string LegsCardSlot;

    [SerializeField] private string specialEffect1; //if there is a special effect, like an ability to play twice it stores here
    [SerializeField] private string specialEffect2;
    [SerializeField] private string specialEffect3;

    [SerializeField] private int HeadCardSlotValue; //int of the value of the effect, FOR LATER rn it defaults to plus 1
    [SerializeField] private int ArmsCardSlotValue;
    [SerializeField] private int LegsCardSlotValue;


    private void Start()
    {
        if (DDOL == null) { DDOL = GameObject.Find("DDOL"); inventory = DDOL.GetComponent<Inventory>(); }
        if (inventory != null) { mechDeckList = inventory.mechDeckList; fullmechDeckList = inventory.fullmechDeckList; cardEffectList = inventory.cardEffectList; cardEffectValueList = inventory.cardEffectValueList; }
        if (mainCamera == null) { mainCamera = GameObject.Find("Main Camera"); cameraControl = mainCamera.GetComponent<CameraControl>(); }
        if (upHeadButton == null)
        {
            upHeadButton = GameObject.Find("Head");
            upArmsButton = GameObject.Find("Arms Upper");

            downArmsButton = GameObject.Find("Arms Lower");
            downLegsButton = GameObject.Find("Legs");
        }

        if (SortCards == null) { SortCards = GameObject.Find("SortCards"); }

        GetCards();
        DisplayCards();
        GetInventoryData();
    }

    private void Update()
    {
        if (currentButton != EventSystem.current.currentSelectedGameObject) { lastButton = currentButton; }

        StartCoroutine(WaitForUpdate());

        //UpdateInventoryData();
    }

    void GetCards() //get all card slots from ui grid, put them in list
    {
        GameObject temp;
        foreach (Transform child in SortCards.transform) //get children buttons from the grid
        {
            temp = child.gameObject;
            UICardObjectList.Add(temp);
        }

        foreach (GameObject CardSlot in UICardObjectList) //set them all inactive ...
        { 
            CardSlot.SetActive(false);
        }
    }

    Color GetColor(string card) //grab the color data for the card and update it when rendering the cards, this can also be where the sprite is updated in future
    {
        
        index = fullmechDeckList.IndexOf(card);
        type = mechCardType[index];

        switch(type)
        {
            case CardType.head:
                return HeadColor;
            case CardType.arms:
                return ArmsColor;
            case CardType.legs:
                return LegsColor;
            default:
                return Color.white;
        }
    }

    void DisplayCards() //display cards in the card slots of ui
    {
        int unlockNum = mechDeckList.Count; //grab how many cards are in inventory
        Transform tempTransform;
        //GameObject temp;
        //Image tempimage;
        //Color tempcolor;

        for (int i = 0; i < unlockNum; i++) //for each card unlocked, set it to game object and set the card slot to active
        {
            cardName = mechDeckList[i]; //set the text on the card to be the card name

            //SET COLOR
            tempTransform = UICardObjectList[i].GetComponent<Transform>(); //grab transform of card slot
            temp = tempTransform.gameObject;
            tempimage = temp.GetComponent<Image>();
            tempcolor = GetColor(cardName);
            tempimage.color = tempcolor;

            //SET NAME
            temp = tempTransform.GetChild(0).gameObject; //grab the child of card slot
            cardNameText = temp.GetComponent<TextMeshProUGUI>();
            cardNameText.text = cardName;

            //SET ACTIVE
            UICardObjectList[i].SetActive(true); //display the card
        }
    }

    public void SelectCard() //when a card is pressed, itll be selected THIS IS WHAT CARD SLOTS USE
    {
        if (UICardObjectList.Contains(lastButton)) //if last button was a card slot
        {
            DeSelectCard(lastButton); //deselect last button
        }


        //grab information
        Transform tempTransform;
        tempTransform = currentButton.GetComponent<Transform>();
        temp = tempTransform.GetChild(0).gameObject;
        cardNameText = temp.GetComponent<TextMeshProUGUI>(); 

        cardName = cardNameText.text; //get card name

        tempimage = currentButton.GetComponent<Image>();
        float alpha;
        tempcolor = tempimage.color;
        alpha = .75f;
        tempcolor.a = alpha;
        tempimage.color = tempcolor; //make the card less visible if selected 
        Debug.Log(currentButton.name);
    }

    void DeSelectCard(GameObject card) //removes visual to show card was de-selected by clicking another item that doesnt work
    {
        temp = card;
        tempimage = card.GetComponent<Image>();
        tempcolor = tempimage.color;
        tempcolor.a = 1f;
        tempimage.color = tempcolor;
    }

    public void SelectMechSlot() //when a slot is pressed, itll be compared to last button press to see if the card can go there THIS IS WHAT MECH SLOTS USE
    {
        //GET MECH TYPE
        if (currentButton == upHeadButton) { SlotType = CardType.head; }
        if (currentButton == upArmsButton || currentButton == downArmsButton) { SlotType = CardType.arms; }
        if (currentButton == downLegsButton) { SlotType = CardType.legs; }

        if(UICardObjectList.Contains(lastButton)) //if last button pressed was a card slot, check info
        {
            temp = lastButton.transform.GetChild(0).gameObject;
            cardNameText = temp.GetComponent<TextMeshProUGUI>();
            cardName = cardNameText.text;

            index = fullmechDeckList.IndexOf(cardName);
            type = mechCardType[index];

            if(type == SlotType) //if types are correct
            {
                AddCard(cardName);
            }
            else //else reset button to look un-selected
            {
                DeSelectCard(lastButton);
            }
        }
        else //else reset button to look un-selected
        {
            DeSelectCard(lastButton);
        }
    }

    public void AddCard(string card) //add card to mech
    {
        //add the card if it works
        switch(SlotType)
        {
            case CardType.head:
                if (HeadCardSlot == "") //if nothing saved in card slot
                {
                    HeadCardSlot = card;
                    lastButton.SetActive(false);

                    UpdateButtonText(upHeadButton, card);

                    DetermineValue(card, SlotType);
                    UpdateInventoryData();
                }
                else
                {
                    index = mechDeckList.IndexOf(HeadCardSlot);
                    UICardObjectList[index].SetActive(true);

                    HeadCardSlot = card;
                    lastButton.SetActive(false);

                    UpdateButtonText(upHeadButton, card);

                    DetermineValue(card, SlotType);
                    UpdateInventoryData();
                }
                break;
            case CardType.arms:
                if (ArmsCardSlot == "") //if nothing saved in card slot
                {
                    ArmsCardSlot = card;
                    lastButton.SetActive(false);

                    UpdateButtonText(upArmsButton, card);
                    UpdateButtonText(downArmsButton, card);

                    DetermineValue(card, SlotType);
                    UpdateInventoryData();
                }
                else
                {
                    index = mechDeckList.IndexOf(ArmsCardSlot);
                    UICardObjectList[index].SetActive(true);

                    ArmsCardSlot = card;
                    lastButton.SetActive(false);

                    UpdateButtonText(upArmsButton, card);
                    UpdateButtonText(downArmsButton, card);

                    DetermineValue(card, SlotType);
                    UpdateInventoryData();
                }
                break;
            case CardType.legs:
                if (LegsCardSlot == "") //if nothing saved in card slot
                {
                    LegsCardSlot = card;
                    lastButton.SetActive(false);

                    UpdateButtonText(downLegsButton, card);

                    DetermineValue(card, SlotType);
                    UpdateInventoryData();
                }
                else
                {
                    index = mechDeckList.IndexOf(LegsCardSlot);
                    UICardObjectList[index].SetActive(true);

                    LegsCardSlot = card;
                    lastButton.SetActive(false);

                    UpdateButtonText(downLegsButton, card);

                    DetermineValue(card, SlotType);
                    UpdateInventoryData();
                }
                break;
            default:
                break;
        }
    }

    void UpdateButtonText(GameObject button, string text) //update a button's text so it shows visually what is changed
    {
        GameObject textObject = button.transform.GetChild(0).gameObject;
        TextMeshProUGUI textMesh = textObject.GetComponent<TextMeshProUGUI>();
        textMesh.text = text;
    }

    private IEnumerator WaitForUpdate() //waits for X time to record last button press so it doesnt instantly update
    {
        yield return new WaitForSeconds(1.5f);

        currentButton = EventSystem.current.currentSelectedGameObject;
    }

    void DetermineValue(string card, CardType slot) //update data to send to inventory based on card name, index
    {
        index = fullmechDeckList.IndexOf(card);
        int cardValue = cardEffectValueList[index];
        if(cardEffectList[index] != "") { cardEffect = cardEffectList[index]; } //if card effect is found for that card, save it

            switch (slot)
            {
                case CardType.head:
                    HeadCardSlotValue = cardValue;
                    if (cardEffect != "") { specialEffect1 = cardEffect; }
                break;
                case CardType.arms:
                    ArmsCardSlotValue = cardValue;
                    if (cardEffect != "") { specialEffect2 = cardEffect; }
                    break;
                case CardType.legs:
                    LegsCardSlotValue = cardValue;
                    if (cardEffect != "") { specialEffect3 = cardEffect; }
                    break;
                default:
                    break;
            }
    }

    private string UpdateTextData(string card)
    {
        if (card.Contains("Health")) { cardChange = "health"; }
        else if (card.Contains("Energy")) { cardChange = "energy"; }
        else if (card.Contains("Damage")) { cardChange = "damage"; }

        return cardChange;
    }

    private void OpenTextData(string card, int slot)
    {
        if (card.Contains("health"))
        {
            if (slot == 0) { HeadCardSlot = "HeadHealth"; }
            else if (slot == 1) { ArmsCardSlot = "ArmsHealth"; }
            else if (slot == 2) { LegsCardSlot = "LegsHealth"; }
            else { Debug.Log("Error no Data."); }
        }
        else if (card.Contains("energy"))
        {
            if (slot == 0) { HeadCardSlot = "HeadEnergy"; }
            else if (slot == 1) { ArmsCardSlot = "ArmsEnergy"; }
            else if (slot == 2) { LegsCardSlot = "LegsEnergy"; }
            else { Debug.Log("Error no Data."); }
        }
        else if (card.Contains("damage")) 
        {
            if (slot == 0) { HeadCardSlot = "HeadDamage"; }
            else if (slot == 1) { ArmsCardSlot = "ArmsDamage"; }
            else if (slot == 2) { LegsCardSlot = "LegsDamage"; }
            else { Debug.Log("Error no Data."); }
        }
    }

    void UpdateInventoryData() //update data sent to inventory
    {
        inventory.HeadCardSlot = UpdateTextData(HeadCardSlot); //string of card in each slot
        inventory.ArmsCardSlot = UpdateTextData(ArmsCardSlot);
        inventory.LegsCardSlot = UpdateTextData(LegsCardSlot);

        cardEffectsActive = new List<string> { specialEffect1, specialEffect2, specialEffect3};
        inventory.cardEffectsActiveList = cardEffectsActive; //list of active effects from the cards in the mech upgrade script, to send to inventory

        inventory.HeadCardSlotValue = HeadCardSlotValue; //int of value in each slot
        inventory.ArmsCardSlotValue = ArmsCardSlotValue;
        inventory.LegsCardSlotValue = LegsCardSlotValue;
    }

    void GetInventoryData() //upon returning to scene, initialize data
    {
        //Get Special Effects
        if (inventory.cardEffectsActiveList[0] != null) { specialEffect1 = inventory.cardEffectsActiveList[0]; }
        if (inventory.cardEffectsActiveList[1] != null) { specialEffect2 = inventory.cardEffectsActiveList[1]; }
        if (inventory.cardEffectsActiveList[2] != null) { specialEffect3 = inventory.cardEffectsActiveList[2]; }
        cardEffectsActive = new List<string> { specialEffect1, specialEffect2, specialEffect3 };


        HeadCardSlotValue = inventory.HeadCardSlotValue; //Update Slot int Value
        ArmsCardSlotValue = inventory.ArmsCardSlotValue;
        LegsCardSlotValue = inventory.LegsCardSlotValue;

        //Update Card Slot + Button Text
        if (inventory.HeadCardSlot != "") { OpenTextData(inventory.HeadCardSlot, 0); UpdateButtonText(upHeadButton, HeadCardSlot); } //Update button text
        if (inventory.ArmsCardSlot != "")
        {
            OpenTextData(inventory.ArmsCardSlot, 1);
            UpdateButtonText(upArmsButton, ArmsCardSlot);
            UpdateButtonText(downArmsButton, ArmsCardSlot);
        }
        if (inventory.LegsCardSlot != "") { OpenTextData(inventory.LegsCardSlot, 2); UpdateButtonText(downLegsButton, LegsCardSlot); }

        //Hide Mech cards in use
        if (fullmechDeckList.Contains(HeadCardSlot)) { index = fullmechDeckList.IndexOf(HeadCardSlot); temp = UICardObjectList[index]; temp.SetActive(false); } 
        if (fullmechDeckList.Contains(ArmsCardSlot)) { index = fullmechDeckList.IndexOf(ArmsCardSlot); temp = UICardObjectList[index]; temp.SetActive(false); }
        if (fullmechDeckList.Contains(LegsCardSlot)) { index = fullmechDeckList.IndexOf(LegsCardSlot); temp = UICardObjectList[index]; temp.SetActive(false); }
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
