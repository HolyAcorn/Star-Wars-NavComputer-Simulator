using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace SwNavComp
{

    
    public class HyperLane
    {

        public List<Planet> Planets { get; private set; }

        public TypeEnum Type = new TypeEnum();
        public string name { get; private set; }

        public void SetType(int index)
        {
            if (index == -1) Type = TypeEnum.Major;
            else if (index == 0) Type = TypeEnum.Medium;
            else Type = TypeEnum.Minor;

        }




        public HyperLane(string hyperLaneName, PlanetRuntimeSet planetList, bool noHyperlanes = false)
        {
            name = hyperLaneName;
            Planets = new List<Planet>();
            if (noHyperlanes)
            {
                foreach (Planet planet in planetList.items)
                {
                    if (planet.displayName == "test") {
                        Debug.Log("");

                    }
                    Planets.Add(planet);
                }
                return;
            }
            for (int p = 0; p < planetList.Count(); p++)
            {
                Planet planet = planetList.Get(p);
                for (int h = 0; h < planet.HyperlaneRoutes.Count; h++)
                {
                    if ((planet.HyperlaneRoutes[h] != "" && name.ToLower() == planet.HyperlaneRoutes[h].ToLower()) || name == "HyperLaneMasterList")
                    {
                        Planets.Add(planet);
                        //planet.SetHyperLaneRoute(name, Planets.items.IndexOf(planet));
                    }

                }
            }
        }

        public JsonPlanetFile CreateJsonObject()
        {
            JsonPlanetFile jsonPlanetFile = new JsonPlanetFile
            {
                Type = (int)Type,
            };

            List<JsonPlanets> planets = new List<JsonPlanets>();

            foreach (Planet planet in Planets)
            {
                planets.Add(planet.CreateJsonObject());
            }

            jsonPlanetFile.JsonPlanets = planets.ToArray();
            return jsonPlanetFile;
        }

    }
}