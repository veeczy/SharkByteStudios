using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private CardZone enemyDeck;
    [SerializeField] private CardZone enemyHand;
    [SerializeField] private CardZone enemyGraveyard;
    [SerializeField] private int handSize = 3;

    void Start()
    {
        RefillHand();
    }

    public void Attack()
    {
        // Index 0 is the leftmost card
        List<Card> cards = enemyHand.GetAllCards();

        if (cards.Count > 0)
        {
            Card card = cards[0];
            card.OnPlay();
            enemyGraveyard.Add(card);
        }

        RefillHand();
    }

    private void RefillHand()
    {
        while (enemyHand.Count < handSize && enemyDeck.Count > 0)
            enemyHand.Add(enemyDeck.RandomCard());
    }
}