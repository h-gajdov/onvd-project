using System;
using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    
    private Button lastButton;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void Update()
    {
        Button button = PointingButton();
        if (button == null)
        {
            lastButton = button;
            return;
        }
        if(lastButton != button && button.enabled) PlayHoverSound(button);

        if (Input.GetMouseButtonDown(0))
        {
            Play("ButtonClick");
        }
        
        lastButton = button;
    }

    private void PlayHoverSound(Button button)
    {
        Sound hover = FindSoundByName("ButtonHover");
        AudioSource source = button.gameObject.AddComponent<AudioSource>();
        source.clip = hover.clip;
        source.volume = OptionsData.sfxVolume;
        source.Play();
        Destroy(source, source.clip.length);
    }
    
    public static Button PointingButton()
    {
        return PointingButton(GameMath.GetEventSystemRaycastResults());
    }
    
    private static Button PointingButton(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            Button button = curRaysastResult.gameObject.GetComponent<Button>();
            if (button != null)
                return button;
        }
        return null;
    }

    public static Sound FindSoundByName(string name)
    {
        return Array.Find(instance.sounds, sound => sound.name == name);
    }

    public static void UpdateSFXVolume(float volume)
    {
        OptionsData.sfxVolume = volume;
        foreach (Sound s in instance.sounds)
        {
            if (!s.sfx) continue;
            s.source.volume = s.volume = volume;
        }
    }
    
    public static void Play(string name)
    {
        Sound sound = Array.Find(instance.sounds, sound => sound.name == name);
        if (sound == null) return;
        sound.source.Play();
    }
}