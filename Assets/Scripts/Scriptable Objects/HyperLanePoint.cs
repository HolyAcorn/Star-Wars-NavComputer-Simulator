using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TravelTime;

[CreateAssetMenu(fileName ="Hyperlane Point", menuName ="Scriptable Objects/Hyperlane/Hyperlane Point")]
public class HyperLanePoint : ScriptableObject
{
    public class Neighbour
    {
        public HyperLanePoint Point;
        public float Distance;
    }

    public Planet Planet;
    public Vector2 Position;
    public List<Neighbour> Neighbours = new List<Neighbour>();
    public List<Path> ShortestPath = new List<Path>();
}
