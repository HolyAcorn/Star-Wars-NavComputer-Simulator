using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    [CreateAssetMenu(fileName = "HyperLane RuntimeSet", menuName = "Scriptable Objects/RuntimeSets/HyperLane RuntimeSet")]
    public class HyperLaneRuntimeSet : RuntimeSet<HyperLane>
    {
        public HyperLane CreateHyperLanePath()
        {
            
            PlanetRuntimeSet planetRuntimeSet = ScriptableObject.CreateInstance<PlanetRuntimeSet>();

            foreach (HyperLane hyperLane in items)
            {
                foreach (Planet planet in hyperLane.Planets)
                {
                    planetRuntimeSet.AddPlanet(planet);
                }
            }
            HyperLane masterHyperLane = new HyperLane("HyperLaneMasterList", planetRuntimeSet);
            return masterHyperLane;
        }
    }
}
