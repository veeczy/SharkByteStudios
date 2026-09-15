using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    private void Update()
    {
 
         transform.Rotate(rotationSpeed * rotationAxis * Time.deltaTime);

    }
}
