using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace SwNavComp
{
    public static class JsonConverter
    {

        public static string[] GetFilesFromDirectory(string dirPath, string dataLocation)
        {
            string _dirPath = dirPath + dataLocation + "/";
            List<string> FileNames = new List<string>();
            string[] files = Directory.GetFiles(_dirPath);
            foreach (string file in files)
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

        public static PlanetRuntimeSet LoadJsonToPlanetRuntimeSet(string dataLocation, string jsonFile, string fileName, PlanetRuntimeSet planetList)
        {
            JsonFile planets = JsonUtility.FromJson<JsonFile>(jsonFile);
            string regex = dataLocation + @"/(.+).json";
            string result = Regex.Match(fileName, regex).Groups[1].Value;
            PlanetRuntimeSet newPlanetList = ScriptableObject.CreateInstance<PlanetRuntimeSet>();

            for (int i = 0; i < planets.JsonPlanets.Length; i++)
            {
                JsonPlanets jsonPlanet = planets.JsonPlanets[i];
                bool shouldCreateNew = true;
                foreach (Planet planet1 in planetList.items)
                {
                    if (planet1.name == jsonPlanet.Name)
                    {
                        planet1.HyperlaneRoutes.Add(result);
                        planet1.IndexInHyperLane.Add(i);
                        shouldCreateNew = false;
                    }
                }
                if (shouldCreateNew)
                {
                    Planet planet = ScriptableObject.CreateInstance<Planet>();
                    planet.Initialize(jsonPlanet.Name, jsonPlanet.CoordX, jsonPlanet.CoordY);
                    planet.HyperlaneRoutes.Add(result);
                    planet.IndexInHyperLane.Add(i);
                    planetList.Add(planet);
                    newPlanetList.Add(planet);
                }
            }
            return newPlanetList;
        }


        public static int GetType(string json)
        {
            JsonFile jsonFile = JsonUtility.FromJson<JsonFile>(json);
            return jsonFile.Type;
        }

        public static string ConvertToJson(object t)
        {
            return JsonUtility.ToJson(t);
        }

        


        public static void SaveFileToJson(string fileName, string json, string savePath, bool shouldOverwrite = true)
        {
            string finalSavePath = savePath + fileName + ".json";
            if(!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
            if (File.Exists(finalSavePath) && shouldOverwrite) Debug.Log("Overwritten file");
            else if (!shouldOverwrite && File.Exists(finalSavePath)) return;

            File.WriteAllText(finalSavePath, json);
        }
    }
}
