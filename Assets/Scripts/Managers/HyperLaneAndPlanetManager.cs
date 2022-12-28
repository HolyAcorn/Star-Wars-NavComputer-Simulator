using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SwNavComp
{
    public class HyperLaneAndPlanetManager : MonoBehaviour
    {

        private string dirPath;
        public StringReference dataLocation;

        public HyperLaneRuntimeSet hyperLaneList;
        public PlanetRuntimeSet planetList;
        [SerializeField] GameEvent presentHyperLanes;

        public void CreateHyperlanes()
        {
            hyperLaneList.Clear();
            this.planetList.Clear();
            dirPath = Application.dataPath + "/StreamingAssets/Data/";
            PlanetRuntimeSet planetList = ScriptableObject.CreateInstance<PlanetRuntimeSet>();

            string[] files = JsonConverter.GetFilesFromDirectory(dirPath, dataLocation.Value);

            for (int f = 0; f < files.Length; f++)
            {
                string file = files[f];
                string json = File.ReadAllText(file);
                PlanetRuntimeSet tempPlanets = JsonConverter.LoadJsonToList(dataLocation.Value, json, file, planetList);
                foreach (Planet planet in tempPlanets.items)
                {
                    planetList.Add(planet);
                }
                for (int i = 0; i < planetList.Count(); i++)
                {
                    this.planetList.AddPlanet(planetList.Get(i));
                }
                string regex = dataLocation.Value + @"/(.+).json";
                string result = Regex.Match(file, regex).Groups[1].Value;

                HyperLane hyperLane = ScriptableObject.CreateInstance<HyperLane>();
                hyperLane.Initialize(result, planetList);
                hyperLane.SetType(JsonConverter.GetType(json));
                hyperLane = SortPlanets(hyperLane);

                for (int p = 1; p < hyperLane.Planets.Count(); p++)
                {
                    Planet planet = hyperLane.Planets.Get(p);
                    Planet previousPlanet = hyperLane.Planets.Get(p - 1);
                    Planet.Neighbour neighbour = new Planet.Neighbour(previousPlanet, planet);
                    Planet.Neighbour previousNeighbour = new Planet.Neighbour(planet, previousPlanet);
                    planet.neighbours.Add(neighbour);
                    previousPlanet.neighbours.Add(previousNeighbour);
                }

                hyperLaneList.Add(hyperLane);
                

            }
            Debug.Log("Done");
            presentHyperLanes.Raise();
        }


        private HyperLane SortPlanets(HyperLane hyperLane)
        {
            bool itemMoved = false;
            do
            {
                itemMoved = false;
                for (int i = 0; i < hyperLane.Planets.Count() - 1; i++)
                {
                    Planet planetA = hyperLane.Planets.Get(i);
                    Planet planetB = hyperLane.Planets.Get(i+1);
                    int indexA = GetIndex(planetA);
                    int indexB = GetIndex(planetB);
                    if (planetA.IndexInHyperLane[indexA] > planetB.IndexInHyperLane[indexB])
                    {
                        Planet lowerPlanet = hyperLane.Planets.Get(i + 1);
                        hyperLane.Planets.items[i + 1] = hyperLane.Planets.Get(i);
                        hyperLane.Planets.items[i] = lowerPlanet;
                        itemMoved = true;

                    }
                }
            } while (itemMoved);
            

            return hyperLane;

            int GetIndex(Planet planet)
            {
                int index = 0;
                for (int h = 0; h < planet.HyperlaneRoutes.Count; h++) if (planet.HyperlaneRoutes[h] == hyperLane.name) return h;
                return index;
            }
        }


        
    }
}

