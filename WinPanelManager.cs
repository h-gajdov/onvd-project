using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class WinPanelManager : MonoBehaviour
{
    [SerializeField] private Image[] stars;
    [SerializeField] private Image submitToLeaderboardButton;
    [SerializeField] private TextMeshProUGUI totalScore;
    [SerializeField] private TextMeshProUGUI leaderboardScore;
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private Slider slider;
    [SerializeField] private Sprite litStar;
    [SerializeField] private Sprite unlitStar;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private TMP_InputField inputField;
    
    private float entryScore;
    private bool entryGiven = false;
    
    public static WinPanelManager instance;

    public float debugVar = 99.3f;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) SetProperties(797.7f, debugVar);
        MainMenuManager.EnableOrDisableButton(submitToLeaderboardButton, !entryGiven);
    }

    public static void SetProperties(float totalScore, float score)
    {
        instance.totalScore.text = "Total Score: " + totalScore.ToString("F1").Replace(',', '.');
        instance.leaderboardScore.text = "Leaderboard Score: " + score.ToString("F1").Replace(',', '.');

        int rankResult = LeaderboardManager.GetRankOfScore(score);
        string rankStr = rankResult.ToString();
        switch (rankResult)
        {
            case 1:
                rankStr += "ST";
                break;
            case 2:
                rankStr += "ND";
                break;
            case 3:
                rankStr += "RD";
                break;
            default: 
                rankStr += "TH";
                break;
        }
        
        instance.rank.text = "Rank: " + rankStr;
        
        int numberOfStarsLit;
        if (score >= 80f) numberOfStarsLit = 3;
        else if (score >= 30f) numberOfStarsLit = 2;
        else numberOfStarsLit = 1;

        for (int i = 0; i < numberOfStarsLit; i++) instance.stars[i].sprite = instance.litStar;
        for (int i = numberOfStarsLit; i < 3; i++) instance.stars[i].sprite = instance.unlitStar;

        instance.slider.value = score / 100f;
        instance.entryScore = score;
    }

    public void PlayAgain()
    {
        GameSettings.ReloadScene();
    }

    public void SubmitToLeaderboard()
    {
        leaderboardPanel.SetActive(true);
    }

    public void Close()
    {
        leaderboardPanel.SetActive(false);
    }

    public void ValidateName()
    {
        
        string result = "";
        for (int i = 0; i < inputField.text.Length && i < 10; i++)
        {
            char letter = inputField.text[i];
            if(!Char.IsLetter(letter)) continue;
            result += Char.ToUpper(letter);
        }
        inputField.text = result;
    }

    public void SubmitEntry()
    {
        LeaderboardManager.SetLeaderboardEntry(inputField.text, entryScore);
        leaderboardPanel.SetActive(false);
        entryGiven = true;
    }
}
