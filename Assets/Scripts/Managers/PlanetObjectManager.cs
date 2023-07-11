using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace SwNavComp
{
    public class PlanetObjectManager : MonoBehaviour, IPointerClickHandler
    {

        [HideInInspector] public Planet planet;

        [SerializeField] PlanetRuntimeSet selectedPlanet;
        [SerializeField] GameEvent newSelectedPlanetEvent;

        [SerializeField] GameObject outline;

        public void OnPointerClick(PointerEventData eventData)
        {
            //if (!EventSystem.current.IsPointerOverGameObject()) return;
            selectedPlanet.Clear();
            selectedPlanet.Add(planet);
            newSelectedPlanetEvent.Raise();
        }

        public void FlipOutline()
        {
            if (selectedPlanet.Get(0) == planet) outline.SetActive(true);
            else outline.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
