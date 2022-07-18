using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempTimeDisplay : MonoBehaviour
{

    [SerializeField] Text text;

    public PlanetList targetPlanet;
    public PlanetList startPlanet;
    public FloatReference timeRequired;

    private void Start()
    {
        text.gameObject.SetActive(false);
    }

    public void UpdateText()
    {
        string[] splitText = text.text.Split('X');
        if (startPlanet.Planets.Count > 0 && targetPlanet.Planets.Count > 0)
        {
            text.text = splitText[0] + startPlanet.Planets[0].name + splitText[1] + targetPlanet.Planets[0].name + splitText[2] + timeRequired.Value.ToString("0.00") + splitText[3];

        }
        text.gameObject.SetActive(true);


    }
}
