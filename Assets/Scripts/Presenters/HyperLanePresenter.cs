using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public class HyperLanePresenter : MonoBehaviour
    {
        LineRenderer line;
        [SerializeField] GameObject hyperlanePrefab;

        [SerializeField] HyperLaneRuntimeSet hyperLaneList;

        [SerializeField] float smoothingLength;
        [SerializeField] int smoothingSections;

        [SerializeField] FloatReference sizeDifference;
        public FloatReference majorLineWidth;
        public FloatReference mediumLineWidth;
        public FloatReference minorLineWidth;
        public StyleSettings styleSetting;


        private GameObject parentObject;

        private void Initialize()
        {
            parentObject = new GameObject();
            parentObject.name = "HyperLanes";
        }

        public void CreateAndDisplayHyperLaneLines()
        {
            Initialize();
            foreach (HyperLane hyperLane in hyperLaneList.items)
            {
                CreateHyperLaneLine(hyperLane);
            }
            parentObject.transform.localScale = new Vector3( sizeDifference.Value, sizeDifference.Value);
        }

        private void CreateHyperLaneLine(HyperLane hyperLane)
        {
            GameObject hyperLaneInstance = Instantiate(hyperlanePrefab);
            hyperLaneInstance.name = hyperLane.name;
            hyperLaneInstance.transform.parent = parentObject.transform;
            line = hyperLaneInstance.GetComponent<LineRenderer>();
            line.positionCount = hyperLane.Planets.Count();
            line.startColor = styleSetting.HyperLaneColor;
            line.endColor = styleSetting.HyperLaneColor;
            switch (hyperLane.Type)
            {
                case TypeEnum.Major:
                    line.widthMultiplier = majorLineWidth.Value;
                    break;
                case TypeEnum.Medium:
                    line.widthMultiplier = mediumLineWidth.Value;
                    break;
                case TypeEnum.Minor:
                    line.widthMultiplier = minorLineWidth.Value;
                    break;
                default:
                    break;
            }
            for (int i = 0; i < hyperLane.Planets.Count(); i++)
            {
                Planet planet = hyperLane.Planets.Get(i);
                Vector3 point = new Vector3(planet.CoordX, planet.CoordY, 0);
                line.SetPosition(i, point);
            }
        }

    }
}
