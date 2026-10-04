using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deck : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private PlayModifiers mod;
    [SerializeField] private Enemy enemy;
    [SerializeField] private CardZone drawPile;
    [SerializeField] private CardZone hand;
    [SerializeField] private CardZone playArea;
    [SerializeField] private CardZone graveyard;
    [SerializeField] private TextMeshPro remainingCardsText;
    [SerializeField] private float playDelay = 1f;
    [SerializeField] private float cardPlayInterval = 0.5f;

    public bool isPlaying;

    public void DrawCards(int count)
    {
        // Cards start at the draw pile and move gradually to their hand position
        for (int i = 0; i < count && drawPile.Count > 0; i++)
            hand.Add(drawPile.RandomCard());

        remainingCardsText.text = drawPile.Count.ToString();
    }

    public void Discard()
    {
        foreach (Card card in hand.GetSelected())
            graveyard.Add(card);

        RefillHand();
    }

    public void Play()
    {
        if (!isPlaying) StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        // Selected cards move to the play area, keeping their left to right order
        List<Card> playedCards = hand.GetSelected();
        if (playedCards.Count == 0) yield break;

        isPlaying = true;
        foreach (Card card in playedCards)
            playArea.Add(card);

        // Wait for the cards to arrive in the play area
        yield return new WaitForSeconds(playDelay);

        // Play cards from left to right with delay in between each card, each card is sent to the graveyard after
        foreach (Card card in playedCards)
        {
            card.OnPlay();
            graveyard.Add(card);
            yield return new WaitForSeconds(cardPlayInterval);
        }

        mod.EndHand(); // tell mod script that the hand has stopped playing

        enemy.Attack(); // Actually do the enemy attack turn after the player turn

        RefillHand();
        isPlaying = false;
    }

    // Draw cards until the hand size is reached
    private void RefillHand()
    {
        DrawCards(deckManager.handSize - hand.Count);
    }
}