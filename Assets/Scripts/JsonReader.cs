using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
//using UnityEditor;


public class JsonReader : MonoBehaviour
{
    public PlanetList PlanetList;
    public HyperLaneList HyperLaneList;
    public StringVariable progressDescription;
    public FloatVariable progress;
    public GameEvent jsonReadDone;

    private string dirPath;
    public StringReference dataLocation;
    private string[] Files;
    private string fileName;


    public void CreatePlanetsAndHyperLanes()
    {
        HyperLaneList.Hyperlanes.Clear();
        dirPath = Application.dataPath + "/StreamingAssets/Data/";
        progressDescription.Value = "Clearing Data";
        for (int h = 0; h < HyperLaneList.Hyperlanes.Count; h++)
        {
            HyperLane hyperLane1 = HyperLaneList.Hyperlanes[h];
            progress.Value = Mathf.Lerp(0, HyperLaneList.Hyperlanes.Count, h);
            if (hyperLane1.Points.Count > 0)
            {
                hyperLane1.Points.Clear();
            }
        }
        HyperLaneList.Hyperlanes.Clear();
        PlanetList.ClearList();
        List<Planet> planetList = new List<Planet>();
        //Files = Resources.LoadAll<string>("Data");
        Files = GetFiles();
        for (int f = 0; f < Files.Length; f++)
        {
            progressDescription.Value = "Creating hyperlane: " + Files[f];
            progress.Value = Mathf.Lerp(0, Files.Length, f);
            string file = Files[f];
            fileName = file;
            string json = File.ReadAllText(file);
            List<Planet> tempList = LoadJson(json, file, planetList);
            foreach (Planet planet in tempList)
            {
                planetList.Add(planet);
            }
            List<string> hyperNames = new List<string>();
            for (int i = 0; i < planetList.Count; i++)
            {
                //if (!PlanetList.Contains(planetList[i]))
                //{
                PlanetList.AddPlanet(planetList[i]);
                //}
            }
            JsonFile jsonFile = JsonUtility.FromJson<JsonFile>(json);
            string regex = @"\/Data\/(.+).json";
            string result = Regex.Match(file, regex).Groups[1].Value;
            HyperLane hyperLane = CreateHyperLane(result, planetList);
            hyperLane.SetType(jsonFile.Type);
            hyperLane.SetupNeighbours();
            string dirPath = "Assets/ScriptableObjects/HyperLanes/";
            //AssetDatabase.CreateAsset(hyperLane, dirPath + hyperLane.name + ".asset");
        }

        progressDescription.Value = "Creating Assets...";
        for (int i = 0; i < planetList.Count; i++)
        {
            progress.Value = Mathf.Lerp(0, planetList.Count, i);
            //AssetDatabase.CreateAsset(planetList[i], "Assets/ScriptableObjects/Planets/" + planetList[i].name + ".asset");

        }

        progressDescription.Value = "Removing duplicates...";
        for (int i = PlanetList.Planets.Count-1; i >= 0; i--)
        {
            progress.Value = Mathf.Lerp(PlanetList.Planets.Count, 0, i);
            Planet planet1 = PlanetList.Planets[i];
            for (int p = PlanetList.Planets.Count-1; p >= 0; p--)
            {
                Planet planet2 = PlanetList.Planets[p];
                if (planet2.name == planet1.name && planet2 != planet1)
                {
                    PlanetList.RemovePlanet(planet2);
                }

            }
        }
        jsonReadDone.Raise();
    }

    private string[] GetFiles()
    {
        string dirPath = this.dirPath + dataLocation.Value + "/";
        List<string> FileNames = new List<string>();
        Files = Directory.GetFiles(dirPath);
        foreach (string file in Files)
        {
            FileNames.Add(file);
        }
        for (int i = 0; i < FileNames.Count; i++)
        {
            if (FileNames[i].Contains(".meta"))
            {
                FileNames.Remove(FileNames[i]);
                i = 0;
            }
        }
        return FileNames.ToArray();
    }
    public List<Planet> LoadJson(string json, string fileName, List<Planet> planetList)
    {
        JsonFile planets = JsonUtility.FromJson<JsonFile>(json);
        string regex = @"\/Data\/(.+).json";
        string result = Regex.Match(fileName, regex).Groups[1].Value;
        List<Planet> planetListNew = new List<Planet>();

        for (int i = 0; i < planets.JsonPlanets.Length; i++)
        {
            JsonPlanets jsonPlanet = planets.JsonPlanets[i];
            bool shouldCreateNew = true;

            if (shouldCreateNew)
            {
                Planet planet = ScriptableObject.CreateInstance<Planet>();
                planet.name = jsonPlanet.Name;
                planet.Name = jsonPlanet.Name;
                planet.CoordX = jsonPlanet.CoordX;
                planet.CoordY = jsonPlanet.CoordY;
                planet.HyperlaneRoutes = new List<string>();
                planet.HyperlaneRoutes.Add(result);
                planetList.Add(planet);
            }
            /*foreach (Planet planet1 in planetList)
            {
                if (planet1.name == jsonPlanet.Name)
                {
                    planet1.HyperlaneRoutes.Add(result);
                    shouldCreateNew = false;
                }
            }*/

        }
        return planetListNew;
    }

    public HyperLane CreateHyperLane(string HyperLaneName, List<Planet> planetList)
    {
        HyperLane hyperLane = ScriptableObject.CreateInstance<HyperLane>();
        hyperLane.name = HyperLaneName;
        
        hyperLane.Points = new List<HyperLanePoint>();
        for (int p = 0; p < planetList.Count; p++)
        {
            Planet planet = planetList[p];
            for (int h = 0; h < planet.HyperlaneRoutes.Count; h++)
            {

                if (planet.HyperlaneRoutes[h] != "" && hyperLane.name.ToLower() == planet.HyperlaneRoutes[h].ToLower())
                {
                    hyperLane.AddPoint(planet, new Vector2(planet.CoordX, planet.CoordY));
                }
            }

        }
            HyperLaneList.Hyperlanes.Add(hyperLane);
        
        return hyperLane;
    }
}

[Serializable]
public class JsonFile
{
    public JsonPlanets[] JsonPlanets;
    public int Type;
}
[Serializable]
public class JsonPlanets
{
    public string Name;
    public int CoordX;
    public int CoordY;
}


