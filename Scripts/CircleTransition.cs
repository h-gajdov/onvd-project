using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransition : MonoBehaviour
{
    private Canvas canvas;
    private Image blackScreen;
    private Material material;

    private Rect canvasRect;
    private float canvasWidth;
    private float canvasHeight;

    [SerializeField] private float circleSpeed = 1f;
    [SerializeField] private float alphaSpeed = 1f;
    [SerializeField] private float showValue = 0.5f;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private Image[] images;
    [SerializeField] private Button[] buttons;
    private float radius;
    private float alpha = 0f;
    
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        blackScreen = GetComponent<Image>();
        material = blackScreen.material;
        
        canvasRect = canvas.GetComponent<RectTransform>().rect;
        canvasWidth = canvasRect.width;
        canvasHeight = canvasRect.height;

        SetAlphaOfUIElements(texts, 0f);
        SetAlphaOfUIElements(images, 0f);
        ToggleButtons(false);

        if (OptionsData.playedOpenAnimation)
        {
            material.SetFloat("_Radius", 1f);
            StartCoroutine(Alpha());
            return;
        }
        StartCoroutine(Circle());
        OptionsData.playedOpenAnimation = true;
    }

    private void SetAlphaOfUIElements(TextMeshProUGUI[] array, float a)
    {
        foreach (var t in array)
        {
            Color col = t.color;
            t.color = new Color(col.r, col.g, col.b, a);
        }
    }
    
    private void SetAlphaOfUIElements(Image[] array, float a)
    {
        foreach (var t in array)
        {
            Color col = t.color;
            t.color = new Color(col.r, col.g, col.b, a);
        }
    }

    private void ToggleButtons(bool toggle)
    {
        foreach (var button in buttons) button.enabled = toggle;
    }
    
    private void Update()
    {
        DrawBlackScreen();
        if (Input.GetKeyDown(KeyCode.R)) radius = alpha = 0f;
    }

    private void DrawBlackScreen()
    {
        float dimensionValue = (canvasWidth > canvasHeight) ? canvasWidth : canvasHeight;
        blackScreen.rectTransform.sizeDelta = new Vector2(dimensionValue, dimensionValue);
    }

    private IEnumerator Circle()
    {
        bool started = false;
        yield return null;
        while (radius <= 1f)
        {
            radius = Mathf.Lerp(radius, 1f, circleSpeed * Time.deltaTime);
            material.SetFloat("_Radius", radius);
            
            if (radius >= showValue && !started)
            {
                started = true;
                StartCoroutine(Alpha());
            }
            yield return null;
        }
    }
    
    private IEnumerator Alpha()
    {
        while (alpha <= 1)
        {
            alpha = Mathf.Lerp(alpha, 1f, alphaSpeed * Time.deltaTime);
            SetAlphaOfUIElements(texts, alpha);
            SetAlphaOfUIElements(images, alpha);

            if(alpha >= 0.3f) ToggleButtons(true);
            
            if (alpha >= 0.95f) break;
            
            yield return null;
        }
        
        radius = alpha = 1f;
        material.SetFloat("_Radius", radius);
        SetAlphaOfUIElements(texts, 1f);
        SetAlphaOfUIElements(images, 1f);
        StopAllCoroutines();
    }
}
