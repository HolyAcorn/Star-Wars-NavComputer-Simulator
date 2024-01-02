using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SwNavComp
{
    public class CustomHyperLanePlanetUIPresenter : MonoBehaviour
    {
        [SerializeField] GameObject planetItemRef;

        [SerializeField] HyperLaneRuntimeSet hyperLaneList;
        [SerializeField] TMP_Dropdown hyperLaneDropdown;
        [SerializeField] GameObject planetScrollViewContentRef;


        private HyperLane selectedHyperLane;

        // Start is called before the first frame update
        void Start()
        {
            List<string> hyperLaneNames = new List<string>();

            foreach (HyperLane hyperLane in hyperLaneList.items)
            {
                hyperLaneNames.Add(hyperLane.name);
            }
            hyperLaneDropdown.ClearOptions();
            hyperLaneDropdown.AddOptions(hyperLaneNames);

        }

        public void SetSelectedHyperLane(int value)
        {
            string valueName = hyperLaneList.items[value].name;
            Debug.Log(value + " " + valueName);
            foreach (HyperLane hyperLane in hyperLaneList.items)
            {
                if (hyperLane.name == valueName)
                {
                    selectedHyperLane = hyperLane;
                    break;
                }
            }
            PopulatePlanetView();
        }

        public void PopulatePlanetView()
        {
            /*for (int i = 0; i < planetScrollViewContentRef.transform.childCount; i++)
            {
                GameObject item = planetScrollViewContentRef.transform.GetChild(i).gameObject;
                Destroy(item);
                i--;
            }*/
            for (int i = 0; i < selectedHyperLane.Planets.Count; i++)
            {
                Planet planet = selectedHyperLane.Planets[i];
                GameObject instance = Instantiate(planetItemRef, planetScrollViewContentRef.transform);
                instance.GetComponent<PlanetScrollviewItemPresenter>().Initialize(planet, i + 1);
                
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
