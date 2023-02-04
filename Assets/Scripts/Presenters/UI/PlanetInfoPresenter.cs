using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SwNavComp
{
    public class PlanetInfoPresenter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] PlanetRuntimeSet selectedPlanet;
        [SerializeField] GameObject planetNameTextObject;
        [SerializeField] PlanetRuntimeSet startingPlanet;
        [SerializeField] PlanetRuntimeSet targetPlanet;
        [SerializeField] GameEvent createPathEvent;
        [SerializeField] GameObject hyperLanesTextObject;

        public void DisplayPlanet()
        {
            Text planetName = planetNameTextObject.GetComponent<Text>();
            
            planetName.text = selectedPlanet.Get(0).displayName;

            TMP_Text hyperLaneNames = hyperLanesTextObject.GetComponent<TMP_Text>();
            hyperLaneNames.text = "";
            foreach (string hyperLane in selectedPlanet.Get(0).HyperlaneRoutes)
            {
                hyperLaneNames.text = hyperLaneNames.text + ", " + hyperLane;
            }
        }

        private void OnApplicationQuit()
        {
            targetPlanet.Clear();
            startingPlanet.Clear();
        }

        public void SetStartingPlanet()
        {
            startingPlanet.Clear();
            startingPlanet.Add(selectedPlanet.Get(0));
        }

        public void SetTargetPlanet()
        {
            targetPlanet.Clear();
            targetPlanet.Add(selectedPlanet.Get(0));
        }

        public void OpenPlanetWikiURL()
        {
            selectedPlanet.Get(0).OpenLink();
        }

    }
}
