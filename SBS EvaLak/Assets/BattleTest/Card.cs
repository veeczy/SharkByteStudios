using UnityEngine;

public class Card : MonoBehaviour
{
    public int energyCost = 2;

    public bool selected = false;
    [SerializeField] private GameObject selectionBorder;

    public CardZone Zone { get; set; } // Set by CardZone script

    // Called by CardDrag when the card is clicked (not dragged).
    public void ToggleSelected()
    {
        // Cards can always be deselected, but can only be selected if there is enough energy left
        if (!selected && !DeckManager.Instance.CanSelect(this)) return;

        selected = !selected;
        selectionBorder.SetActive(selected);
    }

    // Runs every effect on this card, in the order they appear on the prefab
    public void OnPlay()
    {
        CardEffect[] effects = GetComponents<CardEffect>();

        foreach (CardEffect effect in effects)
        {
            effect.Activate();
        }
    }
}