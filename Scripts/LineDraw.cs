using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineDraw : MonoBehaviour
{
    public TextAsset countryData;
    public ComputeShader CS;
    public float thickness = 1;
    public bool t;
    
    private int[] offsets;
    
    private Vector2[] GetAllCountryGeometry()
    {
        CountryJSONReader.SetJSONFile(countryData);
        Country[] countries = CountryJSONReader.ReadAllCountries();
        List<Vector2> result = new List<Vector2>();
        List<int> offsetResult = new List<int>();
        
        foreach (Country country in countries)
        {
            foreach (CountryPolygon polygon in country.polygons)
            {
                foreach(Vector2 point in polygon.geometry) result.Add(point);
                offsetResult.Add(polygon.geometry.Length);
            }
        }

        offsets = offsetResult.ToArray();
        return result.ToArray();
    }

    private Vector2[] GetCountryGeometry(Country country)
    {
        List<int> offsetResult = new List<int>();
        List<Vector2> result = new List<Vector2>();
        foreach (CountryPolygon polygon in country.polygons)
        {
            foreach(Vector2 point in polygon.geometry) result.Add(point);
            offsetResult.Add(polygon.geometry.Length);
        }
        
        offsets = offsetResult.ToArray();
        return result.ToArray();
    }
    
    private void Start()
    {
        offsets = new int[0];
        CountryJSONReader.SetJSONFile(countryData);
        Country country = CountryJSONReader.ReadCountry("Russia");
        Vector2[] points = GetCountryGeometry(country);

        DrawGeometry(GetAllCountryGeometry());
    }

    private void DrawGeometry(Vector2[] geometry)
    {
        ComputeBuffer positionsBuffer = new ComputeBuffer(geometry.Length, sizeof(float) * 2, ComputeBufferType.Default);
        ComputeBuffer offsetsBuffer = new ComputeBuffer(offsets.Length, sizeof(int), ComputeBufferType.Default);
        RenderTexture RT = new RenderTexture(16384, 8192, 0);
        RT.enableRandomWrite = true;
        RT.Create();
        CS.SetTexture(0, "surface", RT);
        positionsBuffer.SetData(geometry);
        offsetsBuffer.SetData(offsets);
        CS.SetBuffer(0, "positions", positionsBuffer);
        CS.SetBuffer(0, "offsets", offsetsBuffer);
        CS.SetInt("numOfPositions", geometry.Length);
        CS.SetInt("numOfOffsets", offsets.Length);
        CS.SetFloat("thickness", thickness);
        CS.Dispatch(0, RT.width / 8, RT.height / 8, 1);
        // GetComponent<Renderer>().material.mainTexture = RT;
        
        Texture2D tex = new Texture2D(RT.width, RT.height, TextureFormat.RGB24, false);
        RenderTexture.active = RT;
        tex.ReadPixels(new Rect(0, 0, RT.width, RT.height), 0, 0);
        tex.Apply();
        
        string fileName = "borders.png";
        File.WriteAllBytes(Application.dataPath + "/" + fileName, tex.EncodeToPNG());
    }
}