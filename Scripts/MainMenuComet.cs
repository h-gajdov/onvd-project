using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainMenuComet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float blinkSpeed = 4f;

    private CharacterController controller;

    private void SetPosition()
    {
        Vector3 position = Vector3.zero;
        bool vertical = Random.value > .5f;
        bool left = Random.value > .5f;
        if (vertical)
        {
            position.x = (left) ? -1 : Screen.width + 1;
            position.y = Random.Range(0, Screen.height);
        }
        else
        {
            position.x = Random.Range(0, Screen.width);
            position.y = (left) ? -1 : Screen.height + 1;
        }

        position.z = 45f;
        position = Camera.main.ScreenToWorldPoint(position);
        transform.position = position;
    }
    
    private void Start()
    {
        SetPosition();
        
        Vector3 target = GameMath.GetRandomScreenToWorldPosition(45f);
        Vector3 direction = transform.position - target;
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = lookRotation;

        controller = gameObject.AddComponent<CharacterController>();
    }

    private void Update()
    {
        transform.localScale = Scale(Time.time);
        
        controller.Move(speed * Time.deltaTime * -transform.forward);
    }
    
    private Vector3 Scale(float x)
    {
        float value = Mathf.Abs(0.3f * Mathf.Sin(blinkSpeed * x ));
        return Vector3.one * value;
    }
}
