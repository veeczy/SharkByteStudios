using UnityEngine;

public class Card : MonoBehaviour
{
    public bool selected = false;
    [SerializeField] private GameObject selectionBorder;

    public float damage;

    // Called by CardDrag when the card is clicked (not dragged).
    public void ToggleSelected()
    {
        selected = !selected;
        selectionBorder.SetActive(selected);
    }
}