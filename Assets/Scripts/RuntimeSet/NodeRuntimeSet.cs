using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    [CreateAssetMenu(fileName = "Node RuntimeSet", menuName = "Scriptable Objects/RuntimeSets/Node RuntimeSet")]
    public class NodeRuntimeSet : RuntimeSet<Node>
    {
        public void AddNode(Node planet)
        {
            bool shouldAddNode = true;
            for (int i = 0; i < Count(); i++)
            {
                if (Get(i) == planet)
                {
                    shouldAddNode = false;
                    break;
                }
            }
            if (shouldAddNode)
            {
                Add(planet);
            }
        }
    }
}
