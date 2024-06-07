using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace SwNavComp
{
    public class TopPanelPresenter : MonoBehaviour
    {
        [SerializeField] BoolVariable editMode;

        [SerializeField] Image editModeButton;
        [SerializeField] Color editModeColor;
        Color normalModeColors;

        private void Start()
        {
            normalModeColors = editModeButton.color;
        }

        public void UpdateEditModeButtonColor()
        {
            if (editMode.Value) editModeButton.color = editModeColor;
            else editModeButton.color = normalModeColors;
        }

    }
}
