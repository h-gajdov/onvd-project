using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseInGameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;
    
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
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    private int selectedResolution;
    private bool fullscreenMode;
    private bool canResume = false;
    private bool hasChange = false;
    
    private void Start()
    {
        LoadSettingsData();
    }

    private IEnumerator WaitForInit()
    {
        yield return null;
        canResume = true;
    }
    
    private void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Escape) && canResume) Resume();
        
        MainMenuManager.EnableOrDisableButton(applyButton, hasChange);
        MainMenuManager.EnableOrDisableButton(setToDefaultButton, OptionsData.CanChangeToDefault());
    }
    
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        canResume = false;
    }

    public void Options()
    {
        LoadSettingsData();
        
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
        musicSlider.value = OptionsData.musicVolume;
        Screen.fullScreen = OptionsData.fullscreen;
        fullScreenToggle.isOn = OptionsData.fullscreen;
        fullscreenMode = OptionsData.fullscreen;
        SelectResolution(OptionsData.resolutionIndex);
    }
    
    public void OnMusicChange()
    {
        musicImage.sprite = (musicSlider.value == 0) ? crossedMusic : uncrossedMusic;
        OptionsData.UpdateSound(soundSlider.value, musicSlider.value);
        hasChange = true;
    }

    public void OnSoundChange()
    {
        AudioManager.UpdateSFXVolume(soundSlider.value);
        Player.instance.SetSFXVolume(soundSlider.value);
        soundImage.sprite = (soundSlider.value == 0) ? crossedSound : uncrossedSound;
        OptionsData.UpdateSound(soundSlider.value, musicSlider.value);
        hasChange = true;
    }

    public void ToggleFullscreen()
    {
        fullscreenMode = !fullscreenMode;
        hasChange = true;
    }

    public void Back()
    {
        optionsPanel.SetActive(false);
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
        Screen.SetResolution(MainMenuManager.availableResolutions[index].width, MainMenuManager.availableResolutions[index].height, fullscreenMode);
    }

    public void Apply()
    {
        OptionsData.SaveSettings(soundSlider.value, musicSlider.value, fullscreenMode, selectedResolution);
        
        Screen.SetResolution(MainMenuManager.availableResolutions[selectedResolution].width, MainMenuManager.availableResolutions[selectedResolution].height, fullscreenMode);
        Screen.fullScreen = fullscreenMode;
        hasChange = false;
    }
    
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
