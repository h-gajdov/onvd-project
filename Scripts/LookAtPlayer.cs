using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = Camera.main.transform;
    }

    void Update()
    {
        GameMath.LookAtTransform(transform, player);
    }
}
