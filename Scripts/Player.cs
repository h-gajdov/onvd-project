using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Plane Settings")]
    [SerializeField] private Transform planet;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float turnSmoothTime = 1f;

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
        Move();
    }
    
    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0) move = -cam.up * x + cam.right * z;
        
        transform.RotateAround(planet.position, move, speed * Time.deltaTime);
        
        Quaternion lookRotation = Quaternion.LookRotation(transform.forward, move);
        planeTransform.rotation =  
            Quaternion.Slerp(planeTransform.rotation, lookRotation, turnSmoothTime * Time.deltaTime);

        float distanceFromPlanet = Vector3.Distance(planet.position, transform.position);
        if(Input.GetKey(KeyCode.Q) && distanceFromPlanet > minElevation) 
            controller.Move(-transform.forward);
        else if (Input.GetKey(KeyCode.E) && distanceFromPlanet < maxElevation)
            controller.Move(transform.forward);
    }
}
