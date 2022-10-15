using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FontChanger : MonoBehaviour
{
    public FontObject activeFont;
    Text[] textObjects;

    // Start is called before the first frame update
    void Start()
    {
        ChangeFont();
    }

    

    public void ChangeFont()
    {
        textObjects = Resources.FindObjectsOfTypeAll<Text>();
        foreach (Text text in textObjects)
        {
            if (text.font != activeFont.Font)
            {
                text.font = activeFont.Font;
            }
        }
        TextMeshProUGUI[] textMeshPros = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in textMeshPros)
        {
            if (text.font != activeFont.Font) text.font = activeFont.fontAsset;
        }
    }

}
