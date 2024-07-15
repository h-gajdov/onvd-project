using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMath
{
    
    public const float eatrhRadius = 6371; //kilometers
    public static float maxScore = 100f;
    public static float maxDistance = 1000f;
    
    
    public static Quaternion GlobalQuatToLocal(Transform parent, Quaternion globalQuat)
    {
        Quaternion parentQuaternion = Quaternion.Inverse(parent.rotation);
        Quaternion localQuat = parentQuaternion * globalQuat;
        return localQuat;
    }

    public static Vector2[] ConvertDouble2ToVector2(double[,] doubleArray)
    {
        int length = doubleArray.Length / 2;
        Vector2[] result = new Vector2[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = new Vector2((float)doubleArray[i, 0], (float)doubleArray[i, 1]);
        }
        
        return result;
    }

    public static Vector3[] ConvertVector2ArrayToVector3Array(Vector2[] array)
    {
        Vector3[] result = new Vector3[array.Length];
        for (int i = 0; i < array.Length; i++) result[i] = new Vector3(array[i].x, 0f, array[i].y);
        return result;
    }

    public static float DistanceBetweenPointsOnEarth(Vector3 a, Vector3 b)
    {
        Coordinate aEarth = Coordinate.PointToCoordinate(a);
        Coordinate bEarth = Coordinate.PointToCoordinate(b);

        float lat1 = aEarth.latitude * Mathf.Deg2Rad;
        float lat2 = bEarth.latitude * Mathf.Deg2Rad;
        float lng1 = aEarth.longitude * Mathf.Deg2Rad;
        float lng2 = bEarth.longitude * Mathf.Deg2Rad;
        
        return HaversineFormula(lat1, lat2, lng1, lng2);
    }

    public static void LookAtTransform(Transform looker, Transform looked)
    {
        Vector3 lookDirection = new Vector3(looker.transform.position.x - looked.transform.position.x,
            looker.transform.position.y - looked.transform.position.y, looker.transform.position.z - looked.transform.position.z);
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        looker.transform.rotation = lookRotation;
    }
    
    public static float DistanceBetweenCoordinatesOnEarth(Coordinate a, Coordinate b)
    {
        float lat1 = a.latitude * Mathf.Deg2Rad;
        float lat2 = b.latitude * Mathf.Deg2Rad;
        float lng1 = a.longitude * Mathf.Deg2Rad;
        float lng2 = b.longitude * Mathf.Deg2Rad;

        return HaversineFormula(lat1, lat2, lng1, lng2);
    }

    private static float HaversineFormula(float lat1, float lat2, float lng1, float lng2)
    {
        float deltaLat = lat1 - lat2;
        float deltaLong = lng1 - lng2;
        float arcsinValue = Mathf.Sqrt(Mathf.Sin(deltaLat / 2f) * Mathf.Sin(deltaLat / 2f) +
                                       Mathf.Cos(lat1) * Mathf.Cos(lat2) * Mathf.Sin(deltaLong / 2f) *
                                       Mathf.Sin(deltaLong / 2f));

        return 2f * eatrhRadius * Mathf.Asin(arcsinValue);
    }

    public static float CalculateScoreFromDistance(float distance)
    {
        return Mathf.Clamp(maxScore * (1 - (distance / maxDistance)), 0, maxScore);
    }

    public static Vector3 FixVertexOnSphere(Vector3 point)
    {
        Coordinate coord = Coordinate.PointToCoordinate(point);
        return Coordinate.CoordinateToPoint(coord) * GameManager.planetRadius;
    }
}