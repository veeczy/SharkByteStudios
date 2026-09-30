using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    [Header("Camera Variables")]
    private float sensitivity = 1000f; // how fast the scroll is from touch/mouse input
    public Camera playerCam;

    private float _yaw = 0f; // if moving camera y axis
    private float _pitch = 0f; // if moving camera x axis

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
    public bool canRotate = true;
    private bool turnLeft = false;
    private bool turnRight = false;
    private bool turnUp = false;
    private bool turnDown = false;
    private enum CameraState { left, right, normal, up, down}
    CameraState Direction = CameraState.normal; //default facing normal

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        swipeX = touchManager.GetComponent<TouchManager>().SwipeX();
        swipeY = touchManager.GetComponent<TouchManager>().SwipeY();
        
        UpdateYawPitch();
        CameraStateMachine();
        CameraRotateX();
        CameraRotateY();

        //If swipe or scroll then call method to move camera
    }

    private void CameraRotateX()
    {
        if (canRotate) //if  camera can rotate
        {
            if (_pitch >= 0f && turnRight) //swipe right and can turn right
            {
                if(Direction == CameraState.left) { playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, leftRotation.transform.rotation, sensitivity); } // if facing left, right turn to normal
                else if(Direction == CameraState.normal) { playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, rightRotation.transform.rotation, sensitivity); } // else turn right 
            }
            else if ( _pitch <= 0 && turnLeft) //swipe left and can turn left
            {
                if(Direction == CameraState.right) { playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, leftRotation.transform.rotation, sensitivity); } //if facing right, left turn to normal
                else if(Direction == CameraState.normal) { playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, leftRotation.transform.rotation, sensitivity); } // else turn left
            }

        }
    }
    private void CameraRotateY()
    {
        if (canRotate) //if camera can rotate
        {
            if (_yaw >= 0f && turnUp) //swipe up and can turn up
            {
                if (Direction == CameraState.down) { playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, leftRotation.transform.rotation, sensitivity); } // if facing down, up turn to normal
                else if (Direction == CameraState.normal) { playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, upRotation.transform.rotation, sensitivity); } //else turn up
            }
            else if (_yaw <= 0f && turnDown) //swipe down and can turn down
            {
                if (Direction == CameraState.up) { playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, leftRotation.transform.rotation, sensitivity); } // if facing up, down turn to normal
                else if (Direction == CameraState.normal) { playerCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, downRotation.transform.rotation, sensitivity); } //else turn down
            }
        }
    }
    private void UpdateYawPitch()
    {
        switch (swipeX)
        {
            case 0: _yaw = 0f; 
                break;
            case 1: _yaw = 1f; 
                break;
            case -1: _yaw = -1f; 
                break;
            
            default: break;
        }

        switch (swipeY)
        {
            case 0:
                _pitch = 0f;
                break;
            case 1:
                _pitch = 1f;
                break;
            case -1:
                _pitch = -1f;
                break;

            default: break;
        }
    }

    public void CameraStateMachine()
    {
        if(playerCam.transform.rotation == Quaternion.Slerp(playerCam.transform.rotation, upRotation.transform.rotation, sensitivity))
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
        else { Direction = CameraState.normal; }

        if(Direction == CameraState.up) { turnUp = false; turnDown = true; turnLeft = false; turnRight = false; }
        else if (Direction == CameraState.down) { turnUp = true; turnDown = false; turnLeft = false; turnRight = false; }
        else if (Direction == CameraState.left) { turnUp = false; turnDown = false; turnLeft = false; turnRight = true; }
        else if (Direction == CameraState.right) { turnUp = false; turnDown = false; turnLeft = true; turnRight = false; }
        else if (Direction == CameraState.normal) { turnUp = true; turnDown = true; turnLeft = true; turnRight = true; }
    }
}
