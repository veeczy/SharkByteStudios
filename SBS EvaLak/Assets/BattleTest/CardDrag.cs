using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Card))]
public class CardDrag : MonoBehaviour
{
    // Put this script on each card prefab

    [SerializeField] private float clickThresholdPixels = 5f;

    // Read by Deck so it doesn't pull a card back to its slot while it is being dragged.
    public bool IsDragging { get; private set; }

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
        IsDragging = true;
        mouseDownPosition = Pointer.current.position.ReadValue();

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
        IsDragging = false;

        // Register clicks and drags separately, if card is not moved past the click threshold it is treated as a click
        Vector2 mouseUpPosition = Pointer.current.position.ReadValue();
        if (Vector2.Distance(mouseUpPosition, mouseDownPosition) < clickThresholdPixels)
            card.ToggleSelected();

        deck.ReorderCard(gameObject);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Pointer.current.position.ReadValue());
        dragPlane.Raycast(ray, out float distance);
        return ray.GetPoint(distance);
    }
}