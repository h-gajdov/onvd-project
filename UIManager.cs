using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform cityButtons;
    [SerializeField] private Transform feedbackParent;
    [SerializeField] private GameObject feedbackScorePefab;
    [SerializeField] private TextMeshProUGUI roundsText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float speed = 1f;
    
    private static RectTransform seeCity;
    private static RectTransform nextCity;
    private static float statSpeed;
    private static bool hidden = true;

    private static UIManager instance;

    private void Start()
    {
        seeCity = cityButtons.GetChild(0).GetComponent<RectTransform>();
        nextCity = cityButtons.GetChild(1).GetComponent<RectTransform>();
        statSpeed = speed;
        instance = this;
        
        StartCoroutine(HideCityButtons());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) ShowFeedbackScore(95.3f);
    }

    public static IEnumerator HideCityButtons()
    {
        while (seeCity.position.x > -149f)
        {
            seeCity.anchoredPosition = Vector3.Lerp(seeCity.anchoredPosition, new Vector3(-200f, 0f, 0f), statSpeed * Time.deltaTime);   
            nextCity.anchoredPosition = Vector3.Lerp(nextCity.anchoredPosition, new Vector3(200f, 0f, 0f), statSpeed * Time.deltaTime);
            yield return null;
        }

        seeCity.anchoredPosition = new Vector3(-150, 0f, 0f);   
        nextCity.anchoredPosition = new Vector3(150f, 0f, 0f);
        
        hidden = true;
    }

    public static IEnumerator ShowCityButtons()
    {
        while (seeCity.position.x < 149f)
        {
            seeCity.anchoredPosition = Vector3.Lerp(seeCity.anchoredPosition, new Vector3(200f, 0f, 0f), statSpeed * Time.deltaTime);   
            nextCity.anchoredPosition = Vector3.Lerp(nextCity.anchoredPosition, new Vector3(-200f, 0f, 0f), statSpeed * Time.deltaTime);
            yield return null;
        }

        seeCity.anchoredPosition = new Vector3(150, 0f, 0f);   
        nextCity.anchoredPosition = new Vector3(-150f, 0f, 0f);
        
        hidden = false;
    }

    public static void SetRoundsUI(int playedRounds, int totalRounds)
    {
        instance.roundsText.text = playedRounds.ToString() + " / " + totalRounds.ToString();
    }

    public static void SetScoreUI(float score)
    {
        instance.scoreText.text = "Score: " + score.ToString("F1").Replace(',', '.');
    }

    public static void ShowFeedbackScore(float score)
    {
        GameObject feedback = Instantiate(instance.feedbackScorePefab, instance.feedbackParent);
        feedback.GetComponentInChildren<TextMeshProUGUI>().text = "+" + score.ToString("F1").Replace(',', '.');
        Destroy(feedback, 1f);
    }
}
