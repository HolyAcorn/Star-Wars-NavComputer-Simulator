using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace SwNavComp
{
    
    public class InputManager : MonoBehaviour
    {
        PlayerInput playerInput;
        [SerializeField] Vector2Variable movementDirection;
        [SerializeField] BoolVariable shouldMoveCamera;

        [SerializeField] IntVariable zoomDirection;
        [SerializeField] GameEvent zoomInputEvent;

        [SerializeField] PlanetRuntimeSet selectedPlanet;
        [SerializeField] GameEvent clickedBlankEvent;

        [SerializeField] GameObjectRuntimeSet flipThroughObjectList;
        List<TMP_InputField> flipThroughList = new List<TMP_InputField>();

        [SerializeField] private IntReference flipIndex;
        [SerializeField] IntVariable flipBackwards;
        [SerializeField] GameEvent flipEvent;

        [SerializeField] GameEvent toggleDebug;
        [SerializeField] GameEvent addNewPlanet;

        float edgeSize = 10f;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }



        public void GetCameraMovementInput(CallbackContext context)
        {
            if (context.performed)
            {
                movementDirection.Value = context.ReadValue<Vector2>();
                shouldMoveCamera.Value = true;
            }
            else if (context.canceled)
            {
                shouldMoveCamera.Value = false;
            }
        }


        public void GetZoomInput(CallbackContext context)
        {
            float direction = context.ReadValue<float>();
            if (direction > 0) zoomDirection.Value = -1;
            else if (direction < 0) zoomDirection.Value = 1;
            else zoomDirection.Value = 0;
            zoomInputEvent.Raise();
        }

        public void GetLeftClickInput(CallbackContext context)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            selectedPlanet.Clear();
            clickedBlankEvent.Raise();
        }


        public void OnFlipPressed(CallbackContext context)
        {
            if (!context.started) return;
            flipEvent.Raise();
        }

        public void OnShiftPressed(CallbackContext context)
        {
            if (context.canceled) flipBackwards.Value = 1;
            else if(context.started) flipBackwards.Value = -1;
        }

        public void ToggleDebugPanel(CallbackContext context)
        {
            if (context.started) toggleDebug.Raise();
        }

        public void OnAddNewPlanet(CallbackContext context)
        {
            if (context.started) addNewPlanet.Raise();
        }
    }
}
