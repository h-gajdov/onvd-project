using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadingPanelManager : MonoBehaviour
{
    [SerializeField] private Slider loadSlider;

    public static LoadingPanelManager instance;
    
    private void Start()
    {
        instance = this;
        loadSlider.gameObject.SetActive(false);
    }

    public static IEnumerator LoadLevelAsync(int index)
    {
        instance.loadSlider.gameObject.SetActive(true);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(index);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            instance.loadSlider.value = progressValue;
            yield return null;
        }
        
        instance.loadSlider.gameObject.SetActive(false);
    }
}
