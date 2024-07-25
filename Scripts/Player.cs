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

    private CharacterController controller;
    private Transform planeTransform;

    public static Player instance;

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
        
        controller = GetComponent<CharacterController>();
        planeTransform = transform.GetChild(0);

        transform.position = new Vector3(startElevation, 0f, 0f);
        Vector3 lookDirection = GameManager.GetPlanetDirection(transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = lookRotation;
        moveSpeed = idleSpeed;
    }
    
    private void Update()
    {
        LookAtPlanet();
        Move();
        
        if(Input.GetKeyDown(KeyCode.Space)) DropPackage();
    }

    private void LookAtPlanet()
    {
        Vector3 lookDirection = GameManager.GetPlanetDirection(transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = lookRotation;
    }
    
    private void Move()
    {
        int changesHeight = 0;
        float speed = (Input.GetKey(KeyCode.W)) ? accelerationSpeed : idleSpeed;
        float turnDirection = Input.GetAxisRaw("Horizontal");
        float turnAmount = turnDirection * turnSmoothTime * Time.deltaTime;
        float distanceFromPlanet = GameManager.GetDistanceFromPlanet(transform.position);
        
        moveSpeed = Mathf.Lerp(moveSpeed, speed, speedSmoothTime * Time.deltaTime);
        
        controller.Move(moveSpeed * Time.deltaTime * planeTransform.right);

        if (Input.GetKey(KeyCode.Q) && distanceFromPlanet > minElevation)
        {
            controller.Move(-transform.forward * heightChangeSpeed * Time.deltaTime);
            changesHeight = 1;
        }
        else if (Input.GetKey(KeyCode.E) && distanceFromPlanet < maxElevation)
        {
            controller.Move(transform.forward * heightChangeSpeed * Time.deltaTime);
            changesHeight = -1;
        }
        
        UpdateRotation(turnAmount, turnDirection, changesHeight);
    }

    private void UpdateRotation(float turnAmount, float direction, int changesHeight)
    {
        float tiltAngle = (direction != 0f) ? -direction * 45f : 0f;
        Quaternion tiltRotation = Quaternion.Euler(tiltAngle, changesHeight * 45f, 0f);
        planeTransform.Rotate(0f, 0f, turnAmount);
        planeTransform.GetChild(0).localRotation = Quaternion.Slerp(planeTransform.GetChild(0).localRotation,
            tiltRotation, tiltSmoothTime * Time.deltaTime);
    }

    private void DropPackage()
    {
        Vector3 gravityDown = GameManager.GetPlanetDirection(transform.position);
        GameObject package = Instantiate(packagePrefab, transform.position, Quaternion.identity);
        package.transform.rotation = Quaternion.LookRotation(gravityDown);
        Destroy(package, packageLifetime);
    }
}
