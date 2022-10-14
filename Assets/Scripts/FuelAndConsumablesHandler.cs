using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelAndConsumablesHandler : MonoBehaviour
{

    public StarshipProfile starshipProfile;
    // Start is called before the first frame update
    public FloatVariable timeRequired;



    public int requiredFuelCells = 0;


    public void CalculateFuel()
    {
        float per6Hours = timeRequired.Value / 6;
        
    }

    private void CalculateWeeks()
    {
        float weeks = (timeRequired.Value / 24) / 5;
    }
}
