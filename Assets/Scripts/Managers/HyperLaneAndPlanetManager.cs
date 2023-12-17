using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace SwNavComp
{
    public class HyperLaneAndPlanetManager : MonoBehaviour
    {

        private string dirPath;
        public StringReference dataLocation;

        public HyperLaneRuntimeSet hyperLaneList;
        public PlanetRuntimeSet noHyperlanePlanets;
        public PlanetRuntimeSet planetList;
        [SerializeField] GameEvent presentHyperLanes;


        [SerializeField] FloatReference currentProgress;
        float maxProgress;
        [SerializeField] StringReference progressText;

        PlanetRuntimeSet tempPlanetList;


        [Header("Dev Tools")]
        [SerializeField] bool DEVtoggleSubLight = true;

        private void Start()
        {
            dirPath = Application.dataPath + "/StreamingAssets/Data/";

            tempPlanetList = ScriptableObject.CreateInstance<PlanetRuntimeSet>();
        }

        public async Task CreateHyperlanes()
        {
            hyperLaneList.Clear();
            this.planetList.Clear();
            noHyperlanePlanets.Clear();
            

            string[] files = JsonConverter.GetFilesFromDirectory(dirPath, dataLocation.Value);
            maxProgress = files.Length;
            progressText.Variable.Value = new string("Loading Hyperlanes");
            for (int f = 0; f < files.Length; f++)
            {
                currentProgress.Variable.Value = (f + 1) / maxProgress;
                string file = files[f];
                string jsonFile = File.ReadAllText(file);
             
                List<Planet> tempPlanets = JsonConverter.LoadJsonToPlanetList(dataLocation.Value, jsonFile, file, tempPlanetList);
                

                foreach (Planet planet in tempPlanets)
                {
                    tempPlanetList.Add(planet);
                    if (file.Contains("No Hyperlane")) noHyperlanePlanets.Add(planet);
                }
                for (int i = 0; i < tempPlanetList.Count(); i++)
                {
                    this.planetList.AddPlanet(tempPlanetList.Get(i));
                }
                string regex = dataLocation.Value + @"/(.+).json";
                string result = Regex.Match(file, regex).Groups[1].Value;

                if(result != "No Hyperlane")
                {
                    HyperLane hyperLane = new HyperLane(result, tempPlanetList);
                    
                    hyperLane.SetType(JsonConverter.GetTypeJsonFile(jsonFile));
                    hyperLane = SortPlanets(hyperLane).Result;

                    for (int p = 1; p < hyperLane.Planets.Count; p++)
                    {
                        Planet planet = hyperLane.Planets[p];
                        Planet previousPlanet = hyperLane.Planets[p - 1];
                        Planet.Neighbour neighbour = new Planet.Neighbour(previousPlanet, planet);
                        Planet.Neighbour previousNeighbour = new Planet.Neighbour(planet, previousPlanet);
                        planet.neighbours.Add(neighbour);
                        previousPlanet.neighbours.Add(previousNeighbour);
                    }

                    hyperLaneList.Add(hyperLane);
                }
                else
                {
                    
                }
            }


            if(DEVtoggleSubLight) await Task.Run(() => ConnectAllPlanetsSublight());

            Debug.Log("Done");
            //presentHyperLanes.Raise();
            
        }

        private async void ConnectAllPlanetsSublight()
        {
            maxProgress = planetList.Count();
            progressText.Variable.Value = new string("Connecting Sublight Planets");

            for (int i = 0; i < planetList.Count(); i++)
            {
                Planet planet = planetList.Get(i);
                currentProgress.Variable.Value = (i + 1) / maxProgress;

                for (int y = 0; y < planetList.Count(); y++)
                {
                    Planet otherPlanet = planetList.Get(y);
                    bool areAllreadyNeighbours = false;
                    foreach (Planet.Neighbour neighbour in planet.neighbours)
                    {
                        if (otherPlanet.displayName == neighbour.Planet.displayName) areAllreadyNeighbours = true;
                    }
                    if (!areAllreadyNeighbours)
                    {
                        Planet.Neighbour neighbour = new Planet.Neighbour(otherPlanet, planet, 10);
                        Planet.Neighbour otherNeighbour = new Planet.Neighbour(planet, otherPlanet, 10);
                        planet.neighbours.Add(neighbour);
                        otherPlanet.neighbours.Add(otherNeighbour);
                    }
                }

            }
        }


        private async Task<HyperLane> SortPlanets(HyperLane hyperLane)
        {
            bool itemMoved = false;
            do
            {
                itemMoved = false;
                for (int i = 0; i < hyperLane.Planets.Count - 1; i++)
                {
                    Planet planetA = hyperLane.Planets[i];
                    Planet planetB = hyperLane.Planets[i+1];
                    int indexA = GetIndex(planetA).Result;
                    int indexB = GetIndex(planetB).Result;
                    if (planetA.IndexInHyperLane[indexA] > planetB.IndexInHyperLane[indexB])
                    {
                        Planet lowerPlanet = hyperLane.Planets[i + 1];
                        hyperLane.Planets[i + 1] = hyperLane.Planets[i];
                        hyperLane.Planets[i] = lowerPlanet;
                        itemMoved = true;

                    }
                }
            } while (itemMoved);
            

            return hyperLane;

            async Task<int> GetIndex(Planet planet)
            {
                int index = 0;
                for (int h = 0; h < planet.HyperlaneRoutes.Count; h++) if (planet.HyperlaneRoutes[h] == hyperLane.name) return h;
                return index;
            }
        }


        
    }
}

