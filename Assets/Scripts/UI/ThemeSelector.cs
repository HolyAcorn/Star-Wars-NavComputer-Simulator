using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSelector : MonoBehaviour
{
    public List<StyleSettings> styleSettings;
    public StyleSettings activeStyleSetting;
    public ColorList planetColor;
    public ColorList hyperlaneColor;
    public ColorList pathColor;
    public ColorList gridColor;
    public GameEvent updateColors;

    Dropdown dropDown;

    private void Start()
    {
        PopulateDropdown();
    }

    public void PopulateDropdown()
    {
        dropDown = GetComponent<Dropdown>();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (StyleSettings style in styleSettings)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            string name = style.name.Split('_')[1];
            optionData.text = name;
            options.Add(optionData);
        }
        dropDown.AddOptions(options);
    }

    public void SetTheme(int index)
    {
        activeStyleSetting.PlanetColor = styleSettings[index].PlanetColor;
        activeStyleSetting.HyperLaneColor = styleSettings[index].HyperLaneColor;
        activeStyleSetting.PathColor = styleSettings[index].PathColor;
        activeStyleSetting.GridColor = styleSettings[index].GridColor;
        planetColor.Colors[0] = activeStyleSetting.PlanetColor;
        hyperlaneColor.Colors[0] = activeStyleSetting.HyperLaneColor;
        pathColor.Colors[0] = activeStyleSetting.PathColor;
        gridColor.Colors[0] = activeStyleSetting.GridColor;
        updateColors.Raise();
    }



    public void SetActiveStyleSettingManually()
    {
        activeStyleSetting.PlanetColor = planetColor.Colors[0];
        activeStyleSetting.HyperLaneColor = hyperlaneColor.Colors[0];
        activeStyleSetting.PathColor = pathColor.Colors[0];
        activeStyleSetting.GridColor = gridColor.Colors[0];
    }
}
