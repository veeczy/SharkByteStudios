using UnityEngine;
using System.Collections;

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
    private enum CameraState { left, right, normal, up, down}
    CameraState Direction = CameraState.normal; //default facing normal

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        touchManager = GameObject.Find("TouchManager").GetComponent<TouchManager>();
    }

    // Update is called once per frame
    void Update()
    {
        swipeX = touchManager.SwipeX();
        swipeY = touchManager.SwipeY();
        
        CameraRotate();
        CameraStateMachine();

        //If swipe or scroll then call method to move camera

        //Debug.Log(Direction);
    }

    private void CameraRotate()
    {
        if (!canRotate)
            {return;}

        if (swipeX == 1 && turnRight) //only commenting for one block, logic is all the same. if you swipe your finger
        {                             //right and the camera is in a position to turn right, run the block
            if (Direction == CameraState.normal)//if your camera is facing center, or normal
            {
                Direction = CameraState.right; //direction set to right for state machine
                StartCoroutine(RotateCamera(rightRotation.rotation)); //slerp it on over to the right
            }
            else if (Direction == CameraState.left) //if your camera is facing left
            {
                Direction = CameraState.normal; //direction set to normal
                StartCoroutine(RotateCamera(defaultRotation.rotation)); //slerp it on over to the right
            }
        }

        else if (swipeX == -1 && turnLeft)
        {
            if (Direction == CameraState.normal)
            {
                Direction = CameraState.left;
                StartCoroutine(RotateCamera(leftRotation.rotation));
            }
            else if (Direction == CameraState.right)
            {
                Direction = CameraState.normal;
                StartCoroutine(RotateCamera(defaultRotation.rotation));
            }
        }

        else if (swipeY == 1 && turnUp)
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

        else if (swipeY == -1 && turnDown)
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
            playerCam.transform.rotation = Quaternion.Slerp(startRotation,targetRotation,time);

            yield return null;
        }

        //if it hasnt rotated in time teleport it
        playerCam.transform.rotation = targetRotation;
    }

    public void CameraStateMachine()
    {
        /*if(playerCam.transform.rotation == Quaternion.Slerp(playerCam.transform.rotation, upRotation.transform.rotation, sensitivity))
        {
            Direction = CameraState.up;
        }
        else if(playerCam.transform.rotation == Quaternion.Slerp(playerCam.transform.rotation, downRotation.transform.rotation, sensitivity))
        {
            Direction = CameraState.down;
        }
        else if (playerCam.transform.rotation == Quaternion.Slerp(playerCam.transform.rotation, leftRotation.transform.rotation, sensitivity))
        {
            Direction = CameraState.left;
        }
        else if (playerCam.transform.rotation == Quaternion.Slerp(playerCam.transform.rotation, rightRotation.transform.rotation, sensitivity))
        {
            Direction = CameraState.right;
        }
        else { Direction = CameraState.normal; }*/

        if(Direction == CameraState.up) { turnUp = false; turnDown = true; turnLeft = false; turnRight = false; }
        else if (Direction == CameraState.down) { turnUp = true; turnDown = false; turnLeft = false; turnRight = false; }
        else if (Direction == CameraState.left) { turnUp = false; turnDown = false; turnLeft = false; turnRight = true; }
        else if (Direction == CameraState.right) { turnUp = false; turnDown = false; turnLeft = true; turnRight = false; }
        else if (Direction == CameraState.normal) { turnUp = true; turnDown = true; turnLeft = true; turnRight = true; }
    }

    public void ToggleCamera()
    {
        enabled = !enabled;
    }
}
