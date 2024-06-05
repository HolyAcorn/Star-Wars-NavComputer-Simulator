using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    [Serializable]
    public class JsonPlanetFile
    {
        public int Type;
        public JsonPlanets[] JsonPlanets;
    }
    [Serializable]
    public class JsonPlanets
    {
        public string Name;
        public int ID;
        public float CoordX;
        public float CoordY;
    }
}
