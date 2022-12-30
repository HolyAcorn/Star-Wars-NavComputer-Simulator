using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SwNavComp
{


    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameEvent loadHyperLanes;

        private void Start()
        {
            loadHyperLanes.Raise();
        }

        public void ReloadMap()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

}
