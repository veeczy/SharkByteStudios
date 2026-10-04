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
    private int swipeX;
    private int swipeY;


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

        touchStartPositionAction.performed += TouchStartPosition; 

        touchPositionAction.performed += TouchPosition;
        //EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
        touchPressAction.canceled -= TouchReleased;

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

        endPosition = touchPositionAction.ReadValue<Vector2>();

        CalculateSwipe();
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
    
    private void CalculateSwipe()
    {
        float differenceX = endPosition.x - startPosition.x;
        float differenceY = endPosition.y - startPosition.y;

        swipeX = GetSwipeDirection(differenceX);
        swipeY = GetSwipeDirection(differenceY);
    }

    private int GetSwipeDirection(float difference)
    {
        if (Mathf.Abs(difference) < swipeThreshold)
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

        return 0;
    }

    public int SwipeX() //these store the inputs, so whenever you make the input it checks to see
    {                   //if swipeX is 1, whice means the input is made. it then immediately get's
        int result = swipeX; //set to zero and we return the reusult to see if our input should run

        swipeX = 0;

        return result;
    }

    public int SwipeY()
    {
        int result = swipeY;

        swipeY = 0;

        return result;
    }
}
