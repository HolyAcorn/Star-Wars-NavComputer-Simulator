using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SwNavComp
{
    public class PlanetObject : MonoBehaviour
    {
        public Planet planet;

        [HideInInspector]
        public InputHandler inputHandler;
        private void OnMouseDown()
        {
            if (inputHandler != null)
            {
                inputHandler.OnPlanetClick(planet);
            }
        }
    }
}