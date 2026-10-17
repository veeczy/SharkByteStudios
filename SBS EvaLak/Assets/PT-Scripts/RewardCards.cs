using System.Collections.Generic;
using UnityEngine;

public class RewardCards : MonoBehaviour
{
    public List<string> unlockedDeckList;
    public List<string> fullDeckList;
    public Inventory inventory;
    public GameObject DDOL;

    public string cardName;
    public int cardID;
    public bool reward = false;

    public string cardReward1;
    public string cardReward2;
    public string cardReward3;
    public int rewardNumber = 3;

    public GameObject cardRewardCenter;
    public GameObject cardRewardRight;
    public GameObject cardRewardLeft;

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

        cardRewardLeft = GameObject.Find("CardReward-Left");
        cardRewardCenter = GameObject.Find("CardReward-Center");
        cardRewardRight = GameObject.Find("CardReward-Right");
    }

    public void PickCard()
    {
        if (unlockedDeckList.Count < fullDeckList.Count) //if you havent unlocked all the cards
        {
            if(fullDeckList.Count - unlockedDeckList.Count < 3) { rewardNumber = fullDeckList.Count - unlockedDeckList.Count; } //if less then 3 rewards possible, only reward however many is left
            
            for (int i = 0; i < rewardNumber; i++) //for loop to make sure it happens 3 times
            {
                while (!reward)
                {
                    cardID = Random.Range(0, fullDeckList.Count); //card index 
                    cardName = fullDeckList[cardID];
                    if (!unlockedDeckList.Contains(cardName))
                    {
                        unlockedDeckList.Add(cardName);
                        if(i == 0) { cardReward1 = cardName; }
                        else if(i == 1) { cardReward2 = cardName; }
                        else if (i == 2) { cardReward3 = cardName; }
                        else { break; } //if somehow you do more than 3 iterations, break out
                        reward = true; //break out of while loop if success
                    }
                }
                reward = false; //reset reward being given for next loop
            }
        }
    }

    public void RewardDuplicates()
    {

    }

    public void DisplayCards()
    {
        foreach (var card in fullDeckList)
        {
            for(int i = 0; i < rewardNumber; i++)
            {
                if (card == cardReward1 || card == cardReward2 || card == cardReward3)
                { 
                    temp = GameObject.Find(card);
                    if (i == 0) { temp.transform.position = cardRewardCenter.transform.position; }
                    else if (i == 1) { temp.transform.position = cardRewardLeft.transform.position; }
                    else if (i == 2) { temp.transform.position = cardRewardRight.transform.position; }
                    else { break; } 
                }
            }
            
        } 
    }
}
