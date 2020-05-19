﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraMoveAndZoom : MonoBehaviour
{
    [SerializeField] Button btnLockOrUnLock;
    public float MaxOrthograhicSize, MinOrthograhicSize;
    public Vector2 maxXAndY; // The maximum x and y coordinates the camera can have.
    public Vector2 minXAndY; // The minimum x and y coordinates the camera can have.
    public float xSmooth; // How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth; // How smoothly the camera catches up with it's target movement in the y axis.
    bool isMovingLocked = false;
    public static bool isTakingAction = false;

    //public static bool isMovingCamera;
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.
                                               //public float speed = 0.1F;

    public delegate void CameraZoom(float orthographicSize);
    public static event CameraZoom OnCameraZoom;


    // Use this for initialization
    void Start()
    {
        transform.GetComponentInChildren<CanvasScaler>().scaleFactor = (float)UnityEngine.Screen.width / 192;
        ControlManager.OnMoveToPlant += OnMovingLock;
    }

    void OnMovingLock(bool doing)
    {
        isMovingLocked = doing;
        //Debug.Log(isMovingLocked);
    }


    // Update is called once per frame
    void Update()
    {
        transform.GetComponentInChildren<CanvasScaler>().scaleFactor = (float)UnityEngine.Screen.width / 192;
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        {

            if (isMovingLocked || isTakingAction)
            {
                return;
            }
            Vector2 scroll = Input.mouseScrollDelta;
            GetComponent<Camera>().orthographicSize -= Input.mouseScrollDelta.y * Time.deltaTime * 5f;
            GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize
                                                     , MinOrthograhicSize
                                                     , MaxOrthograhicSize);

            if (OnCameraZoom != null)
            {
                OnCameraZoom(GetComponent<Camera>().orthographicSize);
            }

            /*maxXAndY = new Vector2(16, 9f) - (GetComponent<Camera>().orthographicSize / 4.5f) * new Vector2(8, 4.5f);
            minXAndY = new Vector2(0, 0f) + (GetComponent<Camera>().orthographicSize / 4.5f) * new Vector2(8, 4.5f);*/

            maxXAndY = (new Vector2(16, 9f) - (GetComponent<Camera>().orthographicSize / 5.2f) * new Vector2(8, 4.5f))+ GetComponent<Camera>().orthographicSize/5.2f*new Vector2(1.244f,0.7f);
            minXAndY = (new Vector2(0, 0f) + (GetComponent<Camera>().orthographicSize / 5.2f) * new Vector2(8, 4.5f)) + GetComponent<Camera>().orthographicSize / 5.2f * new Vector2(1.244f, 0.7f);

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float targetX = transform.position.x;
            float targetY = transform.position.y;

            targetX = Mathf.Lerp(targetX, targetX + x, xSmooth * 10f * Time.deltaTime);
            targetY = Mathf.Lerp(targetY, targetY + y, ySmooth * 10f * Time.deltaTime);


            // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
            targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
            targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
            // Set the camera's position to the target position with the same z component.
            transform.position = new Vector3(targetX, targetY, transform.position.z);
            //transform.Translate(new Vector3(x,y,0)*Time.deltaTime);
        }
#elif UNITY_IOS || UNITY_ANDROID

        {
            if (isMovingLocked || isTakingAction)
            {
                return;
            }
            maxXAndY = (new Vector2(16, 9f) - (GetComponent<Camera>().orthographicSize / 5.2f) * new Vector2(8, 4.5f))+ GetComponent<Camera>().orthographicSize/5.2f*new Vector2(1.244f,0.7f);
            minXAndY = (new Vector2(0, 0f) + (GetComponent<Camera>().orthographicSize / 5.2f) * new Vector2(8, 4.5f)) + GetComponent<Camera>().orthographicSize / 5.2f * new Vector2(1.244f, 0.7f);
            Camera camera = Camera.main;

            // If there are two touches on the device...
            if (Input.touchCount == 2)
            {
                //InputManager.isTouchingOnBar = true;
                //ItemDragHandler.resetState();
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


                // If the camera is orthographic...
                if (camera.orthographic)
                {
                    // ... change the orthographic size based on the change in distance between the touches.
                    camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                    // Make sure the orthographic size never drops below zero.
                    camera.orthographicSize = Mathf.Max(camera.orthographicSize, MinOrthograhicSize);
                    camera.orthographicSize = Mathf.Min(camera.orthographicSize, MaxOrthograhicSize);
                    float targetX = transform.position.x;
                    float targetY = transform.position.y;
                    maxXAndY = new Vector2(16, 9f) - (GetComponent<Camera>().orthographicSize / 4.5f) * new Vector2(8, 4.5f);
                    minXAndY = new Vector2(0, 0f) + (GetComponent<Camera>().orthographicSize / 4.5f) * new Vector2(8, 4.5f);
                    targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
                    targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
                    transform.position = new Vector3(targetX, targetY, transform.position.z);
                    if (OnCameraZoom != null)
                    {
                        OnCameraZoom(GetComponent<Camera>().orthographicSize);
                    }
                }
                else
                {
                    // Otherwise change the field of view based on the change in distance between the touches.
                    camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                    // Clamp the field of view to make sure it's between 0 and 180.
                    camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, MinOrthograhicSize, MaxOrthograhicSize);
                }
            }
            else if (Input.touchCount == 1)
            {

                Touch touch = Input.GetTouch(0);
                //RaycastHit2D[] touches = Physics2D.RaycastAll(touch.position, touch.position, 0.5f);
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 touchDeltaPosition = touch.deltaPosition;
                    float targetX = transform.position.x;
                    float targetY = transform.position.y;
                    float smoothRatio = camera.orthographicSize / MaxOrthograhicSize;
                    targetX = Mathf.Lerp(targetX, targetX - touchDeltaPosition.x, xSmooth * smoothRatio * Time.deltaTime);
                    targetY = Mathf.Lerp(targetY, targetY - touchDeltaPosition.y, ySmooth * smoothRatio * Time.deltaTime);
                    // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
                    targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
                    targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

                    transform.position = new Vector3(targetX, targetY, transform.position.z);
                }

            }
           
        }
#endif
    }
}



