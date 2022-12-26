using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempTimeDisplay : MonoBehaviour
{

    [SerializeField] Text text;

    string[] splitText;

   /* public PlanetList targetPlanet;
    public PlanetList startPlanet;
    public FloatReference timeRequired;
    public FloatReference finalDistance;

    private void Start()
    {
        text.gameObject.SetActive(false);
        splitText = text.text.Split('X');
    }

    public void UpdateText()
    {
        if (startPlanet.Planets.Count > 0 && targetPlanet.Planets.Count > 0)
        {
            text.text = splitText[0] + startPlanet.Planets[0].name + splitText[1] + targetPlanet.Planets[0].name + splitText[2] + timeRequired.Value.ToString("0.00") + splitText[3] + finalDistance.Value.ToString("0.00") + splitText[4];

        }
        text.gameObject.SetActive(true);


    }*/
}
