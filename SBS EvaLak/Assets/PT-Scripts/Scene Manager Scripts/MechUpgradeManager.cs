using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MechUpgradeManager : MonoBehaviour
{
    [Header("Init")]
    private Inventory inventory;
    private GameObject DDOL;
    private CameraControl cameraControl;
    private GameObject mainCamera;

    [Header("Card Information")]
    [SerializeField] private string cardName;
    [SerializeField] private List<string> mechDeckList = new List<string>(); //list of mech cards unlocked by player
    [SerializeField] private List<string> fullmechDeckList = new List<string>(); //full mech deck, stored in inventory so rewards can access it

    public enum CardType { head, arms, legs }
    public CardType type;
    [SerializeField] private List<CardType> mechCardType = new List<CardType>(); //list of all locations for full mech deck

    [SerializeField] private List<string> cardEffectList = new List<string>(); //list of all possible effects for full mech deck

    public List<string> cardEffectsActive = new List<string>(); //list of active effects from the cards in the mech upgrade script, to send to inventory


    [Header("UI Setup")]
    public List<GameObject> UICardObjectList;
    [SerializeField] private GameObject SortCards;
    //data stored for button press
    [SerializeField] private string HeadCardSlot;
    [SerializeField] private string ArmsCardSlot;
    [SerializeField] private string LegsCardSlot;

    [SerializeField] private Color HeadColor;
    [SerializeField] private Color ArmsColor;
    [SerializeField] private Color LegsColor;

    [SerializeField] private Color tempcolor;
    [SerializeField] private Image tempimage;
    [SerializeField] private GameObject temp;
    [SerializeField] private int index;
    private CardType SlotType;

    //buttons from Camera Control
    [SerializeField] private GameObject upHeadButton;
    [SerializeField] private GameObject upArmsButton;
    [SerializeField] private GameObject downArmsButton;
    [SerializeField] private GameObject downLegsButton;

    private TextMeshProUGUI cardNameText;
    [SerializeField]  private GameObject currentButton;
    [SerializeField]  private GameObject lastButton;

    private void Start()
    {
        if (DDOL == null) { DDOL = GameObject.Find("DDOL"); inventory = DDOL.GetComponent<Inventory>(); }
        if (inventory != null) { mechDeckList = inventory.mechDeckList; fullmechDeckList = inventory.fullmechDeckList; }
        if (mainCamera == null) { mainCamera = GameObject.Find("Main Camera"); cameraControl = mainCamera.GetComponent<CameraControl>(); }
        if (cameraControl != null) { upHeadButton = cameraControl.upHeadButton; upArmsButton = cameraControl.upArmsButton; downArmsButton = cameraControl.downArmsButton; downLegsButton = cameraControl.downLegsButton; }
        
        if (SortCards == null) { SortCards = GameObject.Find("SortCards"); }


        GetCards();
        DisplayCards();
    }

    private void Update()
    {
        currentButton = EventSystem.current.currentSelectedGameObject;
    }

    void GetCards() //get all card slots from ui grid
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

    Color GetColor(string card) 
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

    public void SelectCard() //when a card is pressed, itll be selected
    {
        lastButton = currentButton;
        currentButton = EventSystem.current.currentSelectedGameObject;

        //grab information
        Transform tempTransform;
        tempTransform = currentButton.GetComponent<Transform>();
        temp = tempTransform.GetChild(0).gameObject;
        cardNameText = temp.GetComponent<TextMeshProUGUI>(); 

        cardName = cardNameText.text; //get card name

        tempimage = currentButton.GetComponent<Image>();
        float alpha = tempimage.color.a;
        tempcolor = tempimage.color;
        alpha = .75f;
        tempcolor.a = alpha;
        tempimage.color = tempcolor; //make the card less visible if selected 
        Debug.Log(currentButton.name);
    }

    void DeSelectCard(GameObject card)
    {
        temp = card;
        tempimage = card.GetComponent<Image>();
        tempcolor = tempimage.color;
        tempcolor.a = 1f;
        tempimage.color = tempcolor;
    }

    public void SelectMechSlot() //when a slot is pressed, itll be compared to last button press to see if the card can go there
    {
        lastButton = currentButton;
        currentButton = EventSystem.current.currentSelectedGameObject;
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

    public void AddCard(string card)
    {
        //add the card if it works
    }
}
