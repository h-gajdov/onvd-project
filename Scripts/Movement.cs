using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform planet;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float turnSmoothTime = 1f;
    
    private CharacterController controller;
    private Transform planeTransform;
    
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        planeTransform = transform.GetChild(0);
    }
    
    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.up * x + transform.right * z;

        transform.RotateAround(planet.position, move, speed * Time.deltaTime);

        if (x == 0 && z == 0) return;
        
        Quaternion lookRotation = Quaternion.LookRotation(transform.forward, move);
        planeTransform.rotation =
            Quaternion.Slerp(planeTransform.rotation, lookRotation, turnSmoothTime * Time.deltaTime);
    }
}
