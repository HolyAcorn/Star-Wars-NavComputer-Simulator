using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [SerializeField] GameObject dataPanel;

    [Header("Tweening")]
    [SerializeField] float mainMenuDelayTime;
    [SerializeField] LeanTweenType mainMenuInType;
    [SerializeField] float mainMenuStartY = 570;
    [Space(10)]

    [Header("Settings")]
    [SerializeField] List<FontObject> fontList;
    [SerializeField] Dropdown fontDropDown;
    [SerializeField] FontObject activeFont;

    [Header("Loading")]
    [SerializeField] GameEvent readFromJson;
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

    public void OpenDataPanel()
    {
        mainMenuPanel.SetActive(false);
        dataPanel.SetActive(true);
    }

    public void LoadMap()
    {
        LoadAsynchronously();

    }

    public void LoadJson()
    {
        if (!isLoading)
        {
            isLoading = true;
            dataPanel.SetActive(false);
            loadingPanel.SetActive(true);

            readFromJson.Raise();
        }
    }
    public void SetLoadingFalse()
    {
        isLoading = false;
    }

    async void LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;


        do
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingText.text = progressDescription.Value;
            //loadingSlider.value = progress.Value;
            loadingSlider.value = progress;

        } while (operation.progress < 0.9f);


        operation.allowSceneActivation = true;

    }

    #region SETTINGS
    public void BackToMainMenu() 
    { 
        settingsPanel.SetActive(false);
        dataPanel.SetActive(false);
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
        fontDropDown.value = activeFont.Index;
    }

    public void OnFontDropDownItemChange(int index)
    {
        activeFont.Font = fontList[index].Font;
        activeFont.Index = index;
        fontChanger.ChangeFont();
    }


    #endregion
}
