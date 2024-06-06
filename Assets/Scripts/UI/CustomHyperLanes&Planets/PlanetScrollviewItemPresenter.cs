using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SwNavComp
{
    namespace HLEditor
    {
        public class PlanetScrollviewItemPresenter : MonoBehaviour
        {
            [SerializeField] TMP_Text orderText;
            [SerializeField] TMP_Text IDText;
            [SerializeField] TMP_InputField planetNameInputField;
            [SerializeField] TMP_InputField yCoordInputField;
            [SerializeField] TMP_InputField xCoordInputField;

            [SerializeField] CustomHyperLanePlanetUIPresenter hyperLanePlanetUIPresenter;

            Planet planet;
            Vector2 coords;

            string displayName;
            int id = 0;
            int orderID;

            public void Initialize(CustomHyperLanePlanetUIPresenter customHyperLane, Planet _planet, int i = 0)
            {
                hyperLanePlanetUIPresenter = customHyperLane;
                planet = _planet;
                name = planet.displayName;
                coords = new Vector2(planet.CoordX, planet.CoordY);
                id = planet.ID;
                orderID = i;

                orderText.SetText(i.ToString());
                IDText.SetText("ID: " + planet.ID.ToString());
                planetNameInputField.SetTextWithoutNotify(name);
                xCoordInputField.SetTextWithoutNotify(coords.x.ToString());
                yCoordInputField.SetTextWithoutNotify(coords.y.ToString());
            }


            public void UpdateName(string value)
            {
                name = value;
                UpdatePlanet();
            }

            public void UpdateCoordX(string value)
            {
                coords.x = float.Parse(value);
                UpdatePlanet();
            }

            public void UpdateCoordY(string value)
            {
                coords.y = float.Parse(value);
                UpdatePlanet();
            }

            public void OnMovePlanet(bool up)
            {
                hyperLanePlanetUIPresenter.ChangePlanetOrder(orderID, up);
            }

            private void UpdatePlanet()
            {
                planet.Initialize(name, id, coords.x, coords.y, false);
            }

            public Planet SavePlanet()
            {
                Planet newPlanet = new Planet();
                newPlanet.HyperlaneRoutes = planet.HyperlaneRoutes;
                newPlanet.Initialize(name, id, coords.x, coords.y, false);
                return newPlanet;
            }

            public void RemovePlanet()
            {
                hyperLanePlanetUIPresenter.RemovePlanet(planet);
            }
        }

    }
}
