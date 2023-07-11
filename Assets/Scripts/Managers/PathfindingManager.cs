using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SwNavComp
{
    public class PathfindingManager : MonoBehaviour
    {
        HyperLane pointMasterList;
        [SerializeField] HyperLaneRuntimeSet hyperLaneList;
        [SerializeField] PlanetRuntimeSet noHyperlanePlanets;

        [SerializeField] PlanetRuntimeSet startingPlanet;
        [SerializeField] PlanetRuntimeSet targetPlanet;

        private Planet startingPoint;
        private Planet targetPoint;

        [SerializeField] FloatVariable finalDistance;
        [SerializeField] PlanetRuntimeSet calculatedPath;
        [SerializeField] StringRuntimeSet tradeRoutesInPath;

        [SerializeField] StringVariable timeTakenDisplay;
        [SerializeField] GameEvent presentPath;

        [SerializeField] float parsecsPerHour = 93.75f;
        [SerializeField] StarshipProfile starshipProfile;

        private void Awake()
        {
            calculatedPath.Clear();
            tradeRoutesInPath.Clear();
        }

        private void OnApplicationQuit()
        {
            calculatedPath.Clear();
            tradeRoutesInPath.Clear();
        }


        public void CreatePath()
        {
            if (targetPlanet.Count() == 0 || startingPlanet.Count() == 0) return;
            pointMasterList = hyperLaneList.CreateHyperLanePath();
            foreach (Planet planet in noHyperlanePlanets.items)
            {
                if (!pointMasterList.Planets.Contains(planet)) pointMasterList.Planets.Add(planet);
            }


            foreach (Planet planet in pointMasterList.Planets.items)
            {
                if(planet.name == "Tython")
                {

                }
                if (planet.name == startingPlanet.Get(0).displayName) startingPoint = planet;
                if (planet.name == targetPlanet.Get(0).displayName) targetPoint = planet;
            }
            //List<Planet> travelPath = Astar();
            List<Planet> travelPath = Dijkstra();
            foreach (Planet planet in travelPath)
            {
                calculatedPath.Add(planet);
            }
            float hoursTaken = TravelTimeHelper.CalculateTimeRequiredInHours(finalDistance.Value, parsecsPerHour, starshipProfile.HyperdriveRating, 0);
            timeTakenDisplay.Value = TravelTimeHelper.DecimalToTime(hoursTaken);
            presentPath.Raise();
        }

        List<Planet> Astar()
        {
            calculatedPath.Clear();
            Dictionary<Planet, Planet> nextPointToTarget = new Dictionary<Planet, Planet>();
            PriorityQueue<Planet> frontier = new PriorityQueue<Planet>();
            Dictionary<Planet, float> distancetoReachPoint = new Dictionary<Planet, float>();

            frontier.Enqueue(targetPoint, 0);
            distancetoReachPoint[targetPoint] = 0;

            while (frontier.Count > 0)
            {
                Planet currentPoint = frontier.Dequeue();

                if (currentPoint == startingPoint) break;
                foreach (Planet.Neighbour neighbour in currentPoint.neighbours)
                {
                    float newDistance = distancetoReachPoint[currentPoint] + neighbour.Distance;
                    Planet nPoint = neighbour.Planet;
                    if (!distancetoReachPoint.ContainsKey(nPoint) || newDistance < distancetoReachPoint[nPoint])
                    {
                        distancetoReachPoint[nPoint] = newDistance;
                        float priority = newDistance + Distance(nPoint, startingPoint);
                        frontier.Enqueue(nPoint, priority);
                        nextPointToTarget[nPoint] = currentPoint;
                    }
                }
            }

            if (!nextPointToTarget.ContainsKey(startingPoint)) return null;

            Queue<Planet> path = new Queue<Planet>();
            Planet currentPathPoint = startingPoint;
            while (currentPathPoint != targetPoint)
            {
                currentPathPoint = nextPointToTarget[currentPathPoint];
                path.Enqueue(currentPathPoint);
            }
            finalDistance.Value = distancetoReachPoint[startingPoint] * 10;
            Debug.Log("Distance to " + targetPoint.name + "is: " + finalDistance.Value.ToString("0.00") + "parsecs");
            List<Planet> listPath = path.ToList();
            listPath.Insert(0, startingPoint);
            return listPath;


            float Distance(Planet p1, Planet p2)
            {
                return ((p1.CoordX - p2.CoordX) + (p1.CoordY - p2.CoordY));
            }
        }

        List<Planet> Dijkstra()
        {
            calculatedPath.Clear();
            Dictionary<Planet, Planet> nextPointToTarget = new Dictionary<Planet, Planet>();
            PriorityQueue<Planet> frontier = new PriorityQueue<Planet>();
            Dictionary<Planet, float> distancetoReachPoint = new Dictionary<Planet, float>();

            frontier.Enqueue(targetPoint, 0);
            distancetoReachPoint[targetPoint] = 0;

            while (frontier.Count > 0)
            {
                Planet currentPoint = frontier.Dequeue();
                if (currentPoint.displayName == "Qiilura" || currentPoint.displayName == "Ota")
                {
                    Debug.Log("ds");
                }
                if (currentPoint == startingPoint) break;
                foreach (Planet.Neighbour neighbour in currentPoint.neighbours)
                {
                    float newDistance = distancetoReachPoint[currentPoint] + neighbour.Distance;
                    Planet nPoint = neighbour.Planet;
                    if (!distancetoReachPoint.ContainsKey(nPoint) || newDistance < distancetoReachPoint[nPoint])
                    {
                        distancetoReachPoint[nPoint] = newDistance;
                        float priority = newDistance;
                        frontier.Enqueue(nPoint, priority);
                        nextPointToTarget[nPoint] = currentPoint;
                    }
                }
            }

            if (!nextPointToTarget.ContainsKey(startingPoint)) return null;

            Queue<Planet> path = new Queue<Planet>();
            Planet currentPathPoint = startingPoint;
            while (currentPathPoint != targetPoint)
            {
                currentPathPoint = nextPointToTarget[currentPathPoint];
                path.Enqueue(currentPathPoint);
            }
            finalDistance.Value = distancetoReachPoint[startingPoint] * 10;
            Debug.Log("Distance to " + targetPoint.name + "is: " + finalDistance.Value.ToString("0.00") + "parsecs");
            List<Planet> listPath = path.ToList();
            listPath.Insert(0, startingPoint);
            return listPath;


            float Distance(Planet p1, Planet p2)
            {
                return ((p1.CoordX - p2.CoordX) + (p1.CoordY - p2.CoordY));
            }
        }
    }
}
