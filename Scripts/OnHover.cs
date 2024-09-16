using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string title;
    [SerializeField] private string description;
    
    private static GameObject mouseFollower;
    private static TextMeshProUGUI titleTMP;
    private static TextMeshProUGUI descTMP;
    
    public static void SetMouseFollower(GameObject value)
    {
        mouseFollower = value;
        titleTMP = value.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        descTMP = value.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseFollower.SetActive(true);

        titleTMP.text = title;
        descTMP.text = description;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseFollower.SetActive(false);
    }

    private void OnDisable()
    {
        mouseFollower.SetActive(false);
    }
}
