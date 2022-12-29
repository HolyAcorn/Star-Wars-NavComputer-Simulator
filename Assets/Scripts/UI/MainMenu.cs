using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MainMenu : MonoBehaviour
{
    FontChanger fontChanger;

    [Header("Panels")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject creditsPanel;

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
        creditsPanel.SetActive(false);
        mainMenuPanel.transform.localScale = new Vector3(0,0,0);
        mainMenuPanel.transform.localPosition = new Vector3(0, mainMenuStartY, 0);
        LeanTween.scale(mainMenuPanel, new Vector3(1, 1, 1), mainMenuDelayTime).setDelay(0.1f).setEase(mainMenuInType);
        LeanTween.moveLocalY(mainMenuPanel, 0.0f, mainMenuDelayTime).setDelay(0.1f).setEase(mainMenuInType);
    }
    

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void FlipSettings(bool value)
    {
        mainMenuPanel.SetActive(!value);
        settingsPanel.SetActive(value);
        if (value)
        {
            fontChanger.ChangeFont();
            hyperLaneData.Value = hyperLaneDataInputField.text;
        }


    }


    public void SetLoadingFalse()
    {
        isLoading = false;
    }

    public void Load()
    {
        SceneManager.LoadScene(1);

    }

    public void FlipCredits(bool value)
    {
        mainMenuPanel.SetActive(!value);
        creditsPanel.SetActive(value);
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }



    #region SETTINGS

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
