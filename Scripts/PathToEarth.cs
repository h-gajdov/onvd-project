using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class PathToEarth : MonoBehaviour
{
    private PathCreator pathCreator;
    public bool generate;

    private void OnValidate()
    {
        pathCreator = GetComponent<PathCreator>();
        
        for (int i = 0; i < pathCreator.bezierPath.NumPoints; i++)
        {
            Vector3 point = pathCreator.bezierPath.GetPoint(i);
            Coordinate coord = Coordinate.PointToCoordinate(point);
            pathCreator.bezierPath.SetPoint(i, Coordinate.CoordinateToPoint(coord) * 600f);
        }
    }
}
