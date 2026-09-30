using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    [Header("Input Manager")]
    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private InputAction touchStartPositionAction;
    

    [Header("Input Values")]
    Vector2 startPosition;
    Vector2 endPosition;

    private Touch touch;
    private Camera camera;

    public void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        GameObject foundCamera = GameObject.Find("Main Camera");
        camera = foundCamera.GetComponent<Camera>();
        touchPressAction = playerInput.actions["TouchPress"]; //this is the Input Action in the player category for button pressing in the Input Manager
        touchPositionAction = playerInput.actions["TouchPosition"];
        touchStartPositionAction = playerInput.actions["TouchStartPosition"];
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
        }
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressed;
        touchStartPositionAction.performed += TouchStartPosition; 
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
        touchStartPositionAction.performed -= TouchStartPosition; 
    }

    public void TouchPressed(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();

        Debug.Log(value);
    }

    public void TouchStartPosition(InputAction.CallbackContext context) 
    {
        Vector2 locationValue = context.ReadValue<Vector2>();
        Debug.Log(locationValue);
        //startPosition = camera.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
    }


    /*bool SwipeX()
    {
        //if(EndPosition - StartPosition < Vector.Right) 
        { return false; }

        //if (EndPosition - StartPosition < Vector.Left) // insert math formula
        { return true; }
    }

    bool SwipeY()
    {
        //if(EndPosition.position - StartPosition.position < Vector.Down) 
        { return false; }

        //if (EndPosition - StartPosition < Vector.Up) // insert math formula
        { return true; }
    }*/
}
