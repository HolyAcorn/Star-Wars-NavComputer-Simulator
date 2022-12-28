using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public struct Defense
{
    public int Fore;
    public int Port;
    public int Starboard;
    public int Aft;
}

[CreateAssetMenu(fileName = "Starship Profile", menuName = "Scriptable Objects/Starship Profile")]
public class StarshipProfile : ScriptableObject
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

    public Defense Defense;

    public int Armor;
    public int HullTraumaThreshold;
    public int HullTrauma;

    public int SystemStrainThreshold;
    public int SystemStrain;

    public int FuelThreshold;
    public int Fuel;

    public int ConsumablesThreshold;
    public int Consumables;

    // 0 is undamaged,
    // 1 is lightly damaged
    // 2 is heavily damaged
    [Range(0, 2)]
    public int damageLevel;


    #region UpdateSettings
    public void UpdateName(string value) { Name = value; }
    public void UpdateModel(string value) { Model = value; }
    public void UpdateType(string value) { Type = value; }
    public void UpdateDescription(string value) { Description = value; }
    public void UpdateCrew(string value) { Crew = value; }
    public void UpdatePassengers(string value) { Passengers = Int32.Parse(value); }
    public void UpdateHyperDriveType(string value) { HyperDriveType = value; }
    public void UpdateHyperdriveRating(string value) { HyperdriveRating = Int32.Parse(value); }
    public void UpdateSilhouette(string value) { Silhouette= Int32.Parse(value); }
    public void UpdateSpeed(string value) { Speed = Int32.Parse(value); }
    public void UpdateHandling(string value) { Handling = Int32.Parse(value); }
    public void UpdateDefenseFore(string value) { Defense.Fore = Int32.Parse(value); }
    public void UpdateDefenseStarboard(string value) { Defense.Starboard = Int32.Parse(value); }
    public void UpdateDefenseAft(string value) { Defense.Aft = Int32.Parse(value); }
    public void UpdateDefensePort(string value) { Defense.Port = Int32.Parse(value); }
    public void UpdateHullTrauma(string value) 
    { 
        HullTrauma = Int32.Parse(value);
        if (HullTrauma / HullTraumaThreshold > 0.75f) damageLevel = 0;
        else if (HullTrauma / HullTraumaThreshold > 0.5f) damageLevel = 1;
        else damageLevel = 2;
    }
    public void UpdateHullTraumaThreshold(string value) { HullTraumaThreshold = Int32.Parse(value); }

    public void UpdateFuelThreshold(string value) { FuelThreshold = Int32.Parse(value); }
    public void UpdateFuel(string value) { Fuel = Int32.Parse(value); }

    public void UpdateConsumablesThreshold(string value) { ConsumablesThreshold = Int32.Parse(value); }
    public void UpdateConsumables(string value) { Consumables = Int32.Parse(value); }

    #endregion

    #region SAVE TO JSON



    public void SaveStarshipToJson()
    {
        string SAVEPATH = Application.dataPath + "/StreamingAssets/Starships/";

    SaveObject saveObject = new SaveObject
        {

            Name = Name,
            Model = Model,
            Type = Type,
            Description = Description,
            Crew = Crew,
            Passengers = Passengers,
            HyperdriveRating = HyperdriveRating,
            HyperDriveType = HyperDriveType,
            Silhouette = Silhouette,
            Speed = Speed,
            Handling = Handling,
            ForeDefense = Defense.Fore,
            PortDefense = Defense.Port,
            StarboardDefense = Defense.Starboard,
            AftDefense = Defense.Aft,
            Armor = Armor,
            HullTraumaThreshold = HullTraumaThreshold,
            HullTrauma = HullTrauma,
            SystemStrain = SystemStrain,
            SystemStrainThreshold = SystemStrainThreshold,
        };

        string json = JsonUtility.ToJson(saveObject);


        if (!Directory.Exists(SAVEPATH)) Directory.CreateDirectory(SAVEPATH);
        if (File.Exists(SAVEPATH + "/" + Name + ".json")) Debug.Log("Overwritten file");

        File.WriteAllText(SAVEPATH + "/" + Name + ".json", json);


    }

    public class SaveObject
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
    }
    #endregion
}
