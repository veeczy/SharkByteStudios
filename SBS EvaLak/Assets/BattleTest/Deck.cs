using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private GameObject deck;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject graveyard;
    [SerializeField] private float cardSpacing = 1.5f;
    [SerializeField] private float depthSpacing = 0.01f;

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

    public void DrawCards(int count)
    {
        count = Mathf.Min(count, deckCards.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, deckCards.Count);
            GameObject card = deckCards[index];
            deckCards.RemoveAt(index);

            card.SetActive(true);
            handCards.Add(card);
        }

        ArrangeHand();
    }

    // Called when a card is dropped, moves it to the hand slot nearest its position
    public void ReorderCard(GameObject card)
    {
        if (!handCards.Remove(card)) return;

        // Horizontal distance from the hand's center, measured in slots
        float x = Vector3.Dot(card.transform.position - hand.transform.position, hand.transform.right);
        int index = Mathf.RoundToInt(x / cardSpacing + handCards.Count * 0.5f);
        index = Mathf.Clamp(index, 0, handCards.Count);

        handCards.Insert(index, card);
        ArrangeHand();
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

        ArrangeHand();
    }

    public void Play()
    {
        // Play card logic goes here eventually
    }

    private void ArrangeHand()
    {
        Transform h = hand.transform;
        int n = handCards.Count;

        // Arrange cards horizontally centered on the hand gameobject transform
        for (int i = 0; i < n; i++)
        {
            float offset = (i - (n - 1) * 0.5f) * cardSpacing;
            Transform card = handCards[i].transform;

            card.position = h.position + h.right * offset - h.forward * (i * depthSpacing);
            card.rotation = h.rotation;
        }
    }
}