using UnityEngine;

public class Card : MonoBehaviour
{
    public int energyCost = 2;

    public bool selected = false;
    [SerializeField] private GameObject selectionBorder;
    [SerializeField] private AudioClip playSound;
    [SerializeField] private AudioClip selectSound;
    [SerializeField, Range(0f, 1f)] private float soundVolume = 1f;

    public CardZone Zone { get; set; } // Set by CardZone script

    // Called by CardDrag when the card is clicked (not dragged).
    public void ToggleSelected()
    {
        // Cards can always be deselected, but can only be selected if there is enough energy left
        if (!selected && !DeckManager.Instance.CanSelect(this)) return;

        selected = !selected;

        if (selectionBorder != null)
            selectionBorder.SetActive(selected);

        if (selectSound != null)
            AudioSource.PlayClipAtPoint(selectSound, Camera.main.transform.position, soundVolume);
    }

    // Runs every effect on this card, in the order they appear on the prefab
    public void OnPlay()
    {
        if (playSound != null)
            AudioSource.PlayClipAtPoint(playSound, Camera.main.transform.position, soundVolume);

        CardEffect[] effects = GetComponents<CardEffect>();

        foreach (CardEffect effect in effects)
        {
            effect.Activate();
        }
    }
}