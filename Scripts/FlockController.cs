using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlockController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Vector3 point;
    private Transform flockDirection;
    private CharacterController controller;
    
    private void Awake()
    {
        point = GameMath.GetRandomPointOnEarth(GameManager.planetRadius);
        controller = gameObject.AddComponent<CharacterController>();
        flockDirection = transform.GetChild(0);

        flockDirection.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    private void Update()
    {
        controller.Move( speed * Time.deltaTime * flockDirection.up);

        GameMath.LookAtTransform(transform, GameManager.planet);
        
        // Vector3 direction = (point - transform.position).normalized;
        // Vector3 up = GameManager.GetPlanetDirection(transform.position).normalized;
        // Vector3 forward = direction - up * Vector3.Dot(direction, up);
        // Quaternion lookRotation = Quaternion.LookRotation(forward.normalized);
        //
        // float singleStep = speed * Mathf.Deg2Rad * Time.deltaTime;
        // float distanceFromTarget = Vector3.Distance(GameMath.FixVertexOnSphere(transform.position), point);
        //
        // transform.position = Vector3.RotateTowards(transform.position, point, singleStep, 0f);
        // transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, turnSmoothTime * Time.deltaTime);
        //
        // if(distanceFromTarget <= 3f) 
        //     point = GameMath.GetRandomPointOnEarth(GameManager.planetRadius);
    }
}
