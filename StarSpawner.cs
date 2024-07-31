using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Random = UnityEngine.Random;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] private Material starMaterial;
    [SerializeField] private GameObject cometPrefab;
    [SerializeField] private Gradient starGradient;
    [SerializeField] private float spawnDistance = 45f;
    [SerializeField] private float maxIntensity = 5f;
    [SerializeField] private int seed = 0;
    [SerializeField] private int numberOfStaticStars = 10;
    [SerializeField] private int numberOfMovingStars = 10;
    [SerializeField] private int numberOfComets = 10;

    private float timer = 0f;
    
    private void Start()
    {
        Random.InitState(seed);
        
        SpawnStaticStars();
        
        Random.InitState(System.Environment.TickCount);
    }

    private void SpawnStaticStars()
    {
        for (int i = 0; i < numberOfStaticStars; i++)
        {
            float gradientValue = Random.Range(0, 1f);
            float intensity = Random.Range(3f, 5f);
            Color color = starGradient.Evaluate(gradientValue);
            GameObject star = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            star.name = "Star";
            Vector3 spawnPosition = GameMath.GetRandomScreenToWorldPosition(spawnDistance);
            star.transform.parent = transform;
            star.transform.position = spawnPosition;
            star.AddComponent<MainMenuStar>();
            star.GetComponent<MeshRenderer>().material = starMaterial;
            star.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", color * intensity);
        }
    }

    private void SpawnComet()
    {
        GameObject comet = Instantiate(cometPrefab);
        comet.name = "Comet";
        comet.transform.parent = transform;
    }
    
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            timer = 0f;
            SpawnComet();
        }
    }
}
