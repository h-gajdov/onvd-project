using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Random = UnityEngine.Random;

public class Boat : MonoBehaviour
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private float speed = 5f;
    private static float distanceToBeSpawned = 0;
    private float distanceTravelled;

    private void Start()
    {
        distanceTravelled = distanceToBeSpawned;
        distanceToBeSpawned += pathCreator.path.length / 5f;
    }

    public void Initialize(float value, PathCreator path)
    {
        pathCreator = path;
        distanceTravelled = value;
    }

    private void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = GameMath.FixVertexOnSphere(pathCreator.path.GetPointAtDistance(distanceTravelled));
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }
}
