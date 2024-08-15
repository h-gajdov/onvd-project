using System;
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
    private int difficulty = -1;
    private bool showCountryName = true;
    private Image selectedDifficulty;

    [Space] [Header("Options Panel")] 
    [SerializeField] private Image musicImage;
    [SerializeField] private Image soundImage;
    [SerializeField] private Sprite crossedMusic;
    [SerializeField] private Sprite uncrossedMusic;
    [SerializeField] private Sprite crossedSound;
    [SerializeField] private Sprite uncrossedSound;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    private Resolution[] availableResolutions;
    private Resolution selectedResolution;
    private bool fullscreenMode;

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
        
        FillResolutionsList();
        
        LeaderboardUser.SetSprites(medals[0], medals[1], medals[2]);
        LeaderboardManager.GetLeaderboard(leaderboardUserPrefab, scrollViewport);
        OnHover.SetMouseFollower(explanationPanel);
        AudioManager.UpdateSFXVolume(soundSlider.value);
        
        DontDestroyOnLoad(gameObject);
    }

    private void FillResolutionsList()
    {
        resolutionsDropdown.ClearOptions();
        availableResolutions = new Resolution[Screen.resolutions.Length];
        Resolution[] resolutions = Screen.resolutions;
        for (int i = resolutions.Length - 1; i >= 0; i--)
        {
            string text = resolutions[i].width + "x" + resolutions[i].height;
            availableResolutions[resolutions.Length - 1 - i] = resolutions[i];
            TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData(text);
            resolutionsDropdown.options.Add(data);
        }

        string label = resolutions[resolutions.Length - 1].width + "x" + resolutions[resolutions.Length - 1].height;
        resolutionsDropdown.captionText.text = label;
        selectedResolution = availableResolutions[0];
    }

    private void HideAllButtons()
    {
        playPanel.SetActive(false);
        optionsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        buttonsPanel.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ShowButtonsPanel();

        EnableOrDisableButton(playButton, CheckIfCanSelectDifficulty());
        EnableOrDisableButton(playButtonDifficulty, CheckIfCanPlay());
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
        buttonsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void Leaderborad()
    {
        buttonsPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }

    public void Exit()
    {
        Debug.Log("Quit Game!");
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
    }

    public void OnSoundChange()
    {
        AudioManager.UpdateSFXVolume(soundSlider.value);
        soundImage.sprite = (soundSlider.value == 0) ? crossedSound : uncrossedSound;
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = fullscreenMode = !Screen.fullScreen;
    }

    public void ToggleShowCountryName()
    {
        showCountryName = !showCountryName;
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
        HideAllButtons();
        gameSettings.SetProperties(numberOfRounds, planes[selectedPlane], difficulty, showCountryName);
        //Destroy(gameObject);
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
        selectedResolution = availableResolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenMode);
    }

    public void Apply()
    {
        OptionsData.ApplySettings(soundSlider.value, musicSlider.value, fullscreenMode, selectedResolution);
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
}

public class ErrorMesages
{
    public static string planeError = "Select one of the plane variants!";
    public static string numberOfCitiesError = "Enter a number larger than 4 for the number of cities!";
    public static string difficultyError = "Select a difficulty to play with!";
}