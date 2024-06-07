using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SwNavComp
{
    public class PlanetObjectManager : MonoBehaviour, IPointerDownHandler
    {

        [HideInInspector] public Planet planet;

        [SerializeField] PlanetRuntimeSet selectedPlanet;
        [SerializeField] GameEvent newSelectedPlanetEvent;

        [SerializeField] GameObject outline;

        [SerializeField] FloatReference size;

        [SerializeField] BoolVariable editMode;

        private void Start()
        {
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            selectedPlanet.Clear();
            selectedPlanet.Add(planet);
            newSelectedPlanetEvent.Raise();
        }


        public void FlipOutline()
        {
            if (selectedPlanet.Get(0) == planet) outline.SetActive(true);
            else outline.SetActive(false);
        }

        public void UpdateSize()
        {
            transform.localScale = new Vector3(size.Value, size.Value, size.Value);
        }


    }
}
