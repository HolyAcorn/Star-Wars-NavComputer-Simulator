using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlanetInfo : MonoBehaviour
{
    public PlanetList activePlanet;

    public PlanetList targetPlanet;

    public PlanetList startingPlanet;

    private Text planetName;


    private void Start()
    {
        for (int c = 0; c < transform.childCount; c++)
        {
            if (transform.GetChild(c).name == "PlanetNameContainer")
            {
                Transform container = transform.GetChild(c);
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


    public void SetTargetLocation()
    {
        targetPlanet.ClearList();
        targetPlanet.AddPlanet(activePlanet.Planets[0]);
    }

    public void SetStartingLocation()
    {

        startingPlanet.ClearList();
        startingPlanet.AddPlanet(activePlanet.Planets[0]);
    }
}
