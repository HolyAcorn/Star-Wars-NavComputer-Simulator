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
        [SerializeField] NodeRuntimeSet noHyperlaneNodes;

        [SerializeField] NodeRuntimeSet startingNode;
        [SerializeField] NodeRuntimeSet targetNode;

        private Node startingPoint;
        private Node targetPoint;

        [SerializeField] FloatVariable finalDistance;
        [SerializeField] NodeRuntimeSet calculatedPath;
        [SerializeField] StringRuntimeSet tradeRoutesInPath;

        [SerializeField] StringVariable timeTakenDisplay;
        [SerializeField] GameEvent presentPath;

        [SerializeField] float parsecsPerHour = 93.75f;


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
            if (targetNode.Count() == 0 || startingNode.Count() == 0) return;
            pointMasterList = hyperLaneList.CreateHyperLanePath();
            foreach (Node planet in noHyperlaneNodes.items)
            {
                if (!pointMasterList.Nodes.Contains(planet)) pointMasterList.Nodes.Add(planet);
            }


            foreach (Node planet in pointMasterList.Nodes.items)
            {
                if(planet.name == "Tython")
                {

                }
                if (planet.name == startingNode.Get(0).displayName) startingPoint = planet;
                if (planet.name == targetNode.Get(0).displayName) targetPoint = planet;
            }
            //List<Node> travelPath = Astar();
            List<Node> travelPath = Dijkstra();
            foreach (Node planet in travelPath)
            {
                calculatedPath.Add(planet);
            }
            float hoursTaken = TravelTimeHelper.CalculateTimeRequiredInHours(finalDistance.Value, parsecsPerHour, 1, 0);
            timeTakenDisplay.Value = TravelTimeHelper.DecimalToTime(hoursTaken);
            presentPath.Raise();
        }

        List<Node> Astar()
        {
            calculatedPath.Clear();
            Dictionary<Node, Node> nextPointToTarget = new Dictionary<Node, Node>();
            PriorityQueue<Node> frontier = new PriorityQueue<Node>();
            Dictionary<Node, float> distancetoReachPoint = new Dictionary<Node, float>();

            frontier.Enqueue(targetPoint, 0);
            distancetoReachPoint[targetPoint] = 0;

            while (frontier.Count > 0)
            {
                Node currentPoint = frontier.Dequeue();

                if (currentPoint == startingPoint) break;
                foreach (Node.Neighbour neighbour in currentPoint.neighbours)
                {
                    float newDistance = distancetoReachPoint[currentPoint] + neighbour.Distance;
                    Node nPoint = neighbour.Node;
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

            Queue<Node> path = new Queue<Node>();
            Node currentPathPoint = startingPoint;
            while (currentPathPoint != targetPoint)
            {
                currentPathPoint = nextPointToTarget[currentPathPoint];
                path.Enqueue(currentPathPoint);
            }
            finalDistance.Value = distancetoReachPoint[startingPoint] * 10;
            Debug.Log("Distance to " + targetPoint.name + "is: " + finalDistance.Value.ToString("0.00") + "parsecs");
            List<Node> listPath = path.ToList();
            listPath.Insert(0, startingPoint);
            return listPath;


            float Distance(Node p1, Node p2)
            {
                return ((p1.CoordX - p2.CoordX) + (p1.CoordY - p2.CoordY));
            }
        }

        List<Node> Dijkstra()
        {
            calculatedPath.Clear();
            Dictionary<Node, Node> nextPointToTarget = new Dictionary<Node, Node>();
            PriorityQueue<Node> frontier = new PriorityQueue<Node>();
            Dictionary<Node, float> distancetoReachPoint = new Dictionary<Node, float>();

            frontier.Enqueue(targetPoint, 0);
            distancetoReachPoint[targetPoint] = 0;

            while (frontier.Count > 0)
            {
                Node currentPoint = frontier.Dequeue();
                if (currentPoint.name == "Qiilura" || currentPoint.name== "Ota")
                {
                    Debug.Log("ds");
                }
                if (currentPoint == startingPoint) break;
                foreach (Node.Neighbour neighbour in currentPoint.neighbours)
                {
                    float newDistance = distancetoReachPoint[currentPoint] + neighbour.Distance;
                    Node nPoint = neighbour.Node;
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

            Queue<Node> path = new Queue<Node>();
            Node currentPathPoint = startingPoint;
            while (currentPathPoint != targetPoint)
            {
                currentPathPoint = nextPointToTarget[currentPathPoint];
                path.Enqueue(currentPathPoint);
            }
            finalDistance.Value = distancetoReachPoint[startingPoint] * 10;
            Debug.Log("Distance to " + targetPoint.name + "is: " + finalDistance.Value.ToString("0.00") + "parsecs");
            List<Node> listPath = path.ToList();
            listPath.Insert(0, startingPoint);
            return listPath;


            float Distance(Node p1, Node p2)
            {
                return ((p1.CoordX - p2.CoordX) + (p1.CoordY - p2.CoordY));
            }
        }
    }
}
