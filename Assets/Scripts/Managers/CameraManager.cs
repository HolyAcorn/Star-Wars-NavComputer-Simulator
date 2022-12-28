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
        }

        public void Zoom()
        {
            _camera.orthographicSize += zoomAmount * zoomDirection.Value;
            if (_camera.orthographicSize == slowDownMovementThreshold) moveSpeed = slowedDownSpeed;
            if (_camera.orthographicSize > slowDownMovementThreshold) moveSpeed = originalMoveSpeed;
            if(_camera.orthographicSize < minCameraSize) _camera.orthographicSize = minCameraSize;
            if(_camera.orthographicSize > maxCameraSize) _camera.orthographicSize = maxCameraSize;
            currentZoom.Value = _camera.orthographicSize;
        }
    }
}
