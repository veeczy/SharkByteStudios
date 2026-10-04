using UnityEngine;

public class Card : MonoBehaviour
{
    public bool selected = false;
    [SerializeField] private GameObject selectionBorder;

    public CardZone Zone { get; set; } // Set by CardZone script

    // Called by CardDrag when the card is clicked (not dragged).
    public void ToggleSelected()
    {
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