using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
    [SerializeField] private float planeSpeed = 1f;
    [SerializeField] private float earthSpeed = 1f;

    [SerializeField] private Transform planePivot;
    [SerializeField] private Transform planeEndPoint;
    [SerializeField] private Transform earth;
    
    //trail forever lifetime 3.402823e+38
    private void Update()
    {
        earth.Rotate(0f, earthSpeed * Time.deltaTime, 0f);
        
        // if (Vector3.Distance(planePivot.GetChild(0).position, planeEndPoint.position) > 0.1f)
        planePivot.Rotate(0f, planeSpeed * Time.deltaTime, 0f);
    }
}
