using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float sensitivity = 1000f; // how fast the scroll is from touch/mouse input

    private float _yaw = 0f; // if moving camera y axis
    private float _pitch = 0f; // if moving camera x axis

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If swipe or scroll then call method to move camera
    }
}
