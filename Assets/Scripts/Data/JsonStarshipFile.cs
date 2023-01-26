using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    [Serializable]
    public class JsonStarshipFile
    {
        public StarshipJsonObject StarshipJsonObject;
    }
    
    [Serializable]
    public class StarshipJsonObject
    {
        public string Name;
        public string Model;
        public string Type;
        public string Description;

        public string Crew;
        public int Passengers;

        public string HyperDriveType;
        public float HyperdriveRating;

        public int Silhouette;
        public int Speed;
        public int Handling;

        public int ForeDefense;
        public int PortDefense;
        public int StarboardDefense;
        public int AftDefense;

        public int Armor;
        public int HullTraumaThreshold;
        public int HullTrauma;

        public int SystemStrainThreshold;
        public int SystemStrain;

        public int FuelThreshold;
        public int Fuel;

        public int ConsumablesThreshold;
        public int Consumables;
    }
}
