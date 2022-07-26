using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSwatch : MonoBehaviour
{
    [HideInInspector]
    public ColorPicker colorPicker;

    public void SendToColorPicker()
    {
        Color color = GetComponent<Image>().color;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) == transform)
            {
                colorPicker.SetColor(color);
                colorPicker.UpdateSliders();
            }
        }
    }
}
