using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI uiText;
    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    private void Awake()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        frameDeltaTimeArray = new float[50];
        Debug.Log(QualitySettings.vSyncCount);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P)) uiText.enabled = !uiText.enabled;
        
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;
        uiText.text = "FPS:" + Mathf.RoundToInt(CalculateFPS()).ToString();
    }

    private float CalculateFPS()
    {
        float total = 0f;
        foreach (float delta in frameDeltaTimeArray)
        {
            total += delta;
        }
        return frameDeltaTimeArray.Length / total;
    }
}
