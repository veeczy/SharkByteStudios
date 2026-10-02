using UnityEngine;
using UnityEngine.InputSystem;

// Put this on each card prefab.
[RequireComponent(typeof(Card))]
public class CardDrag : MonoBehaviour
{
    [SerializeField] private float clickThresholdPixels = 5f;

    private Deck deck;
    private Camera cam;
    private Card card;
    private Plane dragPlane;
    private Vector3 grabOffset;
    private Vector2 mouseDownPosition;

    private void Awake()
    {
        deck = FindAnyObjectByType<Deck>();
        cam = Camera.main;
        card = GetComponent<Card>();
    }

    private void OnMouseDown()
    {
        mouseDownPosition = Mouse.current.position.ReadValue();

        // Drag along the plane the card currently sits on.
        dragPlane = new Plane(transform.forward, transform.position);
        grabOffset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + grabOffset;
    }

    private void OnMouseUp()
    {
        // Register clicks and drags separately, if card is not moved past the click threshold it is treated as a click
        Vector2 mouseUpPosition = Mouse.current.position.ReadValue();
        if (Vector2.Distance(mouseUpPosition, mouseDownPosition) < clickThresholdPixels)
            card.ToggleSelected();

        deck.ReorderCard(gameObject);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        dragPlane.Raycast(ray, out float distance);
        return ray.GetPoint(distance);
    }
}