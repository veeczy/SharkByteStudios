using System.Collections.Generic;
using UnityEngine;

public class ScrapCards : MonoBehaviour
{
    public List<string> fullDeckList;
    public List<string> duplicateList;
    public List<int> scrapValueList;
    public Inventory inventory;
    public GameObject DDOL;

    public int index;
    public int scrapValue;
    public int totalScrap;

    public GameObject numCards;
    public NumCardsInDeck numCardsInDeck;
    public int scrapValueText;
    public int duplicatesText;
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        DDOL = GameObject.Find("DDOL");
        inventory = DDOL.GetComponent<Inventory>();

        fullDeckList = inventory.fullDeckList;
        duplicateList = inventory.duplicateList;
        scrapValueList = inventory.scrapValueList;

        totalScrap = inventory.totalScrap;

        numCards = GameObject.Find("NumCardsUI");
        numCardsInDeck = numCards.GetComponent<NumCardsInDeck>();
        scrapValueText = numCardsInDeck.ScrapValue;
        duplicatesText = numCardsInDeck.Duplicates;
    }

    public void ScrapCard(string card) //for if we want buttons that individually remove specific cards, or also is called by scrap all cards to remove them all
    {
        if (fullDeckList.Contains(card)) //check to see if string is real card
        {
            index = fullDeckList.IndexOf(card); //get index of card
            scrapValue = scrapValueList[index]; //get sv of card
            totalScrap = (totalScrap + scrapValue); //add scrap value to player's scrap
            scrapValueText = totalScrap;
            duplicatesText = duplicateList.Count;
        }
        //duplicateList.Remove(card); //remove card from list
    }

    public void ScrapAllCards() //for button to remove all dupe cards at once to turn to scrap
    {
        if (inventory.duplicateList.Count > 0)
        {
            foreach (var card in inventory.duplicateList)
            {
                ScrapCard(card);
            }
        }
        duplicateList = new List<string>(); //empty list afterwards
        UpdateData();
    }

    private void UpdateData()
    {
        inventory.totalScrap = totalScrap;
        inventory.duplicateList = duplicateList;
        inventory.duplicateCards = duplicateList.Count;
        numCardsInDeck.ScrapValue = totalScrap;
    }
}
