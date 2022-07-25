using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    FontChanger fontChanger;

    [Header("Panels")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject settingsPanel;

    [Header("Settings")]
    [SerializeField] List<FontObject> fontList;
    [SerializeField] Dropdown fontDropDown;
    [SerializeField] FontObject activeFont;
    
    // Start is called before the first frame update
    void Start()
    {
        fontChanger = GetComponent<FontChanger>();
        PopulateFontDropDown();
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        fontChanger.ChangeFont();

    }


    #region SETTINGS
    public void SettingsBackButtonPressed() 
    { 
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        fontChanger.ChangeFont();

    }

    private void PopulateFontDropDown()
    {
        List<string> fontNames = new List<string>();
        foreach (FontObject fontObject in fontList)
        {
            fontNames.Add(fontObject.name);
        }
        fontDropDown.AddOptions(fontNames);
    }

    public void OnFontDropDownItemChange(int index)
    {
        activeFont.Font = fontList[index].Font;
        fontChanger.ChangeFont();
    }


    #endregion
}
