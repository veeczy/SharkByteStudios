using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Card))]
public class CardDrag : MonoBehaviour
{
    // Put this script on each card prefab

    [SerializeField] private float clickThresholdPixels = 5f;
    [SerializeField] private float holdSeconds = 0.6f;

    // Deck script checks this bool to avoid moving cards while being dragged by the player
    public bool IsDragging { get; private set; }

    private Camera cam;
    private Card card;
    private Plane dragPlane;
    private Vector3 grabOffset;
    private Vector2 mouseDownPosition;
    private float mouseDownTime;
    private bool popupShown;

    private void Awake()
    {
        cam = Camera.main;
        card = GetComponent<Card>();
    }

    private void Update()
    {
        if (!IsDragging || popupShown) return;

        float moved = Vector2.Distance(Pointer.current.position.ReadValue(), mouseDownPosition);

        // Show popup if held in place
        if (moved < clickThresholdPixels && Time.time - mouseDownTime >= holdSeconds)
        {
            popupShown = true;
            card.ShowPopup();
        }
    }

    private void OnMouseDown()
    {
        IsDragging = true;
        popupShown = false;
        mouseDownTime = Time.time;
        mouseDownPosition = Pointer.current.position.ReadValue();

        // Drag along the plane the card currently sits on.
        dragPlane = new Plane(transform.forward, transform.position);
        grabOffset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        float moved = Vector2.Distance(Pointer.current.position.ReadValue(), mouseDownPosition);

        if (popupShown && moved >= clickThresholdPixels)
            card.HidePopup();

        transform.position = GetMouseWorldPosition() + grabOffset;
    }

    private void OnMouseUp()
    {
        IsDragging = false;

        // popup stays up after release

        // Register clicks and drags separately, if card is not moved past the click threshold it is treated as a click
        // A hold that showed the popup doesnt change the selection
        Vector2 mouseUpPosition = Pointer.current.position.ReadValue();
        if (!popupShown && Vector2.Distance(mouseUpPosition, mouseDownPosition) < clickThresholdPixels)
            card.ToggleSelected();

        card.Zone.Reorder(card);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Pointer.current.position.ReadValue());
        dragPlane.Raycast(ray, out float distance);
        return ray.GetPoint(distance);
    }
}