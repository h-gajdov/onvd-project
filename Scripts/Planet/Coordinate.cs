using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate
{
    public float latitude;
    public float longitude;
    
    public Coordinate(float latitude, float longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public static Coordinate PointToCoordinate(Vector3 pointOnUnitSphere)
    {
        pointOnUnitSphere.Normalize();
        float latitude = Mathf.Asin(pointOnUnitSphere.y) * Mathf.Rad2Deg;
        float longitude = Mathf.Atan2(pointOnUnitSphere.z, pointOnUnitSphere.x) * Mathf.Rad2Deg;
        return new Coordinate(latitude, longitude);
    }

    public static Vector3 CoordinateToPoint(Coordinate coord)
    {
        float latitudeRad = coord.latitude * Mathf.Deg2Rad;
        float longitudeRad = coord.longitude * Mathf.Deg2Rad;
        
        float y = Mathf.Sin(latitudeRad);
        float r = Mathf.Cos(latitudeRad);
        float x = Mathf.Sin(longitudeRad) * r;
        float z = -Mathf.Cos(longitudeRad) * r;
        return new Vector3(x, y, z);
    }

    public void Print()
    {
        Debug.Log("Lat: " + latitude.ToString() + " Long: " + longitude.ToString());
    }

    public float GetHeight()
    {
        int column = Mathf.FloorToInt((longitude + 180) / 90);
        if (column > 3) column = 3;
        int row = (latitude < 0) ? 1 : 0;
        int index = row * 4 + column;
        float valX = longitude - column * 90;
        float valY = latitude + row * 90;
        Texture2D map = EarthGenerator.instance.heightMaps[index];
        float x = (valX + 180) * 8191 / 90f;
        float y = 8191f - Mathf.Abs(-8191f * (valY - 90f) / 90f);
        return map.GetPixelBilinear(x / 8191f, y / 8191f).grayscale;
    }
}
