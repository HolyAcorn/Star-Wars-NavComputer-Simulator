using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public class HyperLaneObjectPresenter : MonoBehaviour
    {
        private LineRenderer line;
        private TypeEnum type;
        [SerializeField] StyleSettings styleSetting;
        [SerializeField] FloatReference currentMinorLaneSize, currentMediumLaneSize, currentMajorLaneSize;

        public void Initialize(HyperLane hyperLane)
        {
            name = hyperLane.name;
            line = GetComponent<LineRenderer>();
            line.positionCount = hyperLane.Planets.Count();
            line.startColor = styleSetting.HyperLaneColor;
            line.endColor = styleSetting.HyperLaneColor;
            type = hyperLane.Type;
            for (int i = 0; i < hyperLane.Planets.Count(); i++)
            {
                Planet planet = hyperLane.Planets.Get(i);
                Vector3 point = new Vector3(planet.CoordX, planet.CoordY, 0);
                line.SetPosition(i, point);
            }
            SetSize();
        }

        public void InitializePath(PlanetRuntimeSet pointList)
        {
            name = "Generated Path";
            line = GetComponent<LineRenderer>();
            line.positionCount = pointList.Count();
            line.startColor = styleSetting.PathColor;
            line.endColor = styleSetting.PathColor;
            line.sortingOrder++;
            type = TypeEnum.Major;

            for (int i = 0; i < pointList.Count(); i++)
            {
                Vector2 pointPosition = new Vector2(pointList.Get(i).CoordX, pointList.Get(i).CoordY);
                line.SetPosition(i, pointPosition);
            }
            SetSize(0.1f);
        }

        public void SetSize(float modifier = 0)
        {
            switch (type)
            {
                case TypeEnum.Major:
                    line.widthMultiplier = currentMajorLaneSize.Value + modifier;
                    break;
                case TypeEnum.Medium:
                    line.widthMultiplier = currentMediumLaneSize.Value + modifier;
                    break;
                case TypeEnum.Minor:
                    line.widthMultiplier = currentMinorLaneSize.Value + modifier;
                    break;
                default:
                    break;
            }
        }
    }
}
