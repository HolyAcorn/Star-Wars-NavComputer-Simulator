using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public class RegionsPresenter : MonoBehaviour
    {
        [SerializeField] FloatReference sizeDifference;

        public void SetRegionSize()
        {
            transform.localScale = new Vector3(sizeDifference.Value, sizeDifference.Value);
        }
    }
}
