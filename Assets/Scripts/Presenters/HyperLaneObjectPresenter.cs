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

        public void SetSize()
        {
            switch (type)
            {
                case TypeEnum.Major:
                    line.widthMultiplier = currentMajorLaneSize.Value;
                    break;
                case TypeEnum.Medium:
                    line.widthMultiplier = currentMediumLaneSize.Value;
                    break;
                case TypeEnum.Minor:
                    line.widthMultiplier = currentMinorLaneSize.Value;
                    break;
                default:
                    break;
            }
        }
    }
}
