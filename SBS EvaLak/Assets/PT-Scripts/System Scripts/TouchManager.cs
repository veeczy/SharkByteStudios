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
    Vector2 StartPosition;
    Vector2 EndPosition;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions.FindAction("TouchPress"); //this is the Input Action in the player category for button pressing in the Input Manager
        touchPositionAction = playerInput.actions.FindAction("TouchPosition");
    }

    bool SwipeX()
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
    }
}
