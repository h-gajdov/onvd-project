using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsData
{
    public static float sfxVolume;
    public static float musicVolume;
    public static bool fullscreen;
    public static int resolutionIndex;

    public static void SetDefaultSettings()
    {
        SaveSettings(0.5f, 0.5f, true, 0);
        LoadSettings();
    }

    public static void DeleteSettings()
    {
        PlayerPrefs.DeleteAll();
    }
    
    public static void SaveSettings(float sfx, float music, bool full, int res)
    {
        PlayerPrefs.SetFloat("sfxVolume", sfx);
        PlayerPrefs.SetFloat("musicVolume", music);
        int fullscreenValue = (full) ? 1 : 0;
        PlayerPrefs.SetInt("fullscreen", fullscreenValue);
        PlayerPrefs.SetInt("selectedResolutionIndex", res);
        
        PlayerPrefs.SetInt("coldStart", 0);

        sfxVolume = sfx;
        musicVolume = music;
        fullscreen = full;
        resolutionIndex = res;
    }

    public static void UpdateSound(float sfx, float music)
    {
        PlayerPrefs.SetFloat("sfxVolume", sfx);
        PlayerPrefs.SetFloat("musicVolume", music);
            
        sfxVolume = sfx;
        musicVolume = music;
    }

    public static void LoadSettings()
    {
        if (PlayerPrefs.GetInt("coldStart", 1) == 1)
        {
            SetDefaultSettings();
            return;
        }
        
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        fullscreen = PlayerPrefs.GetInt("fullscreen") != 0;
        resolutionIndex = PlayerPrefs.GetInt("selectedResolutionIndex");
    }

    public static bool CanChangeToDefault()
    {
        return musicVolume != 0.5f || sfxVolume != 0.5f || !fullscreen || resolutionIndex != 0;
    }
}
