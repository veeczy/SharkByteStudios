using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private GameObject deck;
    [SerializeField] private GameObject hand;
    [SerializeField] private int drawCount = 5;
    [SerializeField] private float cardSpacing = 1.5f;
    [SerializeField] private float depthSpacing = 0.01f;

    private readonly List<GameObject> deckCards = new List<GameObject>();
    private readonly List<GameObject> handCards = new List<GameObject>();

    private void Awake()
    {
        // Add cards in deck to private card list and disable them
        foreach (Transform child in deck.transform)
        {
            deckCards.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }

    public void DrawCards()
    {
        int count = Mathf.Min(drawCount, deckCards.Count);

        // Select random cards from deck when drawing, remove from deck list and activate them
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, deckCards.Count);
            GameObject card = deckCards[index];
            deckCards.RemoveAt(index);

            card.SetActive(true);
            handCards.Add(card);
        }

        // Move drawn cards to hand
        ArrangeHand();
    }

    private void ArrangeHand()
    {
        Transform h = hand.transform;
        int n = handCards.Count;

        for (int i = 0; i < n; i++)
        {
            float offset = (i - (n - 1) * 0.5f) * cardSpacing;
            Transform card = handCards[i].transform;

            card.position = h.position + h.right * offset - h.forward * (i * depthSpacing);
            card.rotation = h.rotation;
        }
    }
}