using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;

    private Vector3 stopPoint;
    private bool landed = false;
    private float stopDistance;
    
    private void Start()
    {
        stopPoint = CalculateStopPoint();
        stopDistance = CalculateStopDistance();
        rb.AddForce(GameManager.GetPlanetDirection(transform.position) * Player.instance.GravityMultiplier);
    }

    private void Update()
    {
        float planetDistance = GameManager.GetDistanceFromPlanet(transform.position);
        if (planetDistance > stopDistance || landed) return;

        landed = true;
        transform.position = stopPoint;
        anim.SetBool("hasFallen", true);
        CalculateDrop();
        Destroy(rb);
        // Destroy(this);
    }

    private void CalculateDrop()
    {
        Coordinate packageCoordinate = Coordinate.PointToCoordinate(stopPoint);
        float distance =
            GameMath.DistanceBetweenCoordinatesOnEarth(packageCoordinate, GameManager.selectedCity.coordinate);
        GameManager.AddScore(GameMath.CalculateScoreFromDistance(distance));
        StartCoroutine(UIManager.ShowCityButtons());
        GameManager.ShowCity();
        // GameManager.SetRandomCity();
    }
    
    private float CalculateStopDistance()
    {
        return Coordinate.PointToCoordinate(transform.position).GetHeight() * EarthGenerator.instance.heightMultiplier +
               GameManager.planetRadius;
    }
    
    private Vector3 CalculateStopPoint()
    {
        Coordinate coord = Coordinate.PointToCoordinate(transform.position);
        Vector3 result = Coordinate.CoordinateToPoint(coord);
        return result * (coord.GetHeight() * EarthGenerator.instance.heightMultiplier +
               GameManager.planetRadius);
    }
}
