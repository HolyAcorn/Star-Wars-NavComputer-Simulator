using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starshipinfo : MonoBehaviour
{

    public FloatVariable hyperDriveRating;

    public void ToggleVisibility(bool value)
    {
        gameObject.SetActive(value);
    }

    public void UpdateHyperDriveRating(string value)
    {
        hyperDriveRating.Value = float.Parse(value);
    }
}
