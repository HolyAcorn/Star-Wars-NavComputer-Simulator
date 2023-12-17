using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

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




        public HyperLane(string hyperLaneName, PlanetRuntimeSet planetList)
        {
            name = hyperLaneName;
            Planets = new List<Planet>();
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

    }
}