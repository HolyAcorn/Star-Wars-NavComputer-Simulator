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
            JsonPlanetFile planets = JsonUtility.FromJson<JsonPlanetFile>(jsonFile);
            string regex = dataLocation + @"/(.+).json";
            string result = Regex.Match(fileName, regex).Groups[1].Value;
            PlanetRuntimeSet newPlanetList = ScriptableObject.CreateInstance<PlanetRuntimeSet>();

            if (fileName.Contains("Rainboh"))
            {
                Debug.Log("test");
            }

            for (int i = 0; i < planets.JsonPlanets.Length; i++)
            {
                JsonPlanets jsonPlanet = planets.JsonPlanets[i];
                bool shouldCreateNew = true;
                foreach (Planet planet1 in planetList.items)
                {
                    if (planet1.name == jsonPlanet.Name)
                    {
                        if (planet1.name == "Unnamed" && (planet1.CoordX != jsonPlanet.CoordX || planet1.CoordY != jsonPlanet.CoordY)) continue;
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


        public static int GetTypeJsonFile(string json)
        {
            JsonPlanetFile jsonFile = JsonUtility.FromJson<JsonPlanetFile>(json);
            return jsonFile.Type;
        }

        public static string ConvertToJson(object t)
        {
            return JsonUtility.ToJson(t);
        }

        public static void SaveFileToJson(string fileName, string json, string savePath, bool shouldOverwrite = true)
        {
            string finalSavePath = savePath + fileName + ".json";
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
            if (File.Exists(finalSavePath) && shouldOverwrite) Debug.Log("Overwritten file" + finalSavePath);
            else if (!shouldOverwrite && File.Exists(finalSavePath)) return;

            File.WriteAllText(finalSavePath, json);
        }

        public static void SaveFileToJson(string path, string json, bool shouldOverwrite = true)
        {
            if (File.Exists(path) && shouldOverwrite) Debug.Log("Overwritten file: " + path);
            else if (!shouldOverwrite && File.Exists(path)) return;

            File.WriteAllText(path, json);
        }

        public static StarshipProfile LoadFromJsonStarShip(string jsonFile)
        {
            StarshipProfile profile = ScriptableObject.CreateInstance<StarshipProfile>();
            JsonStarshipFile starshipJson = JsonUtility.FromJson<JsonStarshipFile>(jsonFile);

            profile.Name = starshipJson.StarshipJsonObject.Name;
            profile.Model = starshipJson.StarshipJsonObject.Model;
            profile.Type = starshipJson.StarshipJsonObject.Type;
            profile.Description = starshipJson.StarshipJsonObject.Description;

            profile.Crew = starshipJson.StarshipJsonObject.Crew;
            profile.Passengers = starshipJson.StarshipJsonObject.Passengers;

            profile.HyperDriveType = starshipJson.StarshipJsonObject.HyperDriveType;
            profile.HyperdriveRating = starshipJson.StarshipJsonObject.HyperdriveRating;
            
            profile.Silhouette = starshipJson.StarshipJsonObject.Silhouette;
            profile.Speed = starshipJson.StarshipJsonObject.Speed;
            profile.Handling = starshipJson.StarshipJsonObject.Handling;

            profile.Defense.Fore = starshipJson.StarshipJsonObject.ForeDefense;
            profile.Defense.Aft = starshipJson.StarshipJsonObject.AftDefense;
            profile.Defense.Port = starshipJson.StarshipJsonObject.PortDefense;
            profile.Defense.Starboard = starshipJson.StarshipJsonObject.StarboardDefense;

            profile.Armor = starshipJson.StarshipJsonObject.Armor;
            profile.HullTraumaThreshold = starshipJson.StarshipJsonObject.HullTraumaThreshold;
            profile.HullTrauma = starshipJson.StarshipJsonObject.HullTrauma;

            profile.SystemStrainThreshold = starshipJson.StarshipJsonObject.SystemStrainThreshold;
            profile.SystemStrain = starshipJson.StarshipJsonObject.SystemStrain;

            profile.FuelThreshold = starshipJson.StarshipJsonObject.FuelThreshold;
            profile.Fuel = starshipJson.StarshipJsonObject.Fuel;

            profile.ConsumablesThreshold = starshipJson.StarshipJsonObject.ConsumablesThreshold;
            profile.Consumables = starshipJson.StarshipJsonObject.Consumables;

            return profile;

        }
    }
}
