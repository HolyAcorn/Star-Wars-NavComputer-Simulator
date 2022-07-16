using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class HyperLaneLine : MonoBehaviour
{

    public static List<string> hyperList;

    private HyperLane hyperLane;

    public HyperLaneList hyperLaneList;


    public PlanetList planetList;
    private Vector2[] points;

    private LineRenderer Line;
    private GameObject hyperLaneObject;

    public Vector3[] InitialState;

    public float SmoothingLength = 2f;
    public int SmoothingSections = 10;

    public FloatReference majorLineWidth;
    public FloatReference minorLineWidth;

    private void Start()
    {
        if (!Line)
        {
            Line = GetComponent<LineRenderer>();
        }
        for (int h = 0; h < hyperLaneList.Hyperlanes.Count; h++)
        {
            hyperLane = hyperLaneList.Hyperlanes[h];
            hyperLaneObject = new GameObject();
            hyperLaneObject.name = hyperLane.name;
            Line = hyperLaneObject.AddComponent<LineRenderer>();
            switch (hyperLane.Type)
            {
                case HyperLane.TypeEnum.Major:
                    Line.widthMultiplier = majorLineWidth.Value;
                    break;
                case HyperLane.TypeEnum.Minor:
                    Line.widthMultiplier = minorLineWidth.Value;
                    break;
            }
            points = GetHyperLanePoints(hyperLane);
            Line.positionCount = points.Length;

            for (int i = 0; i < points.Length; i++)
            {
                Line.SetPosition(i, points[i]);
            }
        }

    }



    private Vector2[] GetHyperLanePoints(HyperLane hyperLane)
    {
        Vector2[] points = new Vector2[hyperLane.Points.Count];
        for (int p = 0; p < hyperLane.Points.Count; p++)
        {
            HyperLanePoint point = hyperLane.Points[p];
            points[p] = point.Position / 10;
        }
        return points;


        
    }



    private Vector2[] PlanetToTPoints(Planet[] planets)
    {
        Vector2[] points = new Vector2[planets.Length];

        for (int p = 0; p < planets.Length; p++)
        {
            Planet planet = planets[p];
            points[p] = new Vector2(planet.CoordX, planet.CoordY) / 10;
        }

        return points;

        
    }

    private Planet[] GetHyperlanePlanets(HyperLane hyperLane)
    {
        Planet[] result;
        List<Planet> list = new List<Planet>();
        for (int i = 0; i < planetList.Planets.Count; i++)
        {
            Planet planet = planetList.Planets[i];
            for (int h = 0; h < planet.HyperlaneRoutes.Count; h++)
            {
               
                if (planet.HyperlaneRoutes[h].ToLower() == hyperLane.name.ToLower())
                {
                    list.Add(planet);
                }
            }
        }
        result = list.ToArray();

        return result;
    }

}
