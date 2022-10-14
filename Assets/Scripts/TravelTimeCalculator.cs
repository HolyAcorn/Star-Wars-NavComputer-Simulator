using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TravelTime
{
    public class TravelTimeCalculator : MonoBehaviour
    {

        [HideInInspector]
        public HyperLane pointMasterList;
        [HideInInspector]
        private HyperLane otherPoints;
        public HyperLaneList hyperLaneList;
        public GameObject hyperLanePrefab;
        public FloatReference majorLineWidth;
        public StyleSettings styleSettings;

        public PlanetList startingPlanet;
        public PlanetList targetPlanet;

        private HyperLanePoint startingPoint;
        private HyperLanePoint targetPoint;

        public FloatReference sizeDifference;

        public FloatReference modifier;
        public StarshipProfile starshipProfile;
        public float parsecsPerHour = 93.75f;
        public FloatReference finalDistance;
        public FloatReference timeRequired;

        private Path shortestPath = new Path();
        [SerializeField] HyperPointRuntimeSet calculatedPath;
        [SerializeField] StringRuntimeSet TradeRoutes;
        [SerializeField] GameEvent calculateFuel;


        public void CalculateTimeRequired()
        {
            timeRequired.Variable.Value = ((finalDistance.Value / parsecsPerHour) * starshipProfile.HyperdriveRating) + modifier.Value;
        }

        private void Awake()
        {
            calculatedPath.Clear();
        }
        private void OnApplicationQuit()
        {
            calculatedPath.Clear();
            TradeRoutes.Clear();
        }

        public void CreatePath()
        {
            pointMasterList = hyperLaneList.CreateMasterPointList();
            foreach (HyperLanePoint p in pointMasterList.Points)
            {
                if (p.name == startingPlanet.Planets[0].name)
                {
                    startingPoint = p;
                }
                if (targetPlanet.Planets.Count > 0)
                {
                    if (p.name == targetPlanet.Planets[0].name)
                    {
                        targetPoint = p;
                    }

                }
            }
            ConnectPoints();
            List<HyperLanePoint> point = Astar();
            calculatedPath.items = point;
            GeneratePath(point);
            calculateFuel.Raise();
            TradeRoutes.items = CalculateNumberOfHyperLanes();

        }

        private void GeneratePath(List<HyperLanePoint> points)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            GameObject instance = Instantiate(hyperLanePrefab);
            LineRenderer line = instance.GetComponent<LineRenderer>();
            instance.name = "Generated Path";
            line.startColor = styleSettings.PathColor;
            line.endColor = styleSettings.PathColor;
            line.widthMultiplier = majorLineWidth.Value + 0.1f;
            line.positionCount = points.Count;
            line.sortingOrder++;
            for (int i = 0; i < points.Count; i++)
            {
                line.SetPosition(i, points[i].Position / sizeDifference.Value);
            }
            instance.transform.parent = transform;

        }

        private void ConnectPoints()
        {
            foreach (HyperLane hyperLane in hyperLaneList.Hyperlanes)
            {
                for (int p = 1; p < hyperLane.Points.Count; p++)
                {
                    HyperLanePoint a = hyperLane.Points[p - 1];
                    HyperLanePoint b = hyperLane.Points[p];
                    ConnectNeighbours(a, b);
                }
            }
            for (int i = pointMasterList.Points.Count - 1; i >= 0; i--)
            {
                HyperLanePoint a = pointMasterList.Points[i];
                for (int p = pointMasterList.Points.Count - 1; p >= 0; p--)
                {
                    HyperLanePoint b = pointMasterList.Points[p];
                    if (a.name == b.name && a != b)
                    {
                        HyperLanePoint c = MergePoints(a, b);
                        pointMasterList.Points.Remove(a);
                        pointMasterList.Points.Remove(b);
                        pointMasterList.Points.Add(c);
                        foreach (var neighbour in a.Neighbours)
                        {
                            HyperLanePoint nPoint = neighbour.Point;
                            for (int n = 0; n < nPoint.Neighbours.Count; n++)
                            {
                                HyperLanePoint nPoint2 = nPoint.Neighbours[n].Point;
                                if (nPoint2.name == c.name)
                                {
                                    nPoint.Neighbours[n].Point = c;
                                }
                            }
                        }
                    }

                }
            }
        }

        private HyperLanePoint MergePoints(HyperLanePoint a, HyperLanePoint b)
        {
            HyperLanePoint c = a;
            foreach (var neighbour in b.Neighbours)
            {
                c.Neighbours.Add(neighbour);
            }
            return c;
        }
        private void ConnectNeighbours(HyperLanePoint a, HyperLanePoint b)
        {
            float disance = Vector2.Distance(a.Position, b.Position);
            a.Neighbours.Add(new HyperLanePoint.Neighbour { Point = b, Distance = disance });
            b.Neighbours.Add(new HyperLanePoint.Neighbour { Point = a, Distance = disance });
        }


        private void CalculateShortestPath()
        {
            otherPoints = ScriptableObject.CreateInstance<HyperLane>();
            otherPoints.Points = new List<HyperLanePoint>();


            foreach (HyperLanePoint point in pointMasterList.Points)
            {
                if (point != startingPoint)
                {
                    otherPoints.Points.Add(point);
                }
            }

            // Q = set of neighbours
            // u = Neighbour in Q that has the shortest distance from source
            // v = HyperLanePoint in map
            List<HyperLanePoint> Q = new List<HyperLanePoint> { };
            Dictionary<HyperLanePoint, float> dist = new Dictionary<HyperLanePoint, float> { };
            Dictionary<HyperLanePoint, HyperLanePoint> prev = new Dictionary<HyperLanePoint, HyperLanePoint> { };
            foreach (HyperLanePoint v in otherPoints.Points)
            {
                dist.Add(v, 99);
                prev.Add(v, null);
                Q.Add(v);
            }
            dist[startingPoint] = 0;

            while (Q.Count != 0)
            {

                HyperLanePoint u = Q.OrderBy((v) => dist[v]).First();

                Q.Remove(u);
                for (int v = 0; v < u.Neighbours.Count; v++)
                {
                    HyperLanePoint neighbour = u.Neighbours[v].Point;
                    if (Q.Contains(u.Neighbours[v].Point))
                    {
                        float alt = dist[u] + u.Neighbours[v].Distance;
                        if (alt < dist[neighbour])
                        {
                            dist[neighbour] = alt;
                            prev[neighbour] = u;
                        }
                    }
                }
            }
            for (int index = 0; index < otherPoints.Points.Count; index++)
            {
                HyperLanePoint otherHyperLanePoint = otherPoints.Points[index];
                if (otherHyperLanePoint == startingPoint) continue;
                var path = new Path { HyperLanePoint = otherHyperLanePoint, Distance = dist[otherHyperLanePoint] };
                startingPoint.ShortestPath.Add(path);

                var stop = prev[otherHyperLanePoint];

                while (stop != startingPoint)
                {
                    path.Stops.Insert(0, stop);
                    stop = prev[stop];

                }
            }
            startingPoint.ShortestPath.Sort((a, b) => a.Distance.CompareTo(b.Distance));



        }


        List<HyperLanePoint> Astar()
        {
            Dictionary<HyperLanePoint, HyperLanePoint> nextPointToTarget= new Dictionary<HyperLanePoint, HyperLanePoint>();
            PriorityQueue<HyperLanePoint> frontier = new PriorityQueue<HyperLanePoint>();
            Dictionary<HyperLanePoint, float> distanceToReachPoint = new Dictionary<HyperLanePoint, float>();

            frontier.Enqueue(targetPoint, 0);
            distanceToReachPoint[targetPoint] = 0;

            while (frontier.Count > 0)
            {
                HyperLanePoint currentPpoint = frontier.Dequeue();

                if (currentPpoint == startingPoint) break;
                foreach (HyperLanePoint.Neighbour neighbour in currentPpoint.Neighbours)
                {
                    float newDistance = distanceToReachPoint[currentPpoint] + neighbour.Distance;
                    var nPoint = neighbour.Point;
                    if (!distanceToReachPoint.ContainsKey(nPoint) || newDistance < distanceToReachPoint[nPoint])
                    {
                        distanceToReachPoint[nPoint] = newDistance;
                        float priority = newDistance + Distance(nPoint, startingPoint);
                        frontier.Enqueue(nPoint, priority);
                        nextPointToTarget[nPoint] = currentPpoint;

                    }
                }
            }

            if (!nextPointToTarget.ContainsKey(startingPoint)) return null;


            Queue<HyperLanePoint> path = new Queue<HyperLanePoint>();
            HyperLanePoint curPathPoint = startingPoint;
            while (curPathPoint != targetPoint)
            {
                curPathPoint = nextPointToTarget[curPathPoint];
                path.Enqueue(curPathPoint);
            }
            finalDistance.Variable.Value = distanceToReachPoint[startingPoint] * 10;
            Debug.Log("Distance to: "+ targetPoint.name + " is: "+ finalDistance.Value.ToString("0.00") + " parsecs");
            List<HyperLanePoint> listPath = path.ToList();
            listPath.Insert(0, startingPoint);
            return listPath;

            float Distance(HyperLanePoint p1, HyperLanePoint p2)
            {
                return (p1.Position.x - p2.Position.x) + (p1.Position.y - p2.Position.y);
            }

        }

        Queue<HyperLanePoint> FloodFill()
        {
            Dictionary<HyperLanePoint, HyperLanePoint> nextPlanetToGoal = new Dictionary<HyperLanePoint, HyperLanePoint>();
            Queue<HyperLanePoint> frontier = new Queue<HyperLanePoint>();
            List<HyperLanePoint> visitedPlanets = new List<HyperLanePoint>();

            frontier.Enqueue(targetPoint);

            while (frontier.Count > 0)
            {
                HyperLanePoint currentPlanet = frontier.Dequeue();

                foreach (HyperLanePoint.Neighbour neighbour in currentPlanet.Neighbours)
                {
                    
                    var nPoint = neighbour.Point;
                    if (!visitedPlanets.Contains(nPoint) && !frontier.Contains(nPoint))
                    {
                        frontier.Enqueue(nPoint);
                        nextPlanetToGoal[nPoint] = currentPlanet;
                    }
                }
                visitedPlanets.Add(currentPlanet);
            }

            if (!visitedPlanets.Contains(startingPoint)) return null;
            

            Queue<HyperLanePoint> path = new Queue<HyperLanePoint>();
            HyperLanePoint curPathPlanet = startingPoint;
            while (curPathPlanet != targetPoint)
            {
                curPathPlanet = nextPlanetToGoal[curPathPlanet];
                path.Enqueue(curPathPlanet);
            }
            
            return path;
        }


        private List<string> CalculateNumberOfHyperLanes()
        {
            List<string> hyperLanes = new List<string>();
            for (int a = 0; a < calculatedPath.Count() -1; a++)
            {
                HyperLanePoint hyperPointA = calculatedPath.GetItemFromIndex(a);
                HyperLanePoint hyperPointB = calculatedPath.GetItemFromIndex(a + 1);
                for (int i = 0; i < hyperPointA.Planet.HyperlaneRoutes.Count; i++)
                {
                    string hyperLaneX = hyperPointA.Planet.HyperlaneRoutes[i];
                    for (int y = 0; y < hyperPointB.Planet.HyperlaneRoutes.Count; y++)
                    {
                        string hyperLaneY = hyperPointB.Planet.HyperlaneRoutes[y];

                        if (hyperLaneX == hyperLaneY)
                        {
                            if (!hyperLanes.Contains(hyperLaneX)) hyperLanes.Add(hyperLaneX);
                            break;
                        }
                    }
                }


            }
            return hyperLanes;

        }
    }

    


    public class Path
    {
        public HyperLanePoint HyperLanePoint;
        public float Distance;
        public List<HyperLanePoint> Stops = new List<HyperLanePoint>();
    }
}