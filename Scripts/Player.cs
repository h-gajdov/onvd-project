using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Plane Settings")]
    [SerializeField] private Transform planet;
    [SerializeField] private float idleSpeed = 1f;
    [SerializeField] private float accelerationSpeed = 2f;
    [SerializeField] private float heightChangeSpeed = 0.5f;
    [SerializeField] private float turnSmoothTime = 1f;
    [SerializeField] private float tiltSmoothTime = 1f;

    [Space]
    [Header("Elevation Settings")]
    [SerializeField] private float minElevation = 620f;
    [SerializeField] private float maxElevation = 670f;
    private float startElevation = 645f;

    private CharacterController controller;
    private Transform planeTransform;
    private Transform cam;
    private Vector3 move;
    
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        planeTransform = transform.GetChild(0);
        cam = Camera.main.transform;

        transform.position = new Vector3(startElevation, 0f, 0f);
        Vector3 lookDirection = new Vector3(transform.position.x - planet.position.x,
            transform.position.y - planet.position.y, transform.position.z - planet.position.z);
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = lookRotation;
    }
    
    private void Update()
    {
        LookAtPlanet();
        Move();
    }

    private void LookAtPlanet()
    {
        Vector3 lookDirection = new Vector3(transform.position.x - planet.position.x,
            transform.position.y - planet.position.y, transform.position.z - planet.position.z);
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = lookRotation;
    }
    
    private void Move()
    {
        int changesHeight = 0;
        float speed = (Input.GetKey(KeyCode.W)) ? accelerationSpeed : idleSpeed;
        float turnDirection = Input.GetAxisRaw("Horizontal");
        float turnAmount = turnDirection * turnSmoothTime * Time.deltaTime;
        float distanceFromPlanet = Vector3.Distance(planet.position, transform.position);
        controller.Move(speed * Time.deltaTime * planeTransform.right);

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
}
