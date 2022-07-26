using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerButton : MonoBehaviour
{
    public GameObject colorPickerPrefab;
    public ColorList color;
    public Vector3 offset;
    GameObject instance = null;
    public ThemeSelector themeSelector;

    public void SetButtonColor()
    {
        GetComponent<Image>().color = color.Colors[0];
    }

    public void OpenColor()
    {
        if (instance == null)
        {
            instance = Instantiate(colorPickerPrefab);
            instance.GetComponent<ColorPicker>().isActive = false;
            instance.GetComponent<ColorPicker>().attachedButton = gameObject;
            instance.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position + offset;
            instance.transform.parent = transform.parent.parent.parent;
            instance.transform.localScale = new Vector3(1, 1, 1);
            instance.GetComponent<ColorPicker>().themeSelector = themeSelector;
        }
        instance.GetComponent<ColorPicker>().OpenColorPicker();
    }
}
