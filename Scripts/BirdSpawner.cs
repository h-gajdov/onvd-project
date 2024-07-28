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
            GameObject flockDirection = new GameObject("FlockDirection");
            flockDirection.transform.parent = flock.transform;
            flockDirection.transform.localPosition = Vector3.zero;
            flock.AddComponent<FlockController>();
            flock.transform.parent = transform;
            flock.transform.position = GameMath.GetRandomPointOnEarth(GameManager.planetRadius + spawnHeight);
            
            int numberOfBirds = Random.Range(4, maxBirdsPerFlock + 1);
            for (int j = 0; j < numberOfBirds; j++)
            {
                Transform bird = Instantiate(birdPrefab).transform;
                float x = Random.Range(-5f, 5f);
                float y = Random.Range(-1f, 1f);
                float z = Random.Range(-5f, 5f);
                
                bird.parent = flockDirection.transform;
                bird.localPosition = new Vector3(x, z, y);
                bird.localRotation = Quaternion.identity;
            }
        }
        
        Destroy(this);
    }
}
