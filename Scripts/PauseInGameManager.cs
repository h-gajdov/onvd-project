using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseInGameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;
    
    private void Start()
    {
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) Pause();
    }

    private void Pause()
    {
        // Player.instance
        pausePanel.SetActive(!pausePanel.activeInHierarchy);
        Time.timeScale = (pausePanel.activeInHierarchy) ? 0 : 1;
    }
    
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Options()
    {
        optionsPanel.SetActive(true);
    }

    public void Back()
    {
        optionsPanel.SetActive(false);
    }

    public void Apply()
    {
        optionsPanel.SetActive(false);
        Debug.Log("IMPLEMENT APPLY BUTTON");
    }
    
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
