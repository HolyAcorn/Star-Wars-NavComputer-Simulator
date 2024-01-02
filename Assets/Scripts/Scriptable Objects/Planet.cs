using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    
    public class Planet :  IComparable<Planet>
    {

        
        public float CoordX { get; private set; }
        public float CoordY { get; private set; }
        public int ID { get; private set; }

        public string displayName { get; private set; }
        public List<string> HyperlaneRoutes = new List<string>();
        public List<int> IndexInHyperLane = new List<int>();
        public List<Path> shortestPath;
        public List<Neighbour> neighbours = new List<Neighbour>();
        public GameObject gameObject;
        public string wikiLink { get; private set; }

        public void Initialize(string name,int id, float coordX, float coordY)
        {
            
            displayName = name;
            this.ID = id;
            CoordX = coordX;
            CoordY = coordY;
            wikiLink = "https://starwars.fandom.com/wiki/" + name.Replace(' ', '_');
        }

        public void OpenLink()
        {
            Application.OpenURL(wikiLink);
        }

        public int CompareTo(Planet other, int whichHyperlane)
        {
            if (IndexInHyperLane[whichHyperlane] > other.IndexInHyperLane[whichHyperlane]) return 1;
            else if (IndexInHyperLane[whichHyperlane] < other.IndexInHyperLane[whichHyperlane]) return -1;
            else return 0;
        }


        public int CompareTo(Planet other)
        {
            throw new NotImplementedException();
        }

        public void SetHyperLaneRoute(string hyperName, int Index)
        {
            HyperlaneRoutes.Add(hyperName);
            IndexInHyperLane.Add(Index);
        }

        public class Neighbour
        {
            public Planet Planet;
            public float Distance;
            public bool subLightNeighbour { get; private set; }
            
            public Neighbour(Planet previousPlanet, Planet planet)
            {
                Planet = previousPlanet;
                Vector2 pPlanetCoords = new Vector2(previousPlanet.CoordX, previousPlanet.CoordY);
                Vector2 planetCoords = new Vector2(planet.CoordX, planet.CoordY);
                Distance = Vector2.Distance(pPlanetCoords, planetCoords);
                subLightNeighbour = false;
            }

            public Neighbour(Planet previousPlanet, Planet planet, float subLightMultiplier)
            {
                Planet = previousPlanet;
                Vector2 pPlanetCoords = new Vector2(previousPlanet.CoordX, previousPlanet.CoordY);
                Vector2 planetCoords = new Vector2(planet.CoordX, planet.CoordY);
                Distance = Vector2.Distance(pPlanetCoords, planetCoords) * subLightMultiplier;
                subLightNeighbour = true;
            }
        }

        
    }
}
