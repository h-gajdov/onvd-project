using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField] private GameObject birdPrefab;

    [SerializeField] private float spawnHeight = 10;
    [SerializeField] private int numberOfFlocks = 10;
    [SerializeField] private int maxBirdsPerFlock = 8;

    private void Start()
    {
        for (int i = 0; i < numberOfFlocks; i++)
        {
            GameObject flock = new GameObject("Flock");
            flock.AddComponent<FlockController>();
            flock.transform.parent = transform;
            flock.transform.position = GameMath.GetRandomPointOnEarth(GameManager.planetRadius + spawnHeight);

            GameMath.LookAtTransform(flock.transform, GameManager.planet);
            
            int numberOfBirds = Random.Range(4, maxBirdsPerFlock + 1);
            for (int j = 0; j < numberOfBirds; j++)
            {
                Transform bird = Instantiate(birdPrefab).transform;
                float x = Random.Range(-5f, 5f);
                float y = Random.Range(-1f, 1f);
                float z = Random.Range(-5f, 5f);
                
                bird.parent = flock.transform;
                bird.localPosition = new Vector3(x, z, y);
                bird.localRotation = Quaternion.identity;
            }
        }
    }
}
