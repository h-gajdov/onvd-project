using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainMenuStar : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float maxSize = 1f;
    [SerializeField] private float minSize = 0.2f;

    private float offset = 0f;

    private void Start()
    {
        offset = Random.Range(0f, Mathf.PI);
    }

    private void Update()
    {
        transform.localScale = Scale(Time.time);
    }

    private Vector3 Scale(float x)
    {
        float value = Mathf.Abs((maxSize - minSize) * Mathf.Sin(speed * x + offset)) + minSize;
        return Vector3.one * value;
    }
}
