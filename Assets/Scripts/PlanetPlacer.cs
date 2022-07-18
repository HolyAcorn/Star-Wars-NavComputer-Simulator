using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPlacer : MonoBehaviour
{
    public PlanetList planetList;
    public GameObject planetTemplatePrefab;

    public StyleSettings styleSetting;
    public List<Sprite> sprites;
    public FloatReference sizeDifference;

    public InputHandler inputHandler;

    public void PlacePlanets()
    {
        for (int i = 0; i < planetList.Planets.Count; i++)
        {
            int r = Random.Range(0, sprites.Count-1);
            Planet planet = planetList.Planets[i];
            GameObject instance = Instantiate(planetTemplatePrefab);
            instance.GetComponent<SpriteRenderer>().sprite = sprites[r];
            instance.GetComponent<SpriteRenderer>().color = styleSetting.PlanetColor;
            instance.GetComponent<PlanetObject>().planet = planet;
            instance.GetComponent<PlanetObject>().inputHandler = inputHandler;
            instance.transform.position = new Vector2(planet.CoordX, planet.CoordY) / sizeDifference.Value;
            instance.transform.parent = transform;
            instance.name = planet.Name;
        }
    }

}
