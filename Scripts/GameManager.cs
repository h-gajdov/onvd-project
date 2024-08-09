using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public enum Difficulty {Easy, Medium, Hard}
    public static Difficulty difficulty;
    
    public static Transform planet;
    private static GameObject cityMarker;
    public static float planetRadius = 600f;

    public TextAsset cityJson;
    public TextMeshProUGUI cityText;
    private TextMeshProUGUI countryText;

    public static City[] cities;
    public static City selectedCity;

    public static Vector3 lastTarget = Vector3.zero;
    private static GameManager instance;

    public static int numberOfRounds;
    private static int playedRounds;
    private static float totalScore;
    
    private void Start()
    {
        if(instance == null) instance = this;
        
        planet = GameObject.FindGameObjectWithTag("Planet").transform;
        cityMarker = Resources.Load("Prefabs/CityMarker/CityMarker") as GameObject;
        countryText = cityText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        playedRounds = 0;
    }

    public static Vector3 GetPlanetDirection(Vector3 position)
    {
        return position - planet.position;
    }
    
    public static float GetDistanceFromPlanet(Vector3 position)
    {
        return Vector3.Distance(planet.position, position);
    }

    public static void SetRandomCity()
    {
        selectedCity = cities[Random.Range(0, cities.Length)];
        instance.cityText.text = selectedCity.name;
        instance.countryText.text = selectedCity.countryName;
    }

    public static void SetLastTarget()
    {
        lastTarget = selectedCity.worldPosition;
    }
    
    public static void ShowCity()
    {
        GameObject citySphere = Instantiate(cityMarker);
        TextMeshProUGUI cityText = citySphere.transform.GetComponentInChildren<TextMeshProUGUI>();
        citySphere.transform.position = selectedCity.worldPosition;
        cityText.text = selectedCity.name;
        Destroy(citySphere, 120f);
    }

    public static IEnumerator InitializeGame(int rounds, GameObject plane, int diff, bool showCountryName)
    {
        numberOfRounds = rounds;
        yield return null;
        GameObject p = Instantiate(plane);
        p.transform.parent = Player.instance.transform;
        p.transform.position = Player.instance.transform.position;
        p.transform.localRotation = Quaternion.Euler(0f, 90f, 90f);
        Player.instance.planeTransform = p.transform;
        p.transform.SetSiblingIndex(0);
        instance.countryText.gameObject.SetActive(showCountryName);
        
        switch (diff)
        {
            case 0: difficulty = Difficulty.Easy; break;    
            case 1: difficulty = Difficulty.Medium; break;    
            case 2: difficulty = Difficulty.Hard; break;    
        }
        
        CityJSONReader.SetJSONFile(instance.cityJson);
        switch (difficulty)
        {
            case Difficulty.Easy:
                cities = CityJSONReader.ReadEasyCities();
                break;
            case Difficulty.Medium:
                cities = CityJSONReader.ReadAllCapitals();
                break;
            case Difficulty.Hard:
                cities = CityJSONReader.ReadHardCities();
                break;
        }
        
        SetRandomCity();
        lastTarget = Vector3.zero;

        Destroy(MainMenuManager.instance.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) SetRandomCity();
    }

    public static void AddScore(float score)
    {
        totalScore += score;
        playedRounds++;
        Debug.Log(score.ToString() + " " + numberOfRounds.ToString() + " " + playedRounds.ToString());
        if (playedRounds == numberOfRounds) EndGame();
    }

    private static void EndGame()
    {
        string name = "TEST";
        Debug.Log(totalScore);
        float score = totalScore / numberOfRounds;
        LeaderboardManager.SetLeaderboardEntry(name, score);
    }
}
