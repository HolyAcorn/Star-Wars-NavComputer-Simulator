using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    FontChanger fontChanger;

    [Header("Panels")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject loadingPanel;

    [Header("Tweening")]
    [SerializeField] float mainMenuDelayTime;
    [SerializeField] LeanTweenType mainMenuInType;
    [SerializeField] float mainMenuStartY = 570;
    [Space(10)]

    [Header("Settings")]
    [SerializeField] List<FontObject> fontList;
    [SerializeField] Dropdown fontDropDown;
    [SerializeField] FontObject activeFont;
    [SerializeField] StringVariable hyperLaneData;
    [SerializeField] TMP_InputField hyperLaneDataInputField;

    [Header("Loading")]
    [SerializeField] Slider loadingSlider;
    [SerializeField] Text loadingText;
    [SerializeField] FloatReference progress;
    [SerializeField] StringReference progressDescription;
    bool isLoading = false;

    // Start is called before the first frame update
    void Start()
    {
        fontChanger = GetComponent<FontChanger>();
        fontChanger.ChangeFont();
        PopulateFontDropDown();
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        mainMenuPanel.transform.localScale = new Vector3(0,0,0);
        mainMenuPanel.transform.localPosition = new Vector3(0, mainMenuStartY, 0);
        LeanTween.scale(mainMenuPanel, new Vector3(1, 1, 1), mainMenuDelayTime).setDelay(0.1f).setEase(mainMenuInType);
        LeanTween.moveLocalY(mainMenuPanel, 0.0f, mainMenuDelayTime).setDelay(0.1f).setEase(mainMenuInType);
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


    public void SetLoadingFalse()
    {
        isLoading = false;
    }

    public void Load()
    {
        SceneManager.LoadScene(1);

    }

    #region SETTINGS
    public void BackToMainMenu() 
    { 
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        fontChanger.ChangeFont();
        hyperLaneData.Value = hyperLaneDataInputField.text;

    }

    private void PopulateFontDropDown()
    {
        List<string> fontNames = new List<string>();
        foreach (FontObject fontObject in fontList)
        {
            fontNames.Add(fontObject.name);
        }
        fontDropDown.AddOptions(fontNames);
        fontDropDown.value = activeFont.Index;
    }

    public void OnFontDropDownItemChange(int index)
    {
        activeFont.Font = fontList[index].Font;
        activeFont.Index = index;
        activeFont.fontAsset = fontList[index].fontAsset;
        fontChanger.ChangeFont();
    }


    #endregion
}
