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

    public static void LookAtTransform(Transform looker, Transform looked, bool inverted = false)
    {
        Vector3 lookDirection = looker.transform.position - looked.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation((inverted) ? -lookDirection : lookDirection);
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

    public static Vector3 FixVertexOnSphere(Vector3 point, float distance = 0f)
    {
        Coordinate coord = Coordinate.PointToCoordinate(point);
        return Coordinate.CoordinateToPoint(coord) * (GameManager.planetRadius + distance);
    }

    public static Vector3 GetRandomPointOnEarth(float radius = 1)
    {
        float longitude = Random.Range(-180f, 180f);
        float latitude = Random.Range(-90f, 90f);
        Coordinate coord = new Coordinate(latitude, longitude);
        return Coordinate.CoordinateToPoint(coord) * radius;
    }

    public static Vector3 GetRandomScreenToWorldPosition(float distance = 1f)
    {
        float randomY = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distance)).y, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, distance)).y);
        float randomX = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distance)).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, distance)).x);
        
        return new Vector3(randomX, randomY, distance);
    }

    private static float Speed(float x, float speed)
    {
        float epsilon = 0.1f;
        return (speed - epsilon) * Mathf.Sin(Mathf.PI * x) + epsilon;
    }
    
    public static IEnumerator SlerpTransformToPosition(Transform a, Vector3 b, float t)
    {
        CameraControler.coroutineActive = true;
        
        float previousSpeed = -1f;
        float startDistance = DistanceBetweenPointsOnEarth(a.position, b);
        for (float distance = DistanceBetweenPointsOnEarth(a.position, b);
             distance > 0.001f; distance = DistanceBetweenPointsOnEarth(a.position, b))
        {
            float x = (startDistance - distance) / startDistance;
            float speed = Speed(x, t);
            a.position = Vector3.RotateTowards(a.position, b,  speed * Time.deltaTime, 0f);
            if (previousSpeed == speed) break;
            previousSpeed = speed;
            yield return null;
        }

        CameraControler.coroutineActive = false;
    }
}