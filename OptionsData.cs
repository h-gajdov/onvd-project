using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OptionsData : ScriptableObject
{
    public static float sfxVolume;
    public static float musicVolume;
    public static bool fullscreen;
    public static Resolution resolution;
    
    public static void ApplySettings(float sfx, float music, bool full, Resolution res)
    {
        sfxVolume = sfx;
        musicVolume = music;
        fullscreen = full;
        resolution = res;
    }
}
