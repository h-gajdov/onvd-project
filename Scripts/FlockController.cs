using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private CharacterController controller;

    private Vector3 point;
    
    private void Awake()
    {
        controller = gameObject.AddComponent<CharacterController>();
        point = GameMath.GetRandomPointOnEarth(GameManager.planetRadius);

        GameObject a = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        a.transform.position = point;
    }

    private void Update()
    {
        Vector3 direction = (point - transform.position).normalized;
        Vector3 up = GameManager.GetPlanetDirection(transform.position).normalized;
        Vector3 forward = direction - up * Vector3.Dot(direction, up);
        Quaternion lookRotation = Quaternion.LookRotation(forward.normalized);
        
        float singleStep = speed * Mathf.Deg2Rad * Time.deltaTime;
        float distanceFromTarget = Vector3.Distance(GameMath.FixVertexOnSphere(transform.position), point);
        
        transform.position = Vector3.RotateTowards(transform.position, point, singleStep, 0f);
        transform.rotation = lookRotation;
        
        if(distanceFromTarget <= 3f) 
            point = GameMath.GetRandomPointOnEarth(GameManager.planetRadius);
        
        // controller.Move( speed * Time.deltaTime * transform.up);
        // GameMath.LookAtTransform(transform, GameManager.planet);
    }
}
