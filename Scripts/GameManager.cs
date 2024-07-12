using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static Transform planet;
    private static GameObject cityMarker;
    public static float planetRadius = 600f;

    public TextMeshProUGUI cityText;

    public static City[] cities;
    public static City selectedCity;

    private static GameManager instance;
    
    private void OnValidate()
    {
        if(instance == null) instance = this;
        else
        {
            Destroy(this);
            return;
        }
        
        planet = GameObject.FindGameObjectWithTag("Planet").transform;
        cityMarker = Resources.Load("Prefabs/CityMarker/CityMarker") as GameObject;
    }

    public static Vector3 GetPlanetDirection(Vector3 position)
    {
        return new Vector3(position.x - planet.position.x,
            position.y - planet.position.y, position.z - planet.position.z);
    }
    
    public static float GetDistanceFromPlanet(Vector3 position)
    {
        return Vector3.Distance(planet.position, position);
    }

    public static void SetRandomCity()
    {
        selectedCity = cities[Random.Range(0, cities.Length)];
        instance.cityText.text = selectedCity.name;
    }

    public static void ShowCity()
    {
        GameObject citySphere = Instantiate(cityMarker);
        TextMeshProUGUI cityText = citySphere.transform.GetComponentInChildren<TextMeshProUGUI>();
        citySphere.transform.position = selectedCity.worldPosition;
        cityText.text = selectedCity.name;
        Destroy(citySphere, 120f);
    }

    private void Start()
    {
        SetRandomCity();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) SetRandomCity();
    }
}
