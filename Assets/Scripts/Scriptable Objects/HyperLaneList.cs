using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hyperlane List", menuName = "Scriptable Objects/Hyperlane/Hyperlane List")]
public class HyperLaneList : ScriptableObject
{
    public List<HyperLane> Hyperlanes;


    public HyperLane CreateMasterPointList()
    {
        HyperLane HyperLane = ScriptableObject.CreateInstance<HyperLane>();
        HyperLane.Points = new List<HyperLanePoint>();
        foreach (HyperLane hyperLane in Hyperlanes)
        {
            foreach (HyperLanePoint point in hyperLane.Points)
            {
                if (!HyperLane.Points.Contains(point))
                {
                    HyperLane.Points.Add(point);
                }
            }
        }
        return HyperLane;
    }
}
