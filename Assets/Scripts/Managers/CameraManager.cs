using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public class CameraManager : MonoBehaviour
    {

        [SerializeField] BoolVariable shouldMove;
        [SerializeField] Vector2Variable moveDirection;
        [SerializeField] float moveSpeed = 1f;
        [SerializeField] float slowedDownSpeed = 0.5f;
        float originalMoveSpeed;


        [SerializeField] IntVariable zoomDirection;
        [SerializeField] FloatVariable currentZoom;
        [SerializeField] float minCameraSize = 10f;
        [SerializeField] float maxCameraSize = 90f;
        [SerializeField] float zoomAmount = 1f;
        [SerializeField] float slowDownMovementThreshold = 10f;

        [SerializeField] PlanetRuntimeSet selectedPlanet;
        [SerializeField] GameEvent enableInput;
        private bool shouldMoveToSelectedPlanet = false;
        private Vector3 selectedPlanetPosition;
        private Vector3 startPosition;
        private float elapsedTime;
        private float desiredDuration = 3f;

        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            originalMoveSpeed = moveSpeed;
        }

        private void FixedUpdate()
        {
            if (shouldMove.Value)
            {
                Vector3 movement = moveDirection.Value * moveSpeed;
                transform.position += movement;
            }
            if (shouldMoveToSelectedPlanet)
            {
                MoveToSelectedPlanet();
            }
        }

        public void Zoom()
        {
            _camera.orthographicSize += zoomAmount * zoomDirection.Value;
            if (_camera.orthographicSize == slowDownMovementThreshold) moveSpeed = slowedDownSpeed;
            if (_camera.orthographicSize > slowDownMovementThreshold) moveSpeed = originalMoveSpeed;
            if (_camera.orthographicSize < minCameraSize) _camera.orthographicSize = minCameraSize;
            if (_camera.orthographicSize > maxCameraSize) _camera.orthographicSize = maxCameraSize;
            currentZoom.Value = _camera.orthographicSize;
        }

        private void MoveToSelectedPlanet()
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;
            transform.position = Vector3.Lerp(startPosition, selectedPlanetPosition, Mathf.SmoothStep(0, 1, percentageComplete));
            if (transform.position == selectedPlanetPosition) MoveToPlanetDone();
        }
        
        private void MoveToPlanetDone()
        {
            elapsedTime = 0.0f;
            shouldMoveToSelectedPlanet = false;
            enableInput.Raise();
        }

        public void OnMoveToPlanetEventRaised()
        {
            startPosition = transform.position;
            GameObject go = selectedPlanet.Get(0).gameObject;
            selectedPlanetPosition = new Vector3(go.transform.position.x, go.transform.position.y, transform.position.z);
            shouldMoveToSelectedPlanet = true;
        }

        
    }
}
