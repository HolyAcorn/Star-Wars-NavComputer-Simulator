using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SwNavComp
{
    namespace HLEditor
    {
        public class CustomHyperLanePlanetUIPresenter : MonoBehaviour
        {
            [SerializeField] GameObject planetItemRef;
            [SerializeField] GameObject addNewPlanetRef;

            [SerializeField] HyperLaneRuntimeSet hyperLaneList;
            [SerializeField] TMP_Dropdown hyperLaneDropdown;
            [SerializeField] GameObject planetScrollViewContentRef;

            [SerializeField] GameObjectRuntimeSet flipThroughSet;
            [SerializeField] GameEvent updateFlipThroughSet;
            [SerializeField] StringReference dataLocation;

            [SerializeField] IntReference currentHighestIndex;
            [SerializeField] PlanetRuntimeSet noHyperlanes;

            private HyperLaneDropdown selectedHyperLane;

            private HyperLane noHyperlanesLane;

            List<Planet> currentHyperLaneList = new List<Planet>();

            string newHyperlaneName = "";

            private void Start()
            {
                SetupDropdownList();
                SetupNoHyperLaneLane();
            }

            void SetupNoHyperLaneLane()
            {
                noHyperlanesLane = new HyperLane("Planets without a Hyperlane", noHyperlanes, true);
            }

            void SetupDropdownList()
            {
                List<string> hyperLaneNames = new List<string>();

                foreach (HyperLane hyperLane in hyperLaneList.items)
                {
                    hyperLaneNames.Add(hyperLane.name);
                }
                hyperLaneNames.Add("Planets without a Hyperlane");
                hyperLaneDropdown.ClearOptions();
                hyperLaneDropdown.AddOptions(hyperLaneNames);
                currentHighestIndex.Variable.Value = JsonConverter.GetHighestPlanetIndex(dataLocation.Value);
                if (hyperLaneList.Count() <= 0) return;
                selectedHyperLane = new HyperLaneDropdown( hyperLaneList.Get(0));
                PopulatePlanetView();
            }

            public void SetSelectedHyperLane(int value)
            {
                if (value >= hyperLaneList.Count())
                {
                    selectedHyperLane = new HyperLaneDropdown(noHyperlanesLane);
                }
                else
                {
                    string valueName = hyperLaneList.items[value].name;
                    Debug.Log(value + " " + valueName);
                    foreach (HyperLane hyperLane in hyperLaneList.items)
                    {
                        if (hyperLane.name == valueName)
                        {
                            selectedHyperLane = new(hyperLane);

                            break;
                        }
                    }
                }
                flipThroughSet.Clear();
                currentHyperLaneList.Clear();
                PopulatePlanetView();
                updateFlipThroughSet.Raise();
            }

            public void PopulatePlanetView()
            {
                RectTransform planetScrollViewRect = planetScrollViewContentRef.GetComponent<RectTransform>();
                planetScrollViewRect.sizeDelta = new Vector2(0, addNewPlanetRef.GetComponent<RectTransform>().rect.height);
                
                List<GameObject> children = new List<GameObject>();
                foreach (Transform child in planetScrollViewContentRef.transform)
                {
                    children.Add(child.gameObject);
                }
                children.ForEach(child => Destroy(child));
                
                for (int i = 0; i < selectedHyperLane.hyperLane.Planets.Count; i++)
                {
                    currentHyperLaneList.Add(selectedHyperLane.hyperLane.Planets[i]);
                    Planet planet = currentHyperLaneList[i];
                    GameObject instance = Instantiate(planetItemRef, planetScrollViewContentRef.transform);
                    instance.GetComponent<PlanetScrollviewItemPresenter>().Initialize(this, planet, i + 1);
                    flipThroughSet.Add(instance);
                    planetScrollViewRect.sizeDelta = new Vector2(0, planetScrollViewRect.sizeDelta.y + addNewPlanetRef.GetComponent<RectTransform>().rect.height);
                }
                GameObject newPlanetButtonInstance = Instantiate(addNewPlanetRef, planetScrollViewContentRef.transform);
                newPlanetButtonInstance.name = newPlanetButtonInstance.name.Replace("(Clone)", "");
                flipThroughSet.Add(newPlanetButtonInstance);
            }



            public void ChangePlanetOrder(int orderID, bool up)
            {
                if (orderID == currentHyperLaneList.Count && !up) return;
                if (orderID == 1 && up) return;
                int i = 1;
                if (up) i = -1;

                var listOrderID = orderID - 1;


                Planet planet = currentHyperLaneList[listOrderID];
                Planet changePlanet = currentHyperLaneList[listOrderID + i];

                currentHyperLaneList[listOrderID + i] = planet;
                currentHyperLaneList[listOrderID] = changePlanet;
                PopulatePlanetView();
            }

            public void OnSaveHyperLane()
            {
                PlanetRuntimeSet planetRuntimeSet = new PlanetRuntimeSet();

                foreach (Transform child in planetScrollViewContentRef.transform)
                {
                    if (child.name == addNewPlanetRef.name) continue; 
                    Planet planet = child.GetComponent<PlanetScrollviewItemPresenter>().SavePlanet();
                    planetRuntimeSet.Add(planet);
                }
                HyperLane newHyperLane;
                if(selectedHyperLane.hyperLane.name == "Planets without a Hyperlane") newHyperLane = new HyperLane("No Hyperlane", planetRuntimeSet, true);
                else newHyperLane = new HyperLane(selectedHyperLane.hyperLane.name, planetRuntimeSet);
                JsonPlanetFile jsonFile = newHyperLane.CreateJsonObject();
                string json = JsonConverter.ConvertToJson(jsonFile);
                JsonConverter.SaveFileToJson(newHyperLane.name, json, Application.dataPath + "/StreamingAssets/Data/" + dataLocation.Value + "/");
            }

            public void AddNewPlanet()
            {
                currentHighestIndex.Variable.Value += 1;
                Planet planet = new Planet();
                planet.Initialize("", currentHighestIndex.Value, 0, 0, false);
                planet.HyperlaneRoutes.Add(selectedHyperLane.hyperLane.name);
                selectedHyperLane.hyperLane.Planets.Add(planet);
                currentHyperLaneList.Clear();
                PopulatePlanetView();
            }

            public void AddNewHyperLane()
            {
                if (newHyperlaneName == "") return;
                HyperLane newHyperlane = new HyperLane(newHyperlaneName, new PlanetRuntimeSet());
                hyperLaneList.Add(newHyperlane);
                SetupDropdownList();
                hyperLaneDropdown.value = hyperLaneList.Count() - 1;
                SetSelectedHyperLane(hyperLaneList.Count() - 1);
            }

            public void UpdateNewHyperLaneName(string value)
            {
                newHyperlaneName = value;
            }

            class HyperLaneDropdown
            {
                public HyperLane hyperLane;
                public List<PlanetScrollviewItemPresenter> planets;

                public HyperLaneDropdown(HyperLane hyperLane, List<PlanetScrollviewItemPresenter> planets = null)
                {
                    this.hyperLane = hyperLane;
                    this.planets = planets;
                }
            }
        }


    }
}
