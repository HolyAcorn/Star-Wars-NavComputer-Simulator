using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Camera myCamera;
    public Vector2 mousePosition;

    private Vector3 cameraFollowPosition;
    private CameraFollow cameraFollow;

    public PlanetList activePlanet;

    public Vector2Reference mousePositionRef;
    [Header("Camera Movement")]
    [SerializeField] float moveAmount = 20f;
    [SerializeField] float edgeSize = 30f;
    [Header("Camera Zoom")]
    [SerializeField] float zoomStep = 1f;
    [SerializeField] float maxZoom = 5.0f;
    [SerializeField] float minZoom = 50.0f;

    private bool clickedOnPlanet = false;
    private void Start()
    {
        cameraFollow = GetComponent<CameraFollow>();
        myCamera = GetComponent<Camera>();
        cameraFollow.Setup(() => cameraFollowPosition);
    }
    void Update()
    {
        mousePosition = myCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition *= 10;
            mousePosition = new Vector2((int)mousePosition.x, (int)mousePosition.y) / 10;
            mousePositionRef.Variable.Value = mousePosition;
            if (!clickedOnPlanet)
            {
                activePlanet.ClearList();
            }
            clickedOnPlanet = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(mousePosition);
        }
        GetCameraMovement();
        HandleZoom();
    }


    public void OnPlanetClick(Planet planet)
    {
        if (activePlanet.Planets.Count > 0)
        {
            activePlanet.ClearList();
        }
        activePlanet.AddPlanet(planet);
        clickedOnPlanet = true;
    }
    private void GetCameraMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cameraFollowPosition.y += moveAmount * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            cameraFollowPosition.y -= moveAmount * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            cameraFollowPosition.x += moveAmount * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            cameraFollowPosition.x -= moveAmount * Time.deltaTime;
        }
        EdgeScrolling();


        void EdgeScrolling()
        {
            if (Input.mousePosition.x > Screen.width - edgeSize)
            {
                cameraFollowPosition.x += moveAmount * Time.deltaTime;
            }
            if (Input.mousePosition.x < edgeSize)
            {
                cameraFollowPosition.x -= moveAmount * Time.deltaTime;
            }
            if (Input.mousePosition.y > Screen.height - edgeSize)
            {
                cameraFollowPosition.y += moveAmount * Time.deltaTime;
            }
            if (Input.mousePosition.y < edgeSize)
            {
                cameraFollowPosition.y -= moveAmount * Time.deltaTime;
            }
        }
    }

    void HandleZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && myCamera.orthographicSize > maxZoom)
        {
            myCamera.orthographicSize -= zoomStep;

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && myCamera.orthographicSize <  minZoom)
        {

            myCamera.orthographicSize += zoomStep;
        }




    }
}



