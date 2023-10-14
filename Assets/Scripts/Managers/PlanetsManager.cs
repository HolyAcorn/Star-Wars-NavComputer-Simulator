using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public class PlanetsManager : MonoBehaviour
    {
        [SerializeField] GameEvent UpdatePlanetSize;

        [Header("Sizes")]
        [SerializeField] FloatVariable cameraMinSize;
        [SerializeField] FloatVariable cameraMaxSize;

        [SerializeField] AnimationCurve sizeStepCurve;
        [SerializeField] FloatRuntimeSet cameraSizeSteps;
        [SerializeField] FloatRuntimeSet sizeStepsValues;
        [SerializeField] FloatReference currentZoom;
        [SerializeField] FloatReference currentPlanetSize;
        [SerializeField] float minSize = 0.5f;
        [SerializeField] float maxSize = 2.5f;
        private float[] sizes;
        private float size;
        // Start is called before the first frame update


        void Start()
        {
            size = minSize;
        }


        public void CheckSize()
        {
            /*sizes = new float[cameraSizeSteps.Count()];
            float range = maxSize - minSize;
            int sizeStepAmounts = cameraSizeSteps.Count() + 1;
            float step = range / sizeStepAmounts;
            for (int i = 0; i < cameraSizeSteps.Count(); i++)
            {
                if (currentZoom.Value > cameraSizeSteps.Get(i)) continue;
                size = Mathf.Lerp(minSize, maxSize, sizeStepsValues.Get(i));
                Debug.Log(minSize + " + " + "(" + step + " * " + i + ") = " + size);
                break;
            }*/

            float cameraSizeRange = cameraMaxSize.Value - cameraMinSize.Value;
            float time = currentZoom.Value / cameraSizeRange;
            float value = sizeStepCurve.Evaluate(time);
            size = Mathf.Lerp(minSize, maxSize, value);

            currentPlanetSize.Variable.Value = size;
            UpdatePlanetSize.Raise();



        }
    }
}
