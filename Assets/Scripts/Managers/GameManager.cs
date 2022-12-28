using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{


    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameEvent loadHyperLanes;

        private void Start()
        {
            loadHyperLanes.Raise();
        }

    }
}
