using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlanetInfo : MonoBehaviour
{/*
    public PlanetList activePlanet;

    public PlanetList targetPlanet;

    public PlanetList startingPlanet;
    public GameEvent calculatePath;

    private Text planetName;
    private Transform child;

    private void Start()
    {
        child = transform.GetChild(1);
        for (int c = 0; c < child.childCount; c++)
        {

            if (child.GetChild(c).name == "PlanetNameContainer")
            {
                Transform container = child.GetChild(c);
                for (int i = 0; i < container.childCount; i++)
                {
                    if (container.GetChild(i).name == "PlanetName")
                    {
                        planetName = container.GetChild(i).GetComponent<Text>();
                    }
                }
            }
        }


    }


    private void Update()
    {
        if (activePlanet.Planets.Count > 0)
        {
            planetName.text = activePlanet.Planets[0].name;
        }
        else
        {
            planetName.text = "";
        }
    }

    public void SetTargetLocation()
    {
        targetPlanet.ClearList();
        targetPlanet.AddPlanet(activePlanet.Planets[0]);
        if (startingPlanet.Planets.Count > 0)
        {
            if (calculatePath != null)
            {
                calculatePath.Raise();
            }
        }
    }

    public void SetStartingLocation()
    {

        startingPlanet.ClearList();
        startingPlanet.AddPlanet(activePlanet.Planets[0]);

    }

    public void SetVisibility(bool visible)
    {
        transform.GetChild(0).gameObject.SetActive(visible);
        child.gameObject.SetActive(visible);
    }*/
}
