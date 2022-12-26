using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwNavComp
{
    public class PlanetCreatorPresenter : MonoBehaviour
    {
        [SerializeField] GameObject planetTemplatePrefab;
        [SerializeField] FloatReference sizeDifference;
        [SerializeField] FloatReference currentCameraSize;

        public StyleSettings styleSetting;

        [SerializeField] List<Sprite> sprites;
        [SerializeField] PlanetRuntimeSet planetList;

        List<GameObject> instances = new List<GameObject>();


        public void PlacePlanets()
        {
            foreach (Planet planet in planetList.items)
            {
                CreatePlanet(planet);
            }
        }
        
        private void CreatePlanet(Planet planet)
        {
            int random = Random.Range(0, sprites.Count-1);
            GameObject instance = Instantiate(planetTemplatePrefab);
            SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();

            PlanetObjectManager planetObjectManager = instance.GetComponent<PlanetObjectManager>();
            planetObjectManager.planet = planet;

            instance.transform.position = new Vector3(planet.CoordX, planet.CoordY) / sizeDifference.Value;
            spriteRenderer.sprite = sprites[random];
            spriteRenderer.color = styleSetting.PlanetColor;

            instance.name = planet.name;
            instances.Add(instance);
           instance.transform.parent = transform;
        }

        public void CheckCameraSize()
        {

        }

        public void ChangeSize(float amount)
        {

        }
    }
}
