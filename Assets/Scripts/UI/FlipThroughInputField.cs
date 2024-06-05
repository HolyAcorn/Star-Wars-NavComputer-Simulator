using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SwNavComp
{
    public class FlipThroughInputField : MonoBehaviour
    {
        [SerializeField] GameObjectRuntimeSet flipThroughObjectList;
        private List<TMP_InputField> flipThroughList = new List<TMP_InputField>();

        [SerializeField] IntReference flipIndex;
        [SerializeField] IntReference flipDirection;

        private void Start()
        {
            flipThroughObjectList.Clear();
        }

        public void SetupFlipThroughList()
        {
            flipThroughList.Clear();
            foreach (var item in flipThroughObjectList.items)
            {
                for (int i = 0; i < item.transform.childCount; i++)
                {
                    GameObject child = item.transform.GetChild(i).gameObject;
                    if (child.GetComponent<TMP_InputField>() == null) continue;
                    flipThroughList.Add(child.GetComponent<TMP_InputField>());
                }
            }
        }

        public void SetFlipIndex()
        {

        }

        public void FlipThrough()
        {
            flipIndex.Variable.Value += flipDirection.Value;
            if (flipIndex.Value > flipThroughList.Count - 1) flipIndex.Variable.Value = 0;
            if (flipIndex.Value < 0) flipIndex.Variable.Value = flipThroughList.Count - 1;

            flipThroughList[flipIndex.Value].Select();
            Debug.Log(flipThroughList[flipIndex.Value].text);
        }
    }
}
