using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Card : MonoBehaviour
{
    public int energyCost = 2;

    public bool selected = false;
    [SerializeField] private GameObject selectionBorder;
    [SerializeField] private AudioClip playSound;
    [SerializeField] private AudioClip selectSound;
    [SerializeField, Range(0f, 1f)] private float soundVolume = 1f;

    [SerializeField, TextArea(2, 6)] private string description;

    // Shared by all cards, found once in the scene
    private static TMP_Text popupText;
    private static Card popupOwner; // The card whose description is currently showing

    public CardZone Zone { get; set; } // Set by CardZone script

    private void Awake()
    {
        if (popupText == null)
        {
            GameObject popupObject = GameObject.Find("PopupText");
            if (popupObject != null)
            {
                popupText = popupObject.GetComponent<TMP_Text>();
                popupObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // Tapping anywhere closes the popup
        if (popupOwner == this && Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            HidePopup();
    }

    /* private void OnDisable()
    {
        if (popupOwner == this)
            HidePopup();
    } */

    // Called by CardDrag when the card is held down.
    public void ShowPopup()
    {
        if (popupText == null) return;

        popupText.text = description;
        popupText.gameObject.SetActive(true);
        popupOwner = this;
    }

    public void HidePopup()
    {
        if (popupText != null)
            popupText.gameObject.SetActive(false);

        if (popupOwner == this)
            popupOwner = null;
    }

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