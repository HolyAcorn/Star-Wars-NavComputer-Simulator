using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public class PathPresenter : MonoBehaviour
    {

        [SerializeField] PlanetRuntimeSet calculatedPath;
        [SerializeField] GameObject hyperLanePrefab;
        [SerializeField] StyleSettings styleSettings;
        [SerializeField] FloatReference majorLineWidth;
        [SerializeField] FloatReference sizeDifference;


        public void PresentPath()
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
            line.positionCount = calculatedPath.Count();
            line.sortingOrder++;
            for (int i = 0; i < calculatedPath.Count(); i++)
            {
                Vector2 pointPosition = new Vector2(calculatedPath.Get(i).CoordX, calculatedPath.Get(i).CoordY); ;
                line.SetPosition(i, pointPosition);
            }
            instance.transform.parent = transform;
        }
    }
}
