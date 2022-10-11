using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
