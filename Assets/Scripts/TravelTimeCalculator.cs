using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TravelTime
{
    public class TravelTimeCalculator : MonoBehaviour
    {
        public HyperLane pointMasterList;
        public HyperLane otherPoints;
        public HyperLaneList hyperLaneList;

        public PlanetList startingPlanet;
        public PlanetList targetPlanet;

        private HyperLanePoint startingPoint;
        private HyperLanePoint targetPoint;

        public FloatReference modifier;
        public FloatReference hyperDriveClass;
        public float parsecsPerHour = 93.75f;

        private Path shortestPath = new Path();


        private void Start()
        {
            pointMasterList = hyperLaneList.CreateMasterPointList();
            foreach (HyperLanePoint point in pointMasterList.Points)
            {
                if (point.name == startingPlanet.Planets[0].name)
                {
                    startingPoint = point;
                }
                if (targetPlanet.Planets.Count > 0)
                {
                    if (point.name == targetPlanet.Planets[0].name)
                    {
                        targetPoint = point;
                    }

                }
            }
            ConnectPoints();
            //CalculateShortestPath();
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
            for (int i = pointMasterList.Points.Count -1; i >= 0; i--)
            {
                HyperLanePoint a = pointMasterList.Points[i];
                for (int p = pointMasterList.Points.Count -1; p >= 0; p--)
                {
                    HyperLanePoint b = pointMasterList.Points[p];
                    if (a.name == b.name && a != b)
                    {
                        HyperLanePoint c = MergePoints(a, b);
                        pointMasterList.Points.Remove(a);
                        pointMasterList.Points.Remove(b);
                        pointMasterList.Points.Add(c);
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
            otherPoints = pointMasterList;


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
    }

    public class Path
    {
        public HyperLanePoint HyperLanePoint;
        public float Distance;
        public List<HyperLanePoint> Stops = new List<HyperLanePoint>();
    }
}