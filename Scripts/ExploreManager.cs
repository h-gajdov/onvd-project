using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ExploreManager : MonoBehaviour
{
    [SerializeField] private TextAsset countryJson;
    [SerializeField] private TextAsset cityJson;
    [SerializeField] private TextAsset descritpionJson;
    [SerializeField] private Texture2D borderMap;

    public Transform cityMarker;
    private TextMeshProUGUI cityMarkerText;
    
    private static Country[] countries;
    
    private void Start()
    {
        CountryJSONReader.SetJSONFile(countryJson);
        CountryJSONReader.SetDescriptionJSONFile(descritpionJson);
        CityJSONReader.SetJSONFile(cityJson);
        countries = CountryJSONReader.ReadAllCountries();
        cityMarkerText = cityMarker.GetComponentInChildren<TextMeshProUGUI>();
        
        string capital = (countries[141].info.capitalCity != null) ? countries[141].info.capitalCity.name : "";
        ExploreUIManager.SetValues(countries[141].info.name, countries[141].info.description, countries[141].info.flag, capital);
        for(int i = 0; i < countries.Length; i++) Debug.Log(i.ToString() + " " + countries[i].info.name);
    }

    private Vector2 PointOnSphereToUV(Vector3 p)
    {
        p.Normalize();
        
        float latitude = Mathf.Asin(p.y);
        float longitude = Mathf.Atan2(p.z, p.x);
                    
        float u = (longitude / Mathf.PI + 1) / 2f;
        float v = latitude / Mathf.PI + 0.5f;
        return new Vector2(u, v);
    }

    private void Update()
    {
        Vector3 playerPosition = Player.instance.transform.position;
        Vector2 uv = PointOnSphereToUV(playerPosition);
        int pixelX = (int)(uv.x * borderMap.width);
        int pixelY = (int)(uv.y * borderMap.height);
        Color color = borderMap.GetPixel(pixelX, pixelY);
        int index = (int)((color.r * 255f) - 1);
        if (index < 0) return;
        UpdateValues(index);
    }

    private void UpdateValues(int index)
    {
        string capital = (countries[index].info.capitalCity != null) ? countries[index].info.capitalCity.name : "";
        ExploreUIManager.SetValues(countries[index].info.name, countries[index].info.description, countries[index].info.flag, capital);
        cityMarker.position = countries[index].info.capitalCity.worldPosition;
        cityMarker.rotation = Quaternion.LookRotation(cityMarker.position.normalized, Vector3.zero);
        cityMarkerText.text = capital;
    }
}
