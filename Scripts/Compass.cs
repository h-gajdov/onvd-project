using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    private Transform navigationArrow;

    private void Start()
    {
        navigationArrow = transform.GetChild(0);
    }

    private void Update()
    {
        Coordinate targetCoord = GameManager.selectedCity.coordinate;
        Coordinate playerCoord = Coordinate.PointToCoordinate(Player.instance.transform.position);
        Vector2 target = new Vector2(targetCoord.longitude, targetCoord.latitude);
        Vector2 player = new Vector2(playerCoord.longitude, playerCoord.latitude);
        Vector2 direction = (player - target).normalized;
        Quaternion rot = Quaternion.LookRotation(direction);
        float value = (direction.x < 0) ? rot.eulerAngles.x : 180 - rot.eulerAngles.x;
        navigationArrow.rotation = Quaternion.Euler(0f, 0f, value);
    }
}
