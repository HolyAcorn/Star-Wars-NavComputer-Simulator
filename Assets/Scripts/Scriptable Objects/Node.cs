using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public enum NodeType
    {
        Planet,
        Point
    }

    public abstract class Node : ScriptableObject, IComparable<Node>
    {
        public int CoordX { get; private set; }
        public int CoordY { get; private set; }

        public List<string> HyperlaneRoutes = new List<string>();
        public List<int> IndexInHyperLane = new List<int>();
        public List<Path> shortestPath;
        public List<Neighbour> neighbours = new List<Neighbour>();
        public GameObject gameObject;
        public NodeType type;

        public virtual void Initialize(string name, int coordX, int coordY, int type = 0)
        {
            this.name = name;
            CoordX = coordX;
            CoordY = coordY;
            this.type = (NodeType)type;
        }


        public virtual int CompareTo(Node other, int whichHyperlane)
        {
            if (IndexInHyperLane[whichHyperlane] > other.IndexInHyperLane[whichHyperlane]) return 1;
            else if (IndexInHyperLane[whichHyperlane] < other.IndexInHyperLane[whichHyperlane]) return -1;
            else return 0;
        }


        public virtual int CompareTo(Node other)
        {
            throw new NotImplementedException();
        }

        public virtual void SetHyperLaneRoute(string hyperName, int Index)
        {
            HyperlaneRoutes.Add(hyperName);
            IndexInHyperLane.Add(Index);
        }

        public class Neighbour
        {
            public Node Node;
            public float Distance;
            public bool subLightNeighbour { get; private set; }

            public Neighbour(Node previousNode, Node planet)
            {
                Node = previousNode;
                Vector2 pNodeCoords = new Vector2(previousNode.CoordX, previousNode.CoordY);
                Vector2 planetCoords = new Vector2(planet.CoordX, planet.CoordY);
                Distance = Vector2.Distance(pNodeCoords, planetCoords);
                subLightNeighbour = false;
            }

            public Neighbour(Node previousNode, Node planet, float subLightMultiplier)
            {
                Node = previousNode;
                Vector2 pNodeCoords = new Vector2(previousNode.CoordX, previousNode.CoordY);
                Vector2 planetCoords = new Vector2(planet.CoordX, planet.CoordY);
                Distance = Vector2.Distance(pNodeCoords, planetCoords) * subLightMultiplier;
                subLightNeighbour = true;
            }
        }
    }
}
