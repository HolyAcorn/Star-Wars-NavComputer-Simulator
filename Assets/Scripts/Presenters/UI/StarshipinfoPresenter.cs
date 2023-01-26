using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using UnityEngine.Networking;
using SwNavComp;

public class StarshipinfoPresenter : MonoBehaviour
{
    [SerializeField] StarshipProfile starshipProfile;

    [SerializeField] TMP_InputField silhouetteInputField;
    [SerializeField] TMP_InputField speedInputField;
    [SerializeField] TMP_InputField handlingInputField;
    [SerializeField] TMP_InputField hyperDriveRatingInputField;
    [SerializeField] TMP_InputField consumablesThresholdInputField;
    [SerializeField] TMP_InputField consumablesInputField;
    [SerializeField] TMP_InputField fuelThresholdInputField;
    [SerializeField] TMP_InputField fuelInputField;
    [SerializeField] TMP_InputField foreInputField;
    [SerializeField] TMP_InputField aftInputField;
    [SerializeField] TMP_InputField starboardInputField;
    [SerializeField] TMP_InputField portInputField;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] TMP_InputField modelInputField;
    [SerializeField] TMP_InputField typeInputField;



    public void PresentStarShipInfo()
    {
        silhouetteInputField.text = starshipProfile.Silhouette.ToString();
        speedInputField.text = starshipProfile.Speed.ToString();
        handlingInputField.text = starshipProfile.Handling.ToString();
        hyperDriveRatingInputField.text = starshipProfile.HyperdriveRating.ToString();
        consumablesThresholdInputField.text = starshipProfile.ConsumablesThreshold.ToString();
        consumablesInputField.text = starshipProfile.Consumables.ToString();
        fuelThresholdInputField.text = starshipProfile.FuelThreshold.ToString();
        fuelInputField.text = starshipProfile.Fuel.ToString();
        foreInputField.text = starshipProfile.Defense.Fore.ToString();
        aftInputField.text = starshipProfile.Defense.Aft.ToString();
        starboardInputField.text = starshipProfile.Defense.Starboard.ToString();
        portInputField.text = starshipProfile.Defense.Port.ToString();
        nameInputField.text = starshipProfile.Name;
        modelInputField.text = starshipProfile.Model;
        typeInputField.text = starshipProfile.Type;
    }

    public void ToggleVisibility(bool value)
    {
        gameObject.SetActive(value);
    }


    public void OnClickLoadStarship()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "json", false);
        if (paths.Length == 0) return;
        string file = File.ReadAllText(paths[0]);
        starshipProfile = JsonConverter.LoadFromJsonStarShip(file);
        PresentStarShipInfo();
    }

   
}
