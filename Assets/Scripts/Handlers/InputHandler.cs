using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    private Camera myCamera;
    public Vector2 mousePosition;

    private Vector3 cameraFollowPosition;
    private CameraFollow cameraFollow;

    public PlanetList activePlanet;
    public GameEvent clickedPlanet;
    public GameEvent clickedBlank;

    public FloatReference sizeDifference;

    public Vector2Reference mousePositionRef;
    [Header("Camera Movement")]
    [SerializeField] float moveAmount = 20f;
    [SerializeField] float edgeSize = 30f;
    [Header("Camera Zoom")]
    [SerializeField] float zoomStep = 1f;
    [SerializeField] float maxZoom = 5.0f;
    [SerializeField] float minZoom = 50.0f;
    [SerializeField] FloatReference currentCameraZoom;
    [SerializeField] GameEvent CheckCameraSize;

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
            if (EventSystem.current.IsPointerOverGameObject()) return;

            mousePosition *= 10;
            mousePosition = new Vector2((int)mousePosition.x, (int)mousePosition.y) / sizeDifference.Value;
            mousePositionRef.Variable.Value = mousePosition;
            if (!clickedOnPlanet)
            {
                activePlanet.ClearList();
                clickedBlank.Raise();
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
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (activePlanet.Planets.Count > 0)
        {
            activePlanet.ClearList();
        }
        activePlanet.AddPlanet(planet);
        clickedOnPlanet = true;
        clickedPlanet.Raise();

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
            currentCameraZoom.Variable.Value = myCamera.orthographicSize;

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && myCamera.orthographicSize < minZoom)
        {

            myCamera.orthographicSize += zoomStep;
            currentCameraZoom.Variable.Value = myCamera.orthographicSize;

        }
        CheckCameraSize.Raise();
    }
}



