using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SwNavComp
{
    public class PlanetScrollviewItemPresenter : MonoBehaviour
    {
        [SerializeField] TMP_Text orderText;
        [SerializeField] TMP_Text IDText;
        [SerializeField] TMP_InputField planetNameInputField;
        [SerializeField] TMP_InputField xCoordInputField;
        [SerializeField] TMP_InputField yCoordInputField;


        Vector2 coords;

        public void Initialize(Planet planet, int i = 0)
        {
            name = planet.displayName;
            coords = new Vector2(planet.CoordX, planet.CoordY);


            orderText.SetText(i.ToString());
            IDText.SetText(planet.ID.ToString());
            planetNameInputField.SetTextWithoutNotify(name);
            xCoordInputField.SetTextWithoutNotify(coords.x.ToString());
            yCoordInputField.SetTextWithoutNotify(coords.y.ToString());
        }

    }
}
