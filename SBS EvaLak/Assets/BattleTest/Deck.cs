using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deck : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private GameObject deck;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject graveyard;
    [SerializeField] private TextMeshPro remainingCardsText;
    [SerializeField] private float cardSpacing = 1.5f;
    [SerializeField] private float depthSpacing = 0.01f;
    [SerializeField] private float moveSpeed = 10f;

    private List<GameObject> deckCards = new List<GameObject>();
    private List<GameObject> handCards = new List<GameObject>();

    private void Awake()
    {
        foreach (Transform child in deck.transform)
        {
            deckCards.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        AnimateHand();
    }

    public void DrawCards(int count)
    {
        count = Mathf.Min(count, deckCards.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, deckCards.Count);
            GameObject card = deckCards[index];
            deckCards.RemoveAt(index);

            // Card starts at deck position and moves gradually to hand position
            card.transform.position = deck.transform.position;
            card.transform.rotation = deck.transform.rotation;
            card.SetActive(true);
            handCards.Add(card);
        }

        remainingCardsText.text = deckCards.Count.ToString();
    }

    // When a card is dropped, moves it to the hand slot nearest its position
    public void ReorderCard(GameObject card)
    {
        if (!handCards.Remove(card)) return;

        // Horizontal distance from the hands center
        float x = Vector3.Dot(card.transform.position - hand.transform.position, hand.transform.right);
        int index = Mathf.RoundToInt(x / cardSpacing + handCards.Count * 0.5f);
        index = Mathf.Clamp(index, 0, handCards.Count);

        handCards.Insert(index, card);
    }

    public void Discard()
    {
        // Loop backwards to avoid skipping items when they are removed from list
        for (int i = handCards.Count - 1; i >= 0; i--)
        {
            GameObject cardObject = handCards[i];
            Card card = cardObject.GetComponent<Card>();

            if (!card.selected) continue;

            card.ToggleSelected(); // clear selection
            handCards.RemoveAt(i);

            cardObject.SetActive(false);
            cardObject.transform.SetParent(graveyard.transform);
            cardObject.transform.localPosition = Vector3.zero;
            cardObject.transform.localRotation = Quaternion.identity;
        }

        // Draw cards until the hand size is reached
        DrawCards(deckManager.handSize - handCards.Count);
    }

    public void Play()
    {
        // Play card logic goes here eventually
    }

    // Moves cards gradually towards their slot on the hand
    private void AnimateHand()
    {
        Transform h = hand.transform;
        int n = handCards.Count;

        float t = 1f - Mathf.Exp(-moveSpeed * Time.deltaTime);

        for (int i = 0; i < n; i++)
        {
            GameObject cardObject = handCards[i];

            // Check if card is being dragged by the player, if so do not move it towards the hand
            if (cardObject.GetComponent<CardDrag>().IsDragging) continue;

            float offset = (i - (n - 1) * 0.5f) * cardSpacing;
            Vector3 targetPosition = h.position + h.right * offset - h.forward * (i * depthSpacing);

            Transform card = cardObject.transform;
            card.position = Vector3.Lerp(card.position, targetPosition, t);
            card.rotation = Quaternion.Slerp(card.rotation, h.rotation, t);
        }
    }
}