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
            instance.transform.parent = transform;
            instance.GetComponent<HyperLaneObjectPresenter>().InitializePath(calculatedPath);
            
        }
    }
}
