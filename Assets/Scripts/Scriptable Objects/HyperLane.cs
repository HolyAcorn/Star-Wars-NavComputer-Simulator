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

        public NodeRuntimeSet Nodes { get; private set; }

        public TypeEnum Type = new TypeEnum();


        public void SetType(int index)
        {
            if (index == -1) Type = TypeEnum.Major;
            else if (index == 0) Type = TypeEnum.Medium;
            else Type = TypeEnum.Minor;

        }




        public void Initialize(string hyperLaneName, NodeRuntimeSet planetList)
        {
            name = hyperLaneName;
            Nodes = ScriptableObject.CreateInstance<NodeRuntimeSet>();
            for (int p = 0; p < planetList.Count(); p++)
            {
                Node planet = planetList.Get(p);
                for (int h = 0; h < planet.HyperlaneRoutes.Count; h++)
                {
                    if ((planet.HyperlaneRoutes[h] != "" && name.ToLower() == planet.HyperlaneRoutes[h].ToLower()) || name == "HyperLaneMasterList")
                    {
                        Nodes.Add(planet);
                        //planet.SetHyperLaneRoute(name, Nodes.items.IndexOf(planet));
                    }

                }
            }
        }

    }
}