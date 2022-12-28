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

        public void OnPointerDown(PointerEventData eventData)
        {
            selectedPlanet.Clear();
            selectedPlanet.Add(planet);
            newSelectedPlanetEvent.Raise();
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
