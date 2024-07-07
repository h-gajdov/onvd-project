using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static Transform planet;
    public static float planetRadius = 600f;

    private void OnValidate()
    {
        planet = GameObject.FindGameObjectWithTag("Planet").transform;
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
}
