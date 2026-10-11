using UnityEngine;

public class RocketShipHover : MonoBehaviour
{

    [SerializeField] private float hoverSpeed;
    [SerializeField] private float hoverHeight;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        transform.position = new Vector3(startPosition.x, newY, hoverSpeed);
    }

}
