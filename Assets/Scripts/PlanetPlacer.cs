using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPlacer : MonoBehaviour
{
    public PlanetList planetList;
    public GameObject planetTemplatePrefab;

    public InputHandler inputHandler;

    void Start()
    {
        for (int i = 0; i < planetList.Planets.Count; i++)
        {
            Planet planet = planetList.Planets[i];
            GameObject instance = Instantiate(planetTemplatePrefab);
            instance.GetComponent<PlanetObject>().planet = planet;
            instance.GetComponent<PlanetObject>().inputHandler = inputHandler;
            instance.transform.position = new Vector2(planet.CoordX, planet.CoordY) / 10;
            instance.name = planet.Name;
        }
    }

}
