using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Planet List", menuName ="Scriptable Objects/Planet List")]
public class PlanetList : ScriptableObject
{
    public List<Planet> Planets;

    public void AddPlanet(Planet planet)
    {
        bool shouldAddPlanet = true;
        if (Planets.Count == 0)
        {
            Planets = new List<Planet>();
        }
        for (int i = 0; i < Planets.Count; i++)
        {
            if (Planets[i] == planet)
            {
                shouldAddPlanet = false;
                break;
            }
        }
        if (shouldAddPlanet)
        {
            Planets.Add(planet);
        }
    }

    public void RemovePlanet(Planet planet)
    {
        if (Planets.Contains(planet))
        {
            Planets.Remove(planet);
        }
    }

    public void ClearList()
    {
        Planets.Clear();
    }

    public void ReplacePlanet(Planet planet, int index = 0)
    {
        if (index < Planets.Count)
        {
            Planets[index] = planet;

        }
    }


    public bool Contains(Planet planet)
    {
        bool isTrue = false;
        foreach (Planet Planet in Planets)
        {
            if (planet.name == Planet.name)
            {
                isTrue = true;
                foreach (string route in planet.HyperlaneRoutes)
                {
                    if (!Planet.HyperlaneRoutes.Contains(route))
                    {
                        Planet.HyperlaneRoutes.Add(route);
                    }

                }
                break;
            }
        }
        return isTrue;
    }

}
