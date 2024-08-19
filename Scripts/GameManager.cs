using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public enum Difficulty {Easy, Medium, Hard}
    public static Difficulty difficulty;
    
    public static Transform planet;
    private static GameObject cityMarker;
    public static float planetRadius = 600f;

    [SerializeField] private TextAsset cityJson;
    [SerializeField] private TextMeshProUGUI cityText;
    [SerializeField] private GameObject winPanel;
    private TextMeshProUGUI countryText;

    public static City[] cities;
    public static City selectedCity;

    public static Vector3 lastTarget = Vector3.zero;
    public static GameManager instance;

    public static int numberOfRounds;
    private static int playedRounds;
    private static float totalScore;

    public static bool canTakeCity = false;
    public static int cityIndex = 0;

    [SerializeField] private GameSettings gameSettings;
    public bool isTutorial = true;
    
    private void Start()
    {
        if(instance == null) instance = this;
        
        planet = GameObject.FindGameObjectWithTag("Planet").transform;
        cityMarker = Resources.Load("Prefabs/CityMarker/CityMarker") as GameObject;
        countryText = cityText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        playedRounds = 0;
        totalScore = 0;
        canTakeCity = false;

        StartCoroutine(InitializeGame(gameSettings));
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
        canTakeCity = false;
        CameraControler.canTakeInput = false;
        instance.StopAllCoroutines();
        instance.StartCoroutine(UIManager.HideCityButtons());
        
        selectedCity = cities[cityIndex++];
        cityIndex = (cityIndex >= cities.Length) ? 0 : cityIndex;
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
    
    public static IEnumerator InitializeGame(GameSettings settings)
    {
        int rounds = settings.rounds;
        GameObject plane = settings.plane;
        int diff = settings.diff;
        bool showCountryName = settings.showCountryName;
        
        numberOfRounds = rounds;
        yield return null;
        GameObject p = Instantiate(plane);
        p.transform.parent = Player.instance.transform;
        p.transform.position = Player.instance.transform.position;
        p.transform.localRotation = Quaternion.Euler(0f, 90f, 90f);
        Player.instance.planeTransform = p.transform;
        p.transform.SetSiblingIndex(0);
        instance.countryText.gameObject.SetActive(showCountryName);

        MapController.InstantiatePlane(plane);
        UIManager.SetRoundsUI(playedRounds, numberOfRounds);
        UIManager.SetScoreUI(0f);
        
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
        CityJSONReader.ShuffleCities(cities);
        if (instance.isTutorial) cities = CityJSONReader.ReadTutorialCities();
        
        SetRandomCity();

        lastTarget = Vector3.zero;
        if(MainMenuManager.instance != null) Destroy(MainMenuManager.instance.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && canTakeCity)
        {
            SetRandomCity();
            Player.canDropPackage = true;
        }
    }

    public static void AddScore(float score)
    {
        totalScore += score;
        playedRounds++;
        UIManager.SetRoundsUI(playedRounds, numberOfRounds);
        UIManager.SetScoreUI(totalScore);
        UIManager.ShowFeedbackScore(score);
        if (playedRounds == numberOfRounds) UIManager.instance.StartCoroutine(EndGame());
    }

    private static IEnumerator EndGame()
    {
        float score = totalScore / numberOfRounds;
        instance.winPanel.SetActive(true);
        yield return null;
        WinPanelManager.SetProperties(totalScore, score);
    }
}
