using System.Collections.Generic;
using UnityEngine;

public class RewardCards : MonoBehaviour
{
    public List<string> unlockedDeckList;
    public List<string> fullDeckList;
    public List<string> duplicateList;
    public Inventory inventory;
    public GameObject DDOL;

    public string cardName;
    public int cardID;
    public bool newreward = false;

    public string cardReward1;
    public string cardReward2;
    public string cardReward3;
    public int rewardNumber = 3;

    public GameObject cardRewardCenter;
    public GameObject cardRewardRight;
    public GameObject cardRewardLeft;

    public GameObject new1; //ui to show if card reward is brand new
    public GameObject new2;
    public GameObject new3; 

    public List<GameObject> cardGameObjectList;
    public GameObject temp;

    void Start()
    {
        Initialize(); //initialize the variables
        PickCard();
        DisplayCards();
    }

    public void Initialize()
    {
        DDOL = GameObject.Find("DDOL");
        inventory = DDOL.GetComponent<Inventory>();
        unlockedDeckList = inventory.unlockedDeckList;
        fullDeckList = inventory.fullDeckList;
        cardGameObjectList = inventory.cardGameObjectList;
        duplicateList = inventory.duplicateList;

        cardRewardLeft = GameObject.Find("CardReward-Left");
        cardRewardCenter = GameObject.Find("CardReward-Center");
        cardRewardRight = GameObject.Find("CardReward-Right");

        new1 = GameObject.Find("new1");
        new2 = GameObject.Find("new2");
        new3 = GameObject.Find("new3");
        new1.SetActive(false);
        new2.SetActive(false);
        new3.SetActive(false);
    }

    public void PickCard()
    {
        if (unlockedDeckList.Count < fullDeckList.Count) //if you havent unlocked all the cards
        {
            if(fullDeckList.Count - unlockedDeckList.Count < 3) { rewardNumber = fullDeckList.Count - unlockedDeckList.Count; } //if less then 3 rewards possible, only reward however many is left
            
            for (int i = 0; i < rewardNumber; i++) //for loop to make sure it happens 3 times
            {
                cardID = Random.Range(0, fullDeckList.Count); //card index 
                cardName = fullDeckList[cardID];

                if (unlockedDeckList.Contains(cardName)) { duplicateList.Add(cardName); }
                else if (!unlockedDeckList.Contains(cardName)) 
                {
                    unlockedDeckList.Add(cardName);
                    newreward = true;
                }

                if (i == 0) { cardReward1 = cardName; if (newreward) { new2.SetActive(true); } }
                else if (i == 1) { cardReward2 = cardName; if (newreward) { new1.SetActive(true); } }
                else if (i == 2) { cardReward3 = cardName; if (newreward) { new3.SetActive(true); } }
                else { break; } //if somehow you do more than 3 iterations, break out
                newreward = false;
            }
        }

        inventory.UpdateLockedCards();
    }

    public void DisplayCards()
    {
        foreach (var card in fullDeckList)
        {
            temp = GameObject.Find(card);
            if (card == cardReward1) { temp.transform.position = cardRewardCenter.transform.position; }
            else if (card == cardReward2) { temp.transform.position = cardRewardLeft.transform.position; }
            else if (card == cardReward3) { temp.transform.position = cardRewardRight.transform.position; }
        }

        //if duplicates, make duplicate to display in empty spot (THIS WILL BE FIXED WHEN ITS RENDERS AND NOT game objects
        if(cardReward1 == cardReward2 || cardReward1 == cardReward3)
        {
            temp = GameObject.Find(cardReward1);
            Instantiate(temp, cardRewardCenter.transform);
        }

        if(cardReward2 == cardReward1 || cardReward2 == cardReward3)
        {
            temp = GameObject.Find(cardReward1);
            Instantiate(temp, cardRewardLeft.transform);
        }

        if (cardReward3 == cardReward1 || cardReward3 == cardReward2)
        {
            temp = GameObject.Find(cardReward1);
            Instantiate(temp, cardRewardRight.transform);
        }

    }
}
