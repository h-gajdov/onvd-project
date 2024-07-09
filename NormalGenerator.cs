using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NormalGenerator : MonoBehaviour
{
    public ComputeShader computeShader;
    public Texture2D heightMap;
    public RenderTexture resultMap;
    public float worldRadius = 600f;
    public float heightMultiplier = 5f;
    public int mapWidth = 8192;
    public int mapHeight = 8192;

    private int kernelHandle;
    private void Start()
    {
        resultMap = new RenderTexture(mapWidth, mapHeight, 24);
        resultMap.enableRandomWrite = true;
        resultMap.Create();

        computeShader.FindKernel("CalculateHeightData");
        computeShader.SetTexture(kernelHandle, "HeightMap", heightMap);
        computeShader.SetTexture(kernelHandle, "ResultMap", resultMap);
        computeShader.SetFloat("worldRadius", worldRadius);
        computeShader.SetFloat("heightMultiplier", heightMultiplier);
        computeShader.SetInt("mapWidth", mapWidth);
        computeShader.SetInt("mapHeight", mapHeight);

        int threadGroupsX = Mathf.CeilToInt(mapWidth / 8f);
        int threadGroupsY = Mathf.CeilToInt(mapHeight / 8f);
        computeShader.Dispatch(kernelHandle, threadGroupsX, threadGroupsY, 1);
        SaveRenderTexture();
    }

    private void SaveRenderTexture()
    {
        RenderTexture.active = resultMap;

        Texture2D texture = new Texture2D(mapWidth, mapHeight, TextureFormat.ARGB32, false);
        texture.ReadPixels(new Rect(0,0,mapWidth,mapHeight),0,0);
        texture.Apply();
        File.WriteAllBytes(Application.dataPath + "/test.png", texture.EncodeToPNG());
    }
}
