using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    [Header("Deck/Card Information")]
    public List<string> fullDeckList; //this is all the possible cards
    public List<string> deckList; //this is what the player owns
    public List<GameObject> cardGameObjectList; //to put the prefabs from the scene into when the cards are matching

    [Header("Init Action Cards")] //FOR STANDARD ACTION CARDS
    public List<bool> checkLockList; //bool list at index of card in fulldecklist to prove if card is unlocked or not
    public List<string> unlockedDeckList; //strings of cards that have been unlocked are added here

    public List<string> duplicateList;
    public int duplicateCards = 0;
    public List<int> scrapValueList; //scrap value should correlate with the card at the same index in fulldecklist

    [Header("Init Component Cards")] //FOR COMPONENT CARDS
    public List<string> fullmechDeckList; //this is all the possible cards for mech 
    public List<string> mechDeckList; //this is what the player owns for mech
    public List<string> cardEffectList; //list of all possible effects for full mech deck
    public List<int> cardEffectValueList; //these are the values for the effects of each card, so if damage is 5 then that should be 5 at the index for that card here

    [Header("Data")]
    public bool cardScene = false;
    private GameObject tempGameObject;

    private List<string> sceneNameList = new List<string>() { "PT_Deck_Build", "BattleSceneTest", "WinScene" };
    public string sceneName;
    public int deckSize = 25; //at some point other scripts will update this
    public int totalScrap; //how much total scrap does the player own

    private static System.Random rng = new System.Random();

    [Header("Mech Data")]
    public string HeadCardSlot; //string of the effect they change
    public string ArmsCardSlot;
    public string LegsCardSlot;

    public List<string> cardEffectsActiveList; //list of active effects from the cards in the mech upgrade script, to send to inventory

    public int HeadCardSlotValue; //int of the value of the effect
    public int ArmsCardSlotValue;
    public int LegsCardSlotValue;

    [Header("UI Data")]
    private GameObject numCardsUI;
    private NumCardsInDeck numCardsInDeck;

    [Header("BATTLE Data")]
    public string planetInfo; //what planet is the battle taking place in
    //DATA for battle
    public int numMult = 0;
    public int numDamage = 0;
    public int numCombo = 0;
    public int numRepair = 0;

    
    private void Start()
    {
        checkLockList = new List<bool>(new bool[fullDeckList.Count]);
        UpdateLockedCards();
        //random population
        //while (deckList.Count < deckLength)
        //{
        //    int randomIndex = Random.Range(0, fullDeckList.Count);
        //    string tempElement = fullDeckList[randomIndex];

        //    if (!deckList.Contains(tempElement)) { deckList.Add(tempElement); }
        //}
    }

    public void UpdateLockedCards()
    {
        int tempIndex = 0;
        foreach (var card in fullDeckList)
        {
            if (unlockedDeckList.Contains(card)) { tempIndex = fullDeckList.IndexOf(card); checkLockList[tempIndex] = true; }
            //else { Debug.Log(card + " is currently locked."); }
        }
    }
    public void OnLevelWasLoaded() //at start of new scene loading
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneNameList.Contains(sceneName)) { cardScene = true; }
        else { cardScene = false; }

        UpdateLockedCards();
        duplicateCards = duplicateList.Count;

        if (cardScene) //if scene with cards in it
        {
            //THIS WILL BREAK IF YOU ENTER THE BATTLE FIRST (i think) WE NEED TO MAKE THIS ONLY LOAD ON DECKBUILDER SCENE 
            // FIXED BELOW FOR YOU DREW! - Vee
            if (sceneName == sceneNameList[0]) // if it is in deckbuilder scene run this extra stuff
            {
                numCardsUI = GameObject.Find("NumCardsUI");
                numCardsInDeck = numCardsUI.GetComponent<NumCardsInDeck>();
            }

            if (sceneName == sceneNameList[1] && deckList.Count == 0) { SceneManager.LoadScene("HangarLevel"); } //if enter battle with empty deck, it sends you back to level select

            //check to see if deck game objects are in the scene and if they are append them to the list
            cardGameObjectList = new List<GameObject>(); //empty list from what it was filled with in old scenes
            foreach (string name in fullDeckList)
            {
                tempGameObject = GameObject.Find(name); //see if card with name is in scene
                if (tempGameObject != null) { cardGameObjectList.Add(tempGameObject); } // if so add to game object list
                else { Debug.Log("Could not find " + name + " in scene: " + sceneName); } // if not then break to next one
            }
        }
    }

    public void AddCardByType(string cardType)
    {
        if (deckList.Count >= deckSize)
        {
            //Debug.Log("full deck");
            return;
        }

        //find the next card that hasnt been added
        foreach (string cardName in fullDeckList)
        {
            //Debug.Log(cardName.StartsWith(cardType) + " | " + fullDeckList.Contains(cardName));

            //starts with takes the beginning of the name and compares it so we dont have to individually hard code the cards
            if (cardName.StartsWith(cardType) && fullDeckList.Contains(cardName) && !deckList.Contains(cardName))
            {
                deckList.Add(cardName);

                //Debug.Log("Added " + cardName);

                numCardsInDeck.AddCards(cardType);

                Shuffle(deckList);

                return;
            }
        }
    }

    public void RemoveCardByType(string cardType)
    {
        if (deckList.Count <= 0)
        {
            //Debug.Log("empty deck");
            return;
        }

        foreach (string cardName in fullDeckList)
        {
            if (cardName.StartsWith(cardType) && fullDeckList.Contains(cardName) && deckList.Contains(cardName))
            {
                deckList.Remove(cardName);
                Shuffle(deckList);
                numCardsInDeck.RemoveCards(cardType);
                return;
            }
        }
    }

    private void Shuffle<String>(List<String> shuffleDeck)
    {
        int n = shuffleDeck.Count;
        //fisher-yates shuffle, very common https://www.geeksforgeeks.org/dsa/shuffle-a-given-array-using-fisher-yates-shuffle-algorithm/ 
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            String temp = shuffleDeck[k];
            shuffleDeck[k] = shuffleDeck[n];
            shuffleDeck[n] = temp;
        }
    }
    public void AddScrap()
    {
        totalScrap += 4;
    }
    public void RemoveScrap(int totalScrapRemove)
    {
        totalScrap -= totalScrapRemove;
    }
}
