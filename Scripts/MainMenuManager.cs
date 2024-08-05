using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject leaderboardPanel;

    [Space]
    [Header("Play Panel")] 
    [SerializeField] private Material planeMaterial;
    [SerializeField] private Image red;
    [SerializeField] private Transform planeSelector;
    [SerializeField] private GameObject[] planes;
    private int selectedPlane = -1;
    private int numberOfRounds;
    private Image selectedColor;

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
    private bool fullscreenMode;

    [Space] [Header("Leaderboard Panel")] 
    [SerializeField] private GameObject leaderboardUserPrefab;
    [SerializeField] private Transform scrollViewport;
    [SerializeField] private Sprite[] medals;
    
    private void Start()
    {
        ShowButtonsPanel();
        planeSelector.gameObject.SetActive(false);
        selectedColor = red;
        planeMaterial.color = planeSelector.GetComponent<Image>().color = selectedColor.color;
        selectedPlane = -1;
        
        FillResolutionsList();
        
        LeaderboardUser.SetSprites(medals[0], medals[1], medals[2]);
        LeaderboardManager.GetLeaderboard(leaderboardUserPrefab, scrollViewport);
        
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
    }

    private void HideAllButtons()
    {
        playPanel.SetActive(false);
        optionsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        buttonsPanel.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ShowButtonsPanel();
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
        playPanel.SetActive(false);
        optionsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        buttonsPanel.SetActive(true);
    }

    public void SelectColor(Image image)
    {
        selectedColor.GetComponent<Button>().enabled = true;
        selectedColor.color = new Color(selectedColor.color.r, selectedColor.color.g, selectedColor.color.b, 1f);
        
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
        musicImage.sprite = (musicSlider.value == 0) ? crossedMusic : uncrossedMusic;
    }

    public void OnSoundChange()
    {
        soundImage.sprite = (soundSlider.value == 0) ? crossedSound : uncrossedSound;
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = fullscreenMode = !Screen.fullScreen;
    }

    public void ChangeScene()
    {
        if (selectedPlane == -1)
        {
            Debug.Log("Select a plane!");
            return;
        }
        SceneManager.LoadScene(1);
        HideAllButtons();
        StartCoroutine(GameManager.InitializeGame(numberOfRounds, planes[selectedPlane]));
    }

    public void SelectResolution()
    {
        int index = resolutionsDropdown.value;
        Resolution resolution = availableResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, fullscreenMode);
    }
}
