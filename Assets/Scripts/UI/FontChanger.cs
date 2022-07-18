using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontChanger : MonoBehaviour
{
    public FontObject activeFont;
    Text[] textObjects;

    // Start is called before the first frame update
    void Start()
    {
        textObjects = Object.FindObjectsOfType<Text>();
    }

    public void ChangeFont()
    {
        foreach (Text text in textObjects)
        {
            if (text.font != activeFont.Font)
            {
                text.font = activeFont.Font;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        ChangeFont();
    }
}