using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SwNavComp
{


    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameEvent presentHyperlanes;
        [SerializeField] BoolVariable editMode;

        private void Start()
        {
            presentHyperlanes.Raise();
        }

        public void ReloadMap()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ToggleEditMode()
        {
            editMode.Value = !editMode.Value;
        }

    }

}
