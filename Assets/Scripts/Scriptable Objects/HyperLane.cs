using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Hyperlane", menuName = "Scriptable Objects/Hyperlane/Hyperlane")]
public class HyperLane : ScriptableObject
{
    public List<HyperLanePoint> Points;


    public TypeEnum Type = new TypeEnum();
    public enum TypeEnum
    {
        Major,
        Minor
    }

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


    public void AddPoint(Planet planet, Vector2 position)
    {
        HyperLanePoint hyperLanePoint = ScriptableObject.CreateInstance<HyperLanePoint>();
        hyperLanePoint.name = planet.name;
        hyperLanePoint.Planet = planet;
        hyperLanePoint.Position = position;

        string dirPath = "Assets/ScriptableObjects/HyperLanes/" + name + "/";
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        //AssetDatabase.CreateAsset(hyperLanePoint, dirPath + hyperLanePoint.name + ".asset");
        Points.Add(hyperLanePoint);
    }

    public void SetupNeighbours()
    {
        for (int p = 1; p < Points.Count; p++)
        {
            HyperLanePoint point1 = Points[p - 1];
            HyperLanePoint point2 = Points[p];
            float distance = Vector2.Distance(point1.Position, point2.Position);
            HyperLanePoint.Neighbour neighbour = new HyperLanePoint.Neighbour();
            neighbour.Point = point2;
            neighbour.Distance = distance;
            point1.Neighbours.Add(neighbour);
        }
    }



}
        