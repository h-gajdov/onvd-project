using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Transform planet;
    
    private void Update()
    {
        Vector3 lookDirection = new Vector3(transform.position.x - planet.position.x,
            transform.position.y - planet.position.y, transform.position.z - planet.position.z);
        Quaternion lookRotation = Quaternion.LookRotation(-lookDirection);
        transform.rotation = lookRotation;
    }
}
