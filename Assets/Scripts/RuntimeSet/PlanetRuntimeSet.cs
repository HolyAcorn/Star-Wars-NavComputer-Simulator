using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    [CreateAssetMenu(fileName = "Planet RuntimeSet", menuName = "Scriptable Objects/RuntimeSets/Planet RuntimeSet")]
    public class PlanetRuntimeSet : RuntimeSet<Planet>
    {
        public void AddPlanet(Planet planet)
        {
            bool shouldAddPlanet = true;
            for (int i = 0; i < Count(); i++)
            {
                if (Get(i) == planet)
                {
                    shouldAddPlanet = false;
                    break;
                }
            }
            if (shouldAddPlanet)
            {
                Add(planet);
            }
        }
    }
}
