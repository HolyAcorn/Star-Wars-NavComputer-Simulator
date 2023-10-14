using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

        [SerializeField] FloatVariable cameraMinSize, cameraMaxSize;
        [SerializeField] FloatVariable currentZoom;
        [SerializeField] FloatReference currentMinorLaneSize, currentMediumLaneSize, currentMajorLaneSize;
        [SerializeField] AnimationCurve sizeStepCurve;

        [SerializeField] FloatReference sizeDifference;
        public float minMajorLineWidth = 0.75f;
        public float minMediumLineWidth = 0.5f;
        public float minMinorLineWidth = 0.25f;
        public float maxMajorLineWidth = 2.25f;
        public float maxMediumLineWidth = 1.5f;
        public float maxMinorLineWidth = 0.75f;

        [SerializeField] GameEvent UpdateHyperLaneSize;


        private List<GameObject> hyperLaneInstances = new List<GameObject>();

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
            CheckSize();
        }

        private void CreateHyperLaneLine(HyperLane hyperLane)
        {
            GameObject hyperLaneInstance = Instantiate(hyperlanePrefab);
            hyperLaneInstance.transform.parent = parentObject.transform;
            hyperLaneInstance.GetComponent<HyperLaneObjectPresenter>().Initialize(hyperLane);
            hyperLaneInstances.Add(hyperLaneInstance);
            
        }


        public void CheckSize()
        {
            currentMinorLaneSize.Variable.Value = GetSizeFromCameraSize(minMinorLineWidth, maxMinorLineWidth);
            currentMediumLaneSize.Variable.Value = GetSizeFromCameraSize(minMediumLineWidth, maxMediumLineWidth);
            currentMajorLaneSize.Variable.Value = GetSizeFromCameraSize(minMajorLineWidth, maxMajorLineWidth);
            UpdateHyperLaneSize.Raise();
            Debug.Log("Size Updated");
        }

        private float GetSizeFromCameraSize(float min, float max)
        {
            float cameraSizeRange = cameraMaxSize.Value - cameraMinSize.Value;
            float time = currentZoom.Value / cameraSizeRange;
            float value = sizeStepCurve.Evaluate(time);
            return Mathf.Lerp(min, min, value);

        }
    }
}
