using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlockController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float flyHeight = 10f;

    private Vector3 point;
    private Transform flockDirection;
    
    private void Awake()
    {
        point = GameMath.GetRandomPointOnEarth(GameManager.planetRadius);
        flockDirection = transform.GetChild(0);

        flockDirection.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    private void Update()
    {
        Vector3 newPos = transform.position + flockDirection.up * speed * Time.deltaTime;
        Vector3 gravityUp = newPos.normalized;
        newPos = Vector3.zero + gravityUp * (GameManager.planetRadius + flyHeight);
        transform.position = newPos;
        transform.rotation = Quaternion.FromToRotation(transform.forward, gravityUp) * transform.rotation;
    }
}
