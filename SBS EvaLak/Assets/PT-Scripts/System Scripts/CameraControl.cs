
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Camera Variables")]
    private float sensitivity = 5f; // how fast the scroll is from touch/mouse input
    public Camera playerCam;

    

    [Header("Camera Rotation")]
    [SerializeField] private Transform rightRotation;
    [SerializeField] private Transform leftRotation;
    [SerializeField] private Transform upRotation;
    [SerializeField] private Transform downRotation;
    [SerializeField] private Transform defaultRotation;
    [SerializeField] private Transform reverseRotation;

    [Header("Touch Controls")]
    int swipeX; //-1 = left, 0 default, 1 = right 
    int swipeY; //-1 = down, 0 default, 1 = up
    private TouchManager touchManager;

    [Header("Mini State Machine")]
    public bool canRotate = true; //stays true while in hanger scene, will get set to false in scenes where camera does not move
    private bool turnLeft = false;
    private bool turnRight = false;
    private bool turnUp = false;
    private bool turnDown = false;
    public enum CameraState { left, right, normal, up, down, reverse }
    public CameraState Direction = CameraState.normal; //default facing normal

    [Header("Hangar Button Stuff")]
    [SerializeField] private GameObject mechUpgradeButton;
    [SerializeField] private GameObject levelSelectButton;
    [SerializeField] private GameObject deckDoorButton;
    [SerializeField] private GameObject shopDoorButton;

    [Header("Detect Scene")]
    [SerializeField] private string sceneName;
    [SerializeField] private Inventory inventory;
    [SerializeField] private List<string> sceneNameList = new List<string>() { "MechUpgrade", "HangarLevel" };

    [Header("Mech Upgrade Stuff")]
    [SerializeField] private GameObject upHeadButton;
    [SerializeField] private GameObject upArmsButton;
    [SerializeField] private GameObject downArmsButton;
    [SerializeField] private GameObject downLegsButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //init data for all
        touchManager = GameObject.Find("TouchManager").GetComponent<TouchManager>();
        GameObject DDOL = GameObject.Find("DDOL");
        inventory = DDOL.GetComponent<Inventory>();
        sceneName = inventory.sceneName; //get active scene name from inventory

        if (sceneName == sceneNameList[0]) //if in MechUpgrade, init data for Mech Upgrade
        {
            upHeadButton = GameObject.Find("Head");
            upArmsButton = GameObject.Find("Arms Upper");

            downArmsButton = GameObject.Find("Arms Lower");
            downLegsButton = GameObject.Find("Legs");

            //set inactive at start unless looking at it
            disableButton(upHeadButton);
            disableButton(upArmsButton);
            disableButton(downArmsButton);
            disableButton(downLegsButton);
        }

        if(sceneName == sceneNameList[1]) //if in Hangar, init data for hangar
        { 
            //button for movement
            if (mechUpgradeButton == null) { mechUpgradeButton = GameObject.Find("Mech Upgrades"); }
            if (levelSelectButton == null) { levelSelectButton = GameObject.Find("LevelSelect"); }

            if (deckDoorButton == null) { deckDoorButton = GameObject.Find("DeckBuild"); }
            if (shopDoorButton == null) { shopDoorButton = GameObject.Find("Shop"); }

            //set inactive at start unless looking at it
            disableButton(mechUpgradeButton);
            disableButton(levelSelectButton);
            disableButton(deckDoorButton);
            disableButton(shopDoorButton);
        }
    }

    // Update is called once per frame
    void Update()
    {
        swipeX = touchManager.SwipeX();
        swipeY = touchManager.SwipeY();

        CameraRotate();
        CameraStateMachine();

        //Debug.Log(Direction);
        if (sceneName == sceneNameList[0]) //if in Mech Upgrade
        { 
            if(Direction == CameraState.normal) //if up enable up buttons
            {
                enableButton(upHeadButton);
                enableButton(upArmsButton);

                disableButton(downArmsButton);
                disableButton(downLegsButton);
            }
            if(Direction == CameraState.down) //if down enable down buttons
            {
                enableButton(downArmsButton);
                enableButton(downLegsButton);

                disableButton(upHeadButton);
                disableButton(upArmsButton);
            }
        }

        if (sceneName == sceneNameList[1]) //if in Hangar
        { 
            if (Direction == CameraState.normal) //if facing forward (towards level select)
            {
                enableButton(levelSelectButton);
                disableButton(mechUpgradeButton);
                disableButton(deckDoorButton);
                disableButton(shopDoorButton);
            }
            if (Direction == CameraState.reverse) //if facing reverse (towards Mech Upgrade)
            {
                disableButton(levelSelectButton);
                enableButton(mechUpgradeButton);
                disableButton(deckDoorButton);
                disableButton(shopDoorButton);
            }
            if (Direction == CameraState.right) //if facing right (towards Deck Build)
            {
                disableButton(levelSelectButton);
                disableButton(mechUpgradeButton);
                enableButton(deckDoorButton);
                disableButton(shopDoorButton);
            }
            if (Direction == CameraState.left) //if facing left (towards Shop)
            {
                disableButton(levelSelectButton);
                disableButton(mechUpgradeButton);
                disableButton(deckDoorButton);
                enableButton(shopDoorButton);
            }
        }
    }

    private void CameraRotate()
    {
        if (!canRotate)
        { return; }

        if (swipeX == -1 && turnRight) //only commenting for one block, logic is all the same. if you swipe your finger
        {                             //right and the camera is in a position to turn right, run the block
            if (Direction == CameraState.normal)//if your camera is facing center, or normal
            {
                Direction = CameraState.right; //direction set to right for state machine
                StartCoroutine(RotateCamera(rightRotation.rotation)); //slerp it on over to the right
            }
            else if (Direction == CameraState.right)
            {
                Direction = CameraState.reverse;
                StartCoroutine(RotateCamera(reverseRotation.rotation));
            }
            else if (Direction == CameraState.reverse)
            {
                Direction = CameraState.left;
                StartCoroutine(RotateCamera(leftRotation.rotation));
            }
            else if (Direction == CameraState.left)
            {
                Direction = CameraState.normal;
                StartCoroutine (RotateCamera(defaultRotation.rotation));
            }

            else if (Direction == CameraState.left) //if your camera is facing left
            {
                Direction = CameraState.normal; //direction set to normal
                StartCoroutine(RotateCamera(defaultRotation.rotation)); //slerp it on over to the right
            }
        }

        else if (swipeX == 1 && turnLeft)
        {
            if (Direction == CameraState.normal)
            {
                Direction = CameraState.left;
                StartCoroutine(RotateCamera(leftRotation.rotation));
            }
            else if (Direction == CameraState.left)
            {
                Direction = CameraState.reverse;
                StartCoroutine(RotateCamera(reverseRotation.rotation));
            }
            else if (Direction == CameraState.reverse)
            {
                Direction = CameraState.right;
                StartCoroutine(RotateCamera(rightRotation.rotation));
            }
            else if (Direction == CameraState.right)
            {
                Direction = CameraState.normal;
                StartCoroutine(RotateCamera(defaultRotation.rotation));
            }
            else if (Direction == CameraState.right)
            {
                Direction = CameraState.normal;
                StartCoroutine(RotateCamera(defaultRotation.rotation));
            }
        }

        else if (swipeY == -1 && turnUp)
        {
            if (Direction == CameraState.normal)
            {
                Direction = CameraState.up;
                StartCoroutine(RotateCamera(upRotation.rotation));
            }
            else if (Direction == CameraState.down)
            {
                Direction = CameraState.normal;
                StartCoroutine(RotateCamera(defaultRotation.rotation));
            }
        }

        else if (swipeY == 1 && turnDown)
        {
            if (Direction == CameraState.normal)
            {
                Direction = CameraState.down;
                StartCoroutine(RotateCamera(downRotation.rotation));
            }
            else if (Direction == CameraState.up)
            {
                Direction = CameraState.normal;
                StartCoroutine(RotateCamera(defaultRotation.rotation));
            }
        }
    }

    private IEnumerator RotateCamera(Quaternion targetRotation)
    {
        //finds your current rotation
        Quaternion startRotation = playerCam.transform.rotation;
        float time = 0f;

        while (time < 2f)
        {
            time += Time.deltaTime * sensitivity;
            //rotates your camera based on the desired rotation
            playerCam.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, time);

            yield return null;
        }

        //if it hasnt rotated in time teleport it
        playerCam.transform.rotation = targetRotation;
    }

    public void CameraStateMachine()
    {
        if(sceneName == sceneNameList[0]) //if in mech upgrade, only can turn up and down
        {
            if (Direction == CameraState.up) { turnUp = false; turnDown = true; turnLeft = false; turnRight = false; }
            else if (Direction == CameraState.down) { turnUp = true; turnDown = false; turnLeft = false; turnRight = false; }
            else if (Direction == CameraState.left) { turnUp = false; turnDown = false; turnLeft = false; turnRight = false; }
            else if (Direction == CameraState.right) { turnUp = false; turnDown = false; turnLeft = false; turnRight = false; }
            else if (Direction == CameraState.normal) { turnUp = false; turnDown = true; turnLeft = false; turnRight = false; }
        }

        if (sceneName == sceneNameList[1]) //if in hangar, you can have all 360 movement
        {
            if (Direction == CameraState.up) { turnUp = false; turnDown = true; turnLeft = false; turnRight = false; }
            else if (Direction == CameraState.down) { turnUp = true; turnDown = false; turnLeft = false; turnRight = false; }
            else if (Direction == CameraState.left) { turnUp = false; turnDown = false; turnLeft = true; turnRight = true; }
            else if (Direction == CameraState.right) { turnUp = false; turnDown = false; turnLeft = true; turnRight = true; }
            else if (Direction == CameraState.normal) { turnUp = true; turnDown = true; turnLeft = true; turnRight = true; }
        }
    }

    public void ToggleCamera()
    {
        enabled = !enabled;
    }

    public void enableButton(GameObject button) { button.SetActive(true); }
    public void disableButton(GameObject button) { button.SetActive(false); }
}
