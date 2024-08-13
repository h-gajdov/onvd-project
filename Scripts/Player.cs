using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Plane Settings")]
    [SerializeField] private float idleSpeed = 1f;
    [SerializeField] private float accelerationSpeed = 2f;
    [SerializeField] private float heightChangeSpeed = 0.5f;
    [SerializeField] private float speedSmoothTime = 1f;
    [SerializeField] private float turnSmoothTime = 1f;
    [SerializeField] private float tiltSmoothTime = 1f;
    private float moveSpeed;

    [Space]
    [Header("Elevation Settings")]
    [SerializeField] private float minElevation = 620f;
    [SerializeField] private float maxElevation = 670f;
    private float startElevation = 645f;
    
    [Space]
    [Header("Package Settings")]
    [SerializeField] private GameObject packagePrefab;
    [SerializeField] private float gravityMultiplier = -9.81f;
    [SerializeField] private float packageLifetime = 100;
    
    [HideInInspector] public Transform planeTransform;

    public static Player instance;
    private static bool move = true;
    
    public float GravityMultiplier
    {
        get
        {
            return gravityMultiplier;
        }
    }
    
    private void Start()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        planeTransform = transform.GetChild(0);

        transform.position = new Vector3(startElevation, 0f, 0f);
        Vector3 lookDirection = GameManager.GetPlanetDirection(transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = lookRotation;
        moveSpeed = idleSpeed;
    }
    
    private void Update()
    {
        Move();

        if(Input.GetKeyDown(KeyCode.Space)) DropPackage();
    }

    private float height = 45f;
    private void Move()
    {
        if (!move) return;
        
        int changesHeight = 0;
        float speed = (Input.GetKey(KeyCode.W)) ? accelerationSpeed : idleSpeed;
        float turnDirection = Input.GetAxisRaw("Horizontal");
        float turnAmount = turnDirection * turnSmoothTime * Time.deltaTime;
        float distanceFromPlanet = GameManager.GetDistanceFromPlanet(transform.position);
        
        Vector3 newPos = transform.position + planeTransform.forward * speed * Time.deltaTime;
        Vector3 gravityUp = newPos.normalized;
        newPos = Vector3.zero + gravityUp * (GameManager.planetRadius + height);
        transform.position = newPos;
        transform.rotation = Quaternion.FromToRotation(transform.forward, gravityUp) * transform.rotation;

        if (Input.GetKey(KeyCode.Q) && distanceFromPlanet > minElevation)
        {
            height -= heightChangeSpeed * Time.deltaTime;
            changesHeight = 1;
        }
        else if (Input.GetKey(KeyCode.E) && distanceFromPlanet < maxElevation)
        {
            height += heightChangeSpeed * Time.deltaTime;
            changesHeight = -1;
        }
        
        UpdateRotation(turnAmount, turnDirection, changesHeight);
    }

    private void UpdateRotation(float turnAmount, float direction, int changesHeight)
    {
        float tiltAngle = (direction != 0f) ? -direction * 45f : 0f;
        Quaternion tiltRotation = Quaternion.Euler(changesHeight * 45f, 0f, tiltAngle);
        planeTransform.Rotate(0f, turnAmount, 0f);
        planeTransform.GetChild(0).localRotation = Quaternion.Slerp(planeTransform.GetChild(0).localRotation,
            tiltRotation, tiltSmoothTime * Time.deltaTime);
    }

    private void DropPackage()
    {
        Vector3 gravityDown = GameManager.GetPlanetDirection(transform.position);
        GameObject package = Instantiate(packagePrefab, transform.position, Quaternion.identity);
        package.transform.rotation = Quaternion.LookRotation(gravityDown);
        GameManager.SetLastTarget();
        Destroy(package, packageLifetime);
    }

    public static void SetMovement(bool value)
    {
        move = value;
    }

    public static Coordinate GetPlayerCoords()
    {
        return Coordinate.PointToCoordinate(instance.transform.position);
    }

    public static float GetElevation()
    {
        return Vector3.Distance(instance.transform.position, GameManager.planet.position);
    }
}
