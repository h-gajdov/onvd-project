using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject explanationPanel;
    [SerializeField] private GameObject aboutPanel;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private GameSettings gameSettings;

    [Space] [Header("Play Panel")] 
    [SerializeField] private Material planeMaterial;
    [SerializeField] private TMP_InputField roundsField;
    [SerializeField] private Image red;
    [SerializeField] private Image playButton;
    [SerializeField] private Transform planeSelector;
    [SerializeField] private GameObject[] planes;
    
    public Transform feedback;
    public GameObject errorPrefab;
    
    private int selectedPlane = -1;
    private int numberOfRounds = 0;
    private Image selectedColor;

    [Space] [Header("DifficultyPanel")] 
    [SerializeField] private Image playButtonDifficulty;
    [SerializeField] private Toggle showCompassToggle;
    [SerializeField] private Toggle showCountryNameToggle;
    private int difficulty = -1;
    private Image selectedDifficulty;

    [Space] [Header("Options Panel")] 
    [SerializeField] private Image musicImage;
    [SerializeField] private Image soundImage;
    [SerializeField] private Image applyButton;
    [SerializeField] private Image setToDefaultButton;
    [SerializeField] private Sprite crossedMusic;
    [SerializeField] private Sprite uncrossedMusic;
    [SerializeField] private Sprite crossedSound;
    [SerializeField] private Sprite uncrossedSound;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    public static Resolution[] availableResolutions;
    private int selectedResolution;
    private bool fullscreenMode;
    private bool hasChange = false;

    [Space] [Header("Leaderboard Panel")] 
    [SerializeField] private GameObject leaderboardUserPrefab;
    [SerializeField] private Transform scrollViewport;
    [SerializeField] private Sprite[] medals;

    public static MainMenuManager instance;
    
    private void Start()
    {
        ShowButtonsPanel();
        planeSelector.gameObject.SetActive(false);
        SelectColor(red);
        planeMaterial.color = planeSelector.GetComponent<Image>().color = new Color(selectedColor.color.r, selectedColor.color.g, selectedColor.color.b, 1f);
        selectedPlane = -1;
        numberOfRounds = 0;
        selectedDifficulty = null;
        instance = this;
        
        LeaderboardUser.SetSprites(medals[0], medals[1], medals[2]);
        LeaderboardManager.GetLeaderboard(leaderboardUserPrefab, scrollViewport);
        OnHover.SetMouseFollower(explanationPanel);
        AudioManager.UpdateSFXVolume(soundSlider.value);
        
        DontDestroyOnLoad(gameObject);
        LoadSettingsData();
        FillResolutionsList();
    }

    private void FillResolutionsList()
    {
        resolutionsDropdown.ClearOptions();
        // availableResolutions = Screen.resolutions;
        availableResolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();
        Array.Reverse(availableResolutions);
        foreach (Resolution res in availableResolutions)
        {
            string text = res.width + "x" + res.height;
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData(text);
            resolutionsDropdown.options.Add(data);
        }

        string label = availableResolutions[0].width + "x" + availableResolutions[0].height;
        resolutionsDropdown.captionText.text = label;
    }

    private void HideAllButtons()
    {
        playPanel.SetActive(false);
        optionsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        buttonsPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ShowButtonsPanel();
        
        EnableOrDisableButton(playButton, CheckIfCanSelectDifficulty());
        EnableOrDisableButton(playButtonDifficulty, CheckIfCanPlay());
        EnableOrDisableButton(applyButton, hasChange);
        EnableOrDisableButton(setToDefaultButton, OptionsData.CanChangeToDefault());
    }

    public static void EnableOrDisableButton(Image button, bool value)
    {
        if (value)
        {
            button.color = button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            button.GetComponent<Button>().enabled = true;
        }
        else
        {
            Color grey = new Color(0.4716981f, 0.4716981f, 0.4716981f);
            button.color = button.GetComponentInChildren<TextMeshProUGUI>().color = grey;
            button.GetComponent<Button>().enabled = false;
        }
    }

    public void Play()
    {
        buttonsPanel.SetActive(false);
        playPanel.SetActive(true);
    }
    
    public void Options()
    {
        LoadSettingsData();
        
        buttonsPanel.SetActive(false);
        optionsPanel.SetActive(true);

        hasChange = false;
    }

    public void SetToDefault()
    {
        OptionsData.SetDefaultSettings();
        LoadSettingsData();
        Apply();
    }

    public void LoadSettingsData()
    {
        OptionsData.LoadSettings();
        
        soundSlider.value = OptionsData.sfxVolume;
        OnMusicChange(OptionsData.musicVolume);
        Screen.fullScreen = OptionsData.fullscreen;
        fullScreenToggle.isOn = OptionsData.fullscreen;
        fullscreenMode = OptionsData.fullscreen;
        SelectResolution(OptionsData.resolutionIndex);
    }

    public void Leaderborad()
    {
        buttonsPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ShowButtonsPanel()
    {
        HideAllButtons();
        buttonsPanel.SetActive(true);
    }

    public void SelectColor(Image image)
    {
        if (selectedColor != null)
        {
            selectedColor.GetComponent<Button>().enabled = true;
            selectedColor.color = new Color(selectedColor.color.r, selectedColor.color.g, selectedColor.color.b, 1f);
        }
        
        Color color = image.color;
        planeMaterial.color = color;
        planeSelector.GetComponent<Image>().color = color;
        color.a = 0.2f;
        image.color = color;
        image.GetComponent<Button>().enabled = false;
        
        selectedColor = image;
    }

    public void SelectPlane(Transform plane)
    {
        selectedPlane = plane.GetSiblingIndex();
        planeSelector.gameObject.SetActive(true);
        planeSelector.position = plane.position;
    }

    public void OnMusicChange()
    {
        musicSource.volume = musicSlider.value;
        musicImage.sprite = (musicSlider.value == 0) ? crossedMusic : uncrossedMusic;
        OptionsData.UpdateSound(OptionsData.sfxVolume, musicSlider.value);
        hasChange = true;
    }

    private void OnMusicChange(float value)
    {
        musicSource.volume = value;
        musicSlider.value = value;
        musicImage.sprite = (value == 0) ? crossedMusic : uncrossedMusic;
        OptionsData.UpdateSound(OptionsData.sfxVolume, value);
        hasChange = true;
    }
    
    public void OnSoundChange()
    {
        AudioManager.UpdateSFXVolume(soundSlider.value);
        soundImage.sprite = (soundSlider.value == 0) ? crossedSound : uncrossedSound;
        OptionsData.UpdateSound(soundSlider.value, OptionsData.musicVolume);
        hasChange = true;
    }

    public void ToggleFullscreen()
    {
        fullscreenMode = !fullscreenMode;
        hasChange = true;
    }

    public void ChangeScene()
    {
        StartCoroutine(LoadingPanelManager.LoadLevelAsync(1));
        HideAllButtons();
        gameSettings.SetProperties(numberOfRounds, planes[selectedPlane], difficulty, showCountryNameToggle.isOn, showCompassToggle.isOn);
    }
    
    public void ShowDifficultyPanel()
    {
        playPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void BackToPlay()
    {
        playPanel.SetActive(true);
        difficultyPanel.SetActive(false);
    }

    private bool CheckIfCanSelectDifficulty()
    {
        if (selectedPlane == -1 || numberOfRounds <= 4) return false;
        return true;
    }

    public static string GetErrorMessage()
    {
        if (instance.selectedPlane == -1) return ErrorMesages.planeError;
        if (instance.numberOfRounds <= 4) return ErrorMesages.numberOfCitiesError;
        if (instance.difficulty == -1) return ErrorMesages.difficultyError;
        return "";
    }
    
    private bool CheckIfCanPlay()
    {
        return difficulty != -1;
    }

    public void SetNumberOfRounds()
    {
        int rounds = (roundsField.text.Length > 0) ? int.Parse(roundsField.text) : 0;
        numberOfRounds = rounds;
    }
    
    public void SelectResolution()
    {
        int index = resolutionsDropdown.value;
        selectedResolution = index;
        hasChange = true;
    }

    private void SelectResolution(int index)
    {
        selectedResolution = index;
        resolutionsDropdown.value = index;
        Screen.SetResolution(availableResolutions[index].width, availableResolutions[index].height, fullscreenMode);
    }

    public void Apply()
    {
        OptionsData.SaveSettings(soundSlider.value, musicSlider.value, fullscreenMode, selectedResolution);
        
        Screen.SetResolution(availableResolutions[selectedResolution].width, availableResolutions[selectedResolution].height, fullscreenMode);
        Screen.fullScreen = fullscreenMode;
        hasChange = false;
    }

    public void SelectDifficulty(Image difficultyImage)
    {
        if (selectedDifficulty != null)
        {
            selectedDifficulty.color = selectedDifficulty.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            selectedDifficulty.GetComponent<Button>().enabled = true;
        }

        difficulty = difficultyImage.transform.GetSiblingIndex();
        selectedDifficulty = difficultyImage;
        selectedDifficulty.color = selectedDifficulty.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.4716981f, 0.4716981f, 0.4716981f);
        selectedDifficulty.GetComponent<Button>().enabled = false;
    }
    
    public void About()
    {
        buttonsPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(2);
        HideAllButtons();
    }
}

public class ErrorMesages
{
    public static string planeError = "Select one of the plane variants!";
    public static string numberOfCitiesError = "Enter a number larger than 4 for the number of cities!";
    public static string difficultyError = "Select a difficulty to play with!";
}