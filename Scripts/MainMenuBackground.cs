using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBackground : MonoBehaviour
{
    [SerializeField] private Image background;

    private void Update()
    {
        Texture2D noiseTexture = new Texture2D(Screen.width, Screen.height);
        for (int i = 0; i < Screen.width; i++)
        {
            for (int j = 0; j < Screen.height; j++)
            {
                float xCoord = (float)i / Screen.width;
                float yCoord = (float)j / Screen.width;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                Color color = new Color(sample, sample, sample);
                noiseTexture.SetPixel(i, j, color);
            }
        }
        noiseTexture.Apply();

        var bytes = noiseTexture.EncodeToPNG();
        Sprite sprite = Sprite.Create(noiseTexture, new Rect(0, 0, noiseTexture.width, noiseTexture.height), new Vector2(noiseTexture.width / 2, noiseTexture.height / 2));
        background.sprite = sprite;
    }
}
