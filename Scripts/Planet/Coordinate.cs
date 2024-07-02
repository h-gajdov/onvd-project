using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate
{
    private float latitude;
    private float longitude;
    
    public Coordinate(float latitude, float longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public static Coordinate PointToCoordinate(Vector3 pointOnUnitSphere)
    {
        float latitude = Mathf.Asin(pointOnUnitSphere.y);
        float longitude = Mathf.Atan2(pointOnUnitSphere.x, -pointOnUnitSphere.z);
        return new Coordinate(latitude, longitude);
    }

    public static Vector3 CoordinateToPoint(Coordinate coord)
    {
        float y = Mathf.Sin(coord.latitude);
        float r = Mathf.Cos(coord.latitude);
        float x = Mathf.Sin(coord.longitude) * r;
        float z = -Mathf.Cos(coord.longitude) * r;
        return new Vector3(x, y, z);
    }

    public void Print()
    {
        Debug.Log("Lat: " + latitude.ToString() + "Long: " + longitude.ToString());
    }

    public float GetHeight()
    {
        int column = Mathf.FloorToInt((longitude + 180) / 72);
        int row = (latitude < 0) ? 1 : 0;
        int index = row * 4 + column;
        Texture2D map = EarthGenerator.instance.heightMaps[index];
        int x = (int)(longitude + 180) * 2047 / 90;
        int y = (int)Mathf.Abs(-2047 * (latitude - 90) / 90);
        Debug.Log(index.ToString() + " " + x.ToString() + " " + y.ToString());
        return map.GetPixel(x, y).r;
    }
}
