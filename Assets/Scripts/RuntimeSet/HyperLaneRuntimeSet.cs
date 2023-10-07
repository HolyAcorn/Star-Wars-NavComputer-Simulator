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
            HyperLane masterHyperLane = ScriptableObject.CreateInstance<HyperLane>();
            NodeRuntimeSet planetRuntimeSet = ScriptableObject.CreateInstance<NodeRuntimeSet>();

            foreach (HyperLane hyperLane in items)
            {
                foreach (Node planet in hyperLane.Nodes.items)
                {
                    planetRuntimeSet.AddNode(planet);
                }
            }
            masterHyperLane.Initialize("HyperLaneMasterList", planetRuntimeSet);
            return masterHyperLane;
        }
    }
}
