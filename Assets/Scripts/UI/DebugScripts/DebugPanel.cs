using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SwNavComp
{
    public class DebugPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text mousePositionText;
        [SerializeField] TMP_Text maxIndexText;

        [SerializeField] Vector2Reference currentMousePosition;
        Vector2 mousePositionLastFrame;
        [SerializeField] IntReference currentMaxIndex;
        int maxIndexLastFrame;

        // Update is called once per frame
        void Update()
        {
            if(currentMousePosition.Value != mousePositionLastFrame)
            {
                mousePositionLastFrame = currentMousePosition.Value;
                mousePositionText.text = currentMousePosition.Value.ToString();
            }

            if (currentMaxIndex.Value != maxIndexLastFrame)
            {
                maxIndexLastFrame = currentMaxIndex.Value;
                maxIndexText.text = currentMaxIndex.Value.ToString();
            }
        }


        public void ToggleVisibility()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
    }
}
