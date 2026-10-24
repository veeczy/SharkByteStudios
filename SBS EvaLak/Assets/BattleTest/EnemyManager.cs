using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private CardZone enemyDeck;
    [SerializeField] private CardZone enemyHand;
    [SerializeField] private CardZone enemyPlayArea;
    [SerializeField] private CardZone enemyGraveyard;
    [SerializeField] private int handSize = 3;
    [SerializeField] private float playDelay = 1f;
    [SerializeField] private bool recycleCardsToDeck = true; // send played cards back to deck, so enemy can't run out of cards

    void Start()
    {
        enemyDeck = GameObject.FindWithTag("EnemyPrefab").GetComponent<CardZone>();
        RefillHand();
    }

    public void Attack()
    {
        StartCoroutine(AttackRoutine());
    }

    public IEnumerator AttackRoutine()
    {
        // Index 0 is the leftmost card
        List<Card> cards = enemyHand.GetAllCards();

        if (cards.Count > 0)
        {
            Card card = cards[0];
            enemyPlayArea.Add(card);

            // Wait for the card to arrive in the play area
            yield return new WaitForSeconds(playDelay);

            card.OnPlay();

            if (recycleCardsToDeck)
                enemyDeck.Add(card);
            else
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