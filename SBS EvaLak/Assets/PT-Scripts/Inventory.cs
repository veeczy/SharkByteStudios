using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class Inventory : MonoBehaviour
{
    public List<string> fullDeckList; //this is all the possible cards
    public List<string> deckList; //this is what the player owns
    public List<GameObject> cardGameObjectList; //to put the prefabs from the scene into when the cards are matching

    private GameObject tempGameObject;

    public bool cardScene = false;
    private List<string> sceneNameList = new List<string>() { "PT_Deck_Build", "BattleSceneTest" };
    public string sceneName;
    public int deckSize = 25; //at some point other scripts will update this

    private static System.Random rng = new System.Random();

    private GameObject numCardsUI;
    private NumCardsInDeck numCardsInDeck;

    public int numMult = 0;
    public int numDamage = 0;
    public int numCombo = 0;
    public int numRepair = 0;

    private void Start()
    {

        //random population
        //while (deckList.Count < deckLength)
        //{
        //    int randomIndex = Random.Range(0, fullDeckList.Count);
        //    string tempElement = fullDeckList[randomIndex];

        //    if (!deckList.Contains(tempElement)) { deckList.Add(tempElement); }
        //}
    }

    public void OnLevelWasLoaded() //at start of new scene loading
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneNameList.Contains(sceneName)) { cardScene = true; }
        else { cardScene = false; }


        if (cardScene) //if scene with cards in it
        {
            //THIS WILL BREAK IF YOU ENTER THE BATTLE FIRST (i think) WE NEED TO MAKE THIS ONLY LOAD ON DECKBUILDER SCENE
            numCardsUI = GameObject.Find("NumCardsUI");
            numCardsInDeck = numCardsUI.GetComponent<NumCardsInDeck>();


            //check to see if deck game objects are in the scene and if they are append them to the list
            foreach (string name in deckList)
            {
                tempGameObject = GameObject.Find(name); //see if card with name is in scene
                if (tempGameObject != null) { cardGameObjectList.Add(tempGameObject); } // if so add to game object list
                else { Debug.Log("Could not find " + name); } // if not then break to next one
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
}
