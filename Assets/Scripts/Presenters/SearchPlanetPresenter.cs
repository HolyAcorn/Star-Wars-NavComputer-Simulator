using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public class SearchPlanetPresenter : MonoBehaviour
    {

        [SerializeField] PlanetRuntimeSet planetMasterList;
        [SerializeField] PlanetRuntimeSet selectedPlanet;
        [SerializeField] GameEvent newSelectedPlanetEvent;
        [SerializeField] GameEvent disableInput;
        [SerializeField] GameEvent enableInput;
        [SerializeField] GameEvent moveCamera;

        [SerializeField] GameObject errorObject;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnValueChanged()
        {
            disableInput.Raise();
        }

        public void OnEndEdit(string input)
        {
            errorObject.SetActive(false);
            bool foundMatch = false;
            foreach (var planet in planetMasterList.items)
            {
                if (planet.displayName.ToLower() == input.ToLower())
                {
                    selectedPlanet.Clear();
                    selectedPlanet.Add(planet);
                    newSelectedPlanetEvent.Raise();
                    moveCamera.Raise();
                    foundMatch = true;

                }
            }
            if (!foundMatch)
            {
                errorObject.SetActive(true);
                enableInput.Raise();
            }


        }
    }
}
