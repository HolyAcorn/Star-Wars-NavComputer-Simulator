using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public abstract class SizeHelper
    {
        public void ChangeChildrenSize(GameObject parent, float amount)
        {
            if (parent.transform.childCount == 0) return;

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                parent.transform.GetChild(i).transform.localScale = new Vector3(amount, amount, amount);
            }
        }


    }
}
