using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private bool moving = false;
    private Vector3 idlePosition;
    
    public static bool coroutineActive = false;
    
    private void MoveCameraToLastTarget()
    {
        StopAllCoroutines();
        moving = true;
        Player.SetMovement(false);
        StartCoroutine(GameMath.SlerpTransformToPosition(transform, GameManager.lastTarget, speed));
    }

    private void MoveCameraToPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(GameMath.SlerpTransformToPosition(transform, Player.instance.transform.position, speed));
        StartCoroutine(ResetPlayer());
    }

    private IEnumerator ResetPlayer()
    {
        while (coroutineActive) yield return null;

        moving = false;
        Player.SetMovement(true);
        transform.localPosition = idlePosition;
    }

    private void Start()
    {
        idlePosition = transform.localPosition;
    }
    
    private void Update()
    {
        GameMath.LookAtTransform(transform , GameManager.planet, true);

        if (Input.GetKeyDown(KeyCode.F)) MoveCameraToLastTarget();
        if (moving && Input.GetKeyDown(KeyCode.N)) MoveCameraToPlayer();
    }
}
