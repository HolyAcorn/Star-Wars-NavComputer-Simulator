using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class HyperLaneLine : MonoBehaviour
{
    public StyleSettings styleSetting;
    public static List<string> hyperList;

    private HyperLane hyperLane;

    public HyperLaneList hyperLaneList;


    public PlanetList planetList;
    private Vector2[] points;

    private LineRenderer Line;
    private List<LineRenderer> Lines = new List<LineRenderer>();
    public GameObject hyperLanePrefab;

    public Vector3[] InitialState;

    public float SmoothingLength = 2f;
    public int SmoothingSections = 10;

    public FloatReference majorLineWidth;
    public FloatReference minorLineWidth;

    public FloatReference sizeDifference;


    public void GenerateHyperLanes()
    {
        
        for (int h = 0; h < hyperLaneList.Hyperlanes.Count; h++)
        {
            hyperLane = hyperLaneList.Hyperlanes[h];
            GameObject instance= Instantiate(hyperLanePrefab);
            instance.name = hyperLane.name;
            Line = instance.GetComponent<LineRenderer>();
            
            Line.startColor = styleSetting.HyperLaneColor;
            Line.endColor = styleSetting.HyperLaneColor;
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
            instance.transform.parent = transform;
            Lines.Add(Line);
        }

    }



    private Vector2[] GetHyperLanePoints(HyperLane hyperLane)
    {
        Vector2[] points = new Vector2[hyperLane.Points.Count];
        for (int p = 0; p < hyperLane.Points.Count; p++)
        {
            HyperLanePoint point = hyperLane.Points[p];
            points[p] = point.Position / sizeDifference.Value;
        }
        return points;


        
    }



    private Vector2[] PlanetToTPoints(Planet[] planets)
    {
        Vector2[] points = new Vector2[planets.Length];

        for (int p = 0; p < planets.Length; p++)
        {
            Planet planet = planets[p];
            points[p] = new Vector2(planet.CoordX, planet.CoordY) / sizeDifference.Value;
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
