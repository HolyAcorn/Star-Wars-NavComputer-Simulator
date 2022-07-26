using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [HideInInspector]
    public bool isActive;
    float hue;
    float saturation;
    float value;
    Color color;
    public GameObject attachedButton;
    public ColorList colors;
    public ColorList savedColors;
    public GameObject colorSwatch;
    public Sprite addSwatchSprite;
    public ThemeSelector themeSelector;
    [SerializeField] GameObject swatchParent;

    [SerializeField] List<GameObject> sliders;


    private void Start()
    {
        CreateSwatches();
        UpdateSliders();
    }
    public void SetHue(float h)
    {
        hue = h;
        color = Color.HSVToRGB(hue, saturation, value);
        SetColor(color);
    }

    public void SetSaturation(float s)
    {
        saturation = s;
        color = Color.HSVToRGB(hue, saturation, value);
        SetColor(color);
    }

    public void SetValue(float v)
    {
        value = v;
        color = Color.HSVToRGB(hue, saturation, value);
        SetColor(color);
    }

    public void UpdateSliders()
    {
        RectTransform handle = null;
        foreach (GameObject slider in sliders)
        {
            for (int p = 0; p < slider.transform.childCount; p++)
            {
                if (slider.transform.GetChild(p).name == "Handle Slide Area")
                {
                    handle = slider.transform.GetChild(p).GetChild(0).GetComponent<RectTransform>();
                }
            }
            Slider s = slider.GetComponent<Slider>();
            switch (slider.name)
            {
                case "Hue":
                    s.SetValueWithoutNotify(hue);
                    handle.anchorMin = new Vector2(hue, 0);
                    handle.anchorMax = new Vector2(hue, 1);
                    break;
                case "Sat":
                    s.SetValueWithoutNotify(saturation);
                    handle.anchorMin = new Vector2(saturation, 0);
                    handle.anchorMax = new Vector2(saturation, 1);
                    break;
                case "Val":
                    s.SetValueWithoutNotify(value);
                    handle.anchorMin = new Vector2(value, 0);
                    handle.anchorMax = new Vector2(value, 1);
                    break;
                default:
                    break;
            }
        }
    }



    public void SetColor(Color color)
    {
        
        if (colors.Colors.Count == 0 || colors.Colors == null)
        {
            colors.Colors = new List<Color>();
            colors.Colors.Add(color);
        }
        else
        {
            colors.Colors[0] = color;
        }
        attachedButton.GetComponent<ColorPickerButton>().SetButtonColor();
        themeSelector.SetActiveStyleSettingManually();

    }

    public void OpenColorPicker()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }

    public void CreateSwatches()
    {
        GameObject instance = CreateSwatch(new Color(1,1,1));
        instance.GetComponent<Image>().sprite = addSwatchSprite;
        instance.GetComponent<Button>().onClick.RemoveAllListeners();
        instance.GetComponent<Button>().onClick.AddListener(CreateSwatchListener);
        for (int i = 0; i < savedColors.Colors.Count; i++)
        {
            Color color = savedColors.Colors[i];
            CreateSwatch(color);
        }
    }

    void CreateSwatchListener()
    {
        CreateSwatch(color);
        savedColors.Colors.Add(color);
    }

    public GameObject CreateSwatch(Color color)
    {
        GameObject instance = Instantiate(colorSwatch);
        instance.GetComponent<RectTransform>().parent = swatchParent.transform;
        instance.GetComponent<RectTransform>().localPosition = new Vector3(instance.transform.position.x, instance.transform.position.y, 0);
        instance.transform.localScale = new Vector3(1, 1, 1);
        instance.GetComponent<ColorSwatch>().colorPicker = this;
        instance.GetComponent<Image>().color = color;
        return instance;
    }
}
