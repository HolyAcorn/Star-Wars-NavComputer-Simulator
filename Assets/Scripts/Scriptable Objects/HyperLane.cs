using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SwNavComp
{

    [CreateAssetMenu(fileName = "Hyperlane", menuName = "Scriptable Objects/Hyperlane/Hyperlane")]
    public class HyperLane : ScriptableObject
    {

        public PlanetRuntimeSet Planets { get; private set; }

        public TypeEnum Type = new TypeEnum();


        public void SetType(int index)
        {
            if (index == 0)
            {
                Type = TypeEnum.Major;

            }
            else
            {
                Type = TypeEnum.Minor;
            }
        }




        public void Initialize(string hyperLaneName, PlanetRuntimeSet planetList)
        {
            name = hyperLaneName;
            Planets = ScriptableObject.CreateInstance<PlanetRuntimeSet>();
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