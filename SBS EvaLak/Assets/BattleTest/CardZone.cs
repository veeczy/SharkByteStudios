using System.Collections.Generic;
using UnityEngine;

// Put this on anywhere cards can be, like draw pile, hand, play area, graveyard
public class CardZone : MonoBehaviour
{
    [SerializeField] private float cardSpacing = 1.5f;
    [SerializeField] private float moveSpeed = 10f;

    // Piles stack their cards on this object and disable them once they arrive, so they are not visible and can't be selected. Currently used for deck and graveyard piles
    [SerializeField] private bool isPile;

    // Cards dropped in a reorderable zone move to the slot nearest them (currently hand only)
    [SerializeField] private bool reorderable;

    private const float pileArriveDistance = 0.05f;

    private List<Card> cards = new List<Card>();

    public int Count => cards.Count;

    private void Awake()
    {
        // Cards placed under this object start in this zone, piles hide them at their center
        foreach (Transform child in transform)
        {
            if (!child.TryGetComponent(out Card card)) continue;

            Add(card);

            if (!isPile) continue;
            card.transform.SetPositionAndRotation(transform.position, transform.rotation);
            card.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        int n = cards.Count;
        float t = 1f - Mathf.Exp(-moveSpeed * Time.deltaTime);

        for (int i = 0; i < n; i++)
        {
            Card card = cards[i];

            // Skip disabled cards (in a pile) and cards the player is dragging
            if (!card.gameObject.activeSelf) continue;
            if (card.TryGetComponent(out CardDrag drag) && drag.IsDragging) continue;

            Vector3 target = transform.position;
            if (!isPile) target += transform.right * ((i - (n - 1) * 0.5f) * cardSpacing);

            Transform cardTransform = card.transform;
            cardTransform.SetPositionAndRotation(
                Vector3.Lerp(cardTransform.position, target, t),
                Quaternion.Slerp(cardTransform.rotation, transform.rotation, t));

            // Easing never lands exactly on the target, so piles snap and disable cards once they are close enough
            if (isPile && Vector3.Distance(cardTransform.position, target) < pileArriveDistance)
            {
                cardTransform.SetPositionAndRotation(target, transform.rotation);
                card.gameObject.SetActive(false);
            }
        }
    }

    // Moves a card here from whichever zone it was in, it then moves gradually to its slot
    public void Add(Card card)
    {
        if (card.Zone != null) card.Zone.cards.Remove(card);
        card.Zone = this;
        cards.Add(card);

        if (isPile && card.selected) card.ToggleSelected(); // clear selection
        card.gameObject.SetActive(true);
    }

    public Card RandomCard()
    {
        return cards[Random.Range(0, cards.Count)];
    }

    // Selected cards in left to right order, this is a copy so cards can be moved out of the zone while looping
    public List<Card> GetSelected()
    {
        return cards.FindAll(card => card.selected);
    }

    // When a card is dropped, moves it to the slot nearest its position
    public void Reorder(Card card)
    {
        if (!reorderable || !cards.Remove(card)) return;

        // Horizontal distance from the zone's center, measured in slots
        float x = Vector3.Dot(card.transform.position - transform.position, transform.right);
        int index = Mathf.RoundToInt(x / cardSpacing + cards.Count * 0.5f);

        cards.Insert(Mathf.Clamp(index, 0, cards.Count), card);
    }

    public List<Card> GetAllCards()
    {
        //function so we can find all the cards in the zone
        return new List<Card>(cards);
    }
}