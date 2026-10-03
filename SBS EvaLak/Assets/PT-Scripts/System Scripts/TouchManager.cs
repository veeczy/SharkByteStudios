using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using enhanced = UnityEngine.InputSystem.EnhancedTouch;

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
    Vector2 locationValue;
    Vector2 endLocationValue;

    [SerializeField] private float swipeThreshold = 100f;


    //private UnityEngine.InputSystem.EnhancedTouch.Touch enhancedTouch;
    //private enhanced.Touch enhancedTouch;
    private UnityEngine.Touch touch;
    private Camera camera;

    private float clickValue;

    public bool clickHeld = false;

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
        /*if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            clickHeld = true;
        }
        else {clickHeld = false;}*/

       // Debug.Log(clickValue);
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressed;
        touchPressAction.canceled += TouchReleased;
        touchPressAction.canceled += FinalPosition;

        touchStartPositionAction.performed += TouchStartPosition; 

        touchPositionAction.performed += TouchPosition;
        //EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
        touchPressAction.canceled -= TouchReleased;
        touchPressAction.canceled -= FinalPosition;

        touchPressAction.performed -= TouchPressed;
        touchStartPositionAction.performed -= TouchStartPosition; 

        touchPositionAction.performed -= TouchPosition;
        //EnhancedTouchSupport.Disable();
    }

    public void TouchPressed(InputAction.CallbackContext context)
    {
        clickValue = context.ReadValue<float>();

        clickHeld = true;
    }

    public void TouchReleased(InputAction.CallbackContext context)
    {
        clickHeld = false;
    }

    public void TouchStartPosition(InputAction.CallbackContext context) 
    {
        startPosition = context.ReadValue<Vector2>();
        //Debug.Log("Start Pos" + locationValue);
        //startPosition = camera.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
    }

    public void TouchPosition(InputAction.CallbackContext context) 
    {
        endLocationValue = context.ReadValue<Vector2>();
        //Debug.Log("End Pos" + endLocationValue);
        //startPosition = camera.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
    }

    public void FinalPosition(InputAction.CallbackContext context)
    {
        endPosition = endLocationValue;
        //Debug.Log(endLocationValue);
    }
    

    public int SwipeX()
    {
        float difference = endPosition.x - startPosition.x;

        Debug.Log(difference);

        if(Mathf.Abs(difference) < swipeThreshold)
        {return 0;}

        if(difference < 0) 
        { 
            //Debug.Log("Left");
            return -1; }

        if (difference > 0)
        { 
            //Debug.Log("Right");
            return 1; }

        if (difference == 0)
        { return 0; }

        else { return 0; }
    }

    public int SwipeY()
    {
        float difference = endPosition.y - startPosition.y;

        if(Mathf.Abs(difference) < swipeThreshold)
        {return 0;}

        if (difference < 0)
        { return -1; }

        if (difference > 0)
        { return 1; }

        if (difference == 0)
        { return 0; }

        else { return 0; }
    }
}
