using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SwNavComp
{
    public class DEVJsonFileAdderUI : MonoBehaviour
    {
        [SerializeField] private string dirPath;
        [SerializeField] private string dataLocation;

        string parameterName = "";
        string defaultValue = "";
        string regexPattern;

        bool createID = false;

        Dictionary<string, int> planetIDs = new Dictionary<string, int>();
        Dictionary<string, string> planetCoords = new Dictionary<string, string>();

        public void ChangeJsonFile(bool add)
        {
            string finalDirPath = Application.dataPath + dirPath;
            string[] files = JsonConverter.GetFilesFromDirectory(finalDirPath, dataLocation);
            string value = "\t" + parameterName + ": " + defaultValue + ",\n";
            for (int f = 0; f < files.Length; f++)
            {
                string file = files[f];
                string jsonFile = File.ReadAllText(file);
                if (createID) CreateIDs(jsonFile);
                Regex regex = new Regex(regexPattern);
                MatchCollection matches = regex.Matches(jsonFile);
                for (int m = 0; m < matches.Count; m++)
                {
                    if (createID)
                    {
                        value = "\n\t\t\t" + parameterName + ": " + planetIDs[matches[m].Groups[1].Value] + ",";
                    }
                    string newText = "";
                    if (add) newText = matches[m].Value + value;
                    jsonFile = jsonFile.Replace(matches[m].Value, newText);
                }
                JsonConverter.SaveFileToJson(file, jsonFile);
            }

            void CreateIDs(string jsonFile)
            {
                string regPattern = "\"Name\": \"(.*)\",\r\n.*(\"CoordX\": .*,\r\n.*\":.*)";
                Regex regex = new Regex(regPattern);
                MatchCollection matches = regex.Matches(jsonFile);

                Debug.Log(matches[0].Groups[0].Value);
                if (planetIDs.Count == 0) planetIDs.Add(matches[0].Groups[1].Value, 0);
                if (planetCoords.Count == 0) planetCoords.Add(matches[0].Groups[1].Value, matches[0].Groups[2].Value);
                for (int i = 0; i < matches.Count; i++)
                {
                    bool allreadyExists = false;
                    int id = 0;
                    string coords = "";
                    string currentPlanetName = matches[i].Groups[1].Value;
                    string currentPlanetCoords = matches[i].Groups[2].Value;
                    for (int y = 0; y < planetIDs.Count; y++)
                    {
                        if (planetIDs.Keys.ElementAt(y) == currentPlanetName && planetCoords[planetIDs.Keys.ElementAt(y)] == currentPlanetCoords)
                        {
                            allreadyExists = true;
                            id = planetIDs[currentPlanetName];
                            coords = planetCoords[currentPlanetName];
                        }
                    }
                    if (!allreadyExists)
                    {
                        id = planetIDs.Count;
                        planetIDs.Add(currentPlanetName, id);
                        planetCoords.Add(currentPlanetName, currentPlanetCoords);
                    }
                }

            }
        }


        public void SetIDBool(bool value)
        {
            createID = value;
        }

        public void UpdateParameter(string value)
        {
            parameterName = '"' + value + '"';
        }

        public void UpdateDefault(string value)
        {
            defaultValue = value;
        }

        public void UpdateRegex(string value)
        {
            regexPattern = value;
        }
    }
}
