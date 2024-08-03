using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject leaderboardPanel;

    private void Start()
    {
        ShowButtonsPanel();
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
}
