using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        public void ChangeJsonFile(bool add)
        {
            string finalDirPath = Application.dataPath + dirPath;
            string[] files = JsonConverter.GetFilesFromDirectory(finalDirPath, dataLocation);
            string value = "\t" + parameterName + ": " + defaultValue + ",\n";
            for (int f = 0; f < files.Length; f++)
            {
                string file = files[f];
                Debug.Log(file);
                string jsonFile = File.ReadAllText(file);
                Regex regex = new Regex(regexPattern);
                MatchCollection matches = regex.Matches(jsonFile);
                for (int m = 0; m < matches.Count; m++)
                {
                    string newText = "";
                    if (add) newText = matches[m].Value + value;
                    jsonFile = jsonFile.Replace(matches[m].Value, newText);
                }
                JsonConverter.SaveFileToJson(file, jsonFile);
            }
            void CreateIDs(string jsonFile)
            {
                string regPattern = "\"Name\": \"(.*)\",";
                Regex regex = new Regex(regPattern);
                MatchCollection matches = regex.Matches(jsonFile);
                Debug.Log(matches[0].Groups[0].Value);

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
