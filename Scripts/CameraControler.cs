using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Transform planet;
    
    private void Update()
    {
        Vector3 lookDirection = GameManager.GetPlanetDirection(transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(-lookDirection);
        transform.rotation = lookRotation;
    }
}
