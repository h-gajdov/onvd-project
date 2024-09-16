using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExploreUIManager : MonoBehaviour
{
    [SerializeField] private GameObject countryInfoPanel;
    [SerializeField] private GameObject minimizedInfoPanel;
    [SerializeField] private TextMeshProUGUI countryName;
    [SerializeField] private TextMeshProUGUI countryNameMinimized;
    [SerializeField] private TextMeshProUGUI capitalCityText;
    [SerializeField] private TextMeshProUGUI countryDescription;
    [SerializeField] private Image flag;
    [SerializeField] private Image minimizedFlag;
    [SerializeField] private float glideSpeed = 5f;

    public static ExploreUIManager instance;

    private Vector3 startInfoPosition;
    private Vector3 hidePos;
    
    private void Start()
    {
        instance = this;
        startInfoPosition = countryInfoPanel.GetComponent<RectTransform>().anchoredPosition;
        hidePos = new Vector3(startInfoPosition.x, 0f, 0f);
        Minimize();
    }

    public void Minimize()
    {
        countryInfoPanel.SetActive(false);
        minimizedInfoPanel.SetActive(true);
        
    }

    public void Maximize()
    {
        StopAllCoroutines();
        countryInfoPanel.GetComponent<RectTransform>().anchoredPosition = hidePos;
        countryInfoPanel.SetActive(true);
        minimizedInfoPanel.SetActive(false);
        StartCoroutine(MoveInfoPanelToStartPosition());
    }

    private IEnumerator MoveInfoPanelToStartPosition()
    {
        RectTransform rectTransform = countryInfoPanel.GetComponent<RectTransform>();
        while (rectTransform.anchoredPosition.y < startInfoPosition.y - 5f)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, startInfoPosition,
                glideSpeed * Time.deltaTime);
            yield return null;
        }

        rectTransform.anchoredPosition = startInfoPosition;
    }
    
    public static void SetValues(string countryName, string countryDescription, Sprite flag, string capitalName)
    {
        instance.countryName.text = countryName;
        instance.countryDescription.text = countryDescription;
        instance.flag.sprite = flag;
        instance.minimizedFlag.sprite = flag;
        instance.countryNameMinimized.text = countryName;
        instance.capitalCityText.text = "Capital: " + capitalName;
    }
}
