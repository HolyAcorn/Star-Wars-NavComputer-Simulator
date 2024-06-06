using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SwNavComp
{
    namespace HLEditor
    {
        public class SelectableInputField : MonoBehaviour
        {
            public bool isSelected {get; private set; } = false;
            [SerializeField] GameEvent setFlipThrough;

            public void Selected() 
            {   
                isSelected = true;
                setFlipThrough.Raise();
            }

            public void Deselect() { isSelected = false; }
        }
    }
}
