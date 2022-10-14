using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Planet", menuName = "Scriptable Objects/Planet")]
public class Planet : ScriptableObject, IComparable<Planet>
{
    public string Name;
    public int CoordX;
    public int CoordY;

    public List<string> HyperlaneRoutes;
    public List<int> HyperLaneIndex = new List<int>();


    public int CompareTo(Planet other, int whichHyperlane)
    {
        if (HyperLaneIndex[whichHyperlane] > other.HyperLaneIndex[whichHyperlane]) return 1;
        else if (HyperLaneIndex[whichHyperlane] < other.HyperLaneIndex[whichHyperlane]) return -1;
        else return 0;
    }
    

    public int CompareTo(Planet other)
    {
        throw new NotImplementedException();
    }
}
