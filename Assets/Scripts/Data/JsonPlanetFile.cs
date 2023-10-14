using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    [Serializable]
    public class JsonPlanetFile
    {
        public JsonPlanets[] JsonPlanets;
        public int Type;
    }
    [Serializable]
    public class JsonPlanets
    {
        public string Name;
        public float CoordX;
        public float CoordY;
    }
}
