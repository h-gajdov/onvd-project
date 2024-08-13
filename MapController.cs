using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float planeRadius = 16f;
    private Transform plane;

    public static MapController instance;
    
    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        GameMath.LookAtTransform(camera , GameManager.planet, true);
        
        Vector3 newPosition = Coordinate.CoordinateToPoint(Player.GetPlayerCoords()) * radius;
        camera.position = newPosition;
        Vector3 lookDirection = Player.instance.planeTransform.forward;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection, GameManager.GetPlanetDirection(plane.position));
        plane.rotation = lookRotation;
    }
    
    public static void InstantiatePlane(GameObject plane)
    {
        float eps = (plane.name == "Speedster") ? 0.25f : 0.5f;
        GameObject p = Instantiate(plane);
        p.transform.parent = instance.camera;
        p.transform.localPosition = new Vector3(0f, 0f, instance.planeRadius);
        p.transform.localScale = Vector3.one * eps;
        instance.plane = p.transform;

        TrailRenderer[] trails = p.GetComponentsInChildren<TrailRenderer>();
        foreach(var trail in trails) Destroy(trail.gameObject);
    }
}
