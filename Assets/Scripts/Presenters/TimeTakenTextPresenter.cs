using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SwNavComp
{
    public class TimeTakenTextPresenter : MonoBehaviour
    {
        [SerializeField] StringVariable displayString;
        [SerializeField] PlanetRuntimeSet startingPlanet;
        [SerializeField] PlanetRuntimeSet endingPlanet;
        [SerializeField] Text text;

        private void Start()
        {
            displayString.Value = "";
        }

        public void DisplayText()
        {
            string textString = "Travelling from " + startingPlanet.Get(0).displayName + " to " + endingPlanet.Get(0).displayName + " will take approximately " + displayString.Value;
            text.text = textString;
        }

        private void OnApplicationQuit()
        {
            displayString.Value = "";
        }
    }
}
