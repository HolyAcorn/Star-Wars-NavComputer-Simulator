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

    [SerializeField] FloatReference currentCameraSize;
    public InputHandler inputHandler;

    private List<GameObject> instances = new List<GameObject>();

    private void Start()
    {
        PlacePlanets();
    }

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
            instances.Add(instance);
        }
    }

    public void CheckCameraSize()
    {
        if (currentCameraSize.Value < 9.0f) ChangeSize(0.5f);
        if (currentCameraSize.Value < 29.0f && currentCameraSize.Value > 9.0f) ChangeSize(1);
        if (currentCameraSize.Value > 29.0f) ChangeSize(2);


    }

    public void ChangeSize(float amount)
    {
        foreach (GameObject instance in instances)
        {
            instance.transform.localScale = new Vector3(amount,amount,amount);
        }
    }

}
