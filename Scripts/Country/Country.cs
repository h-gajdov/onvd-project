using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;

public class CountryJSONReader
{
    private static JSONNode root;
    
    public static void SetJSONFile(TextAsset jsonFile)
    {
        root = JSON.Parse(jsonFile.text)["features"];
    }
    
    public static Country[] ReadAllCountries()
    {
        List<Country> countries = new List<Country>();

        // double[,] test = ReadGeometry(141);
        // Vector2[] geom = GameMath.ConvertDouble2ToVector2(test);
        // countries.Add(new Country(141, geom));

        for (int i = 0, cx = 0; i < root.Count; i++)
        {
            double[,] countryGeometry = ReadGeometry(i);
            CountryInfo info = ReadInfo(i);
            Vector2[] points = GameMath.ConvertDouble2ToVector2(countryGeometry);
            
            countries.Add(new Country(cx++, info, points));
        }

        return countries.ToArray();
    }

    public static CountryInfo ReadInfo(int index)
    {
        var unserializedArray = root[index]["properties"];
        string name = unserializedArray["ADMIN"];
        return new CountryInfo(name);
    }
    
    public static double[,] ReadGeometry(int index)
    {
        double[,] result = null;
        var unserializedArray = root[index]["geometry"]["coordinates"];
        
        if (root[index]["geometry"]["type"] == "Polygon")
        {
            unserializedArray = unserializedArray[0];
            result = new double[unserializedArray.Count,2];
            for (int i = 0; i < unserializedArray.Count; i++)
            {
                result[i, 0] = unserializedArray[i][0];
                result[i, 1] = unserializedArray[i][1];
            }
        }
        else
        {
            int count = 0;
            for (int i = 0; i < unserializedArray.Count; i++)
            {
                count += unserializedArray[i][0].Count;
            }

            result = new double[count, 2];
            for (int i = 0, ix = 0; i < unserializedArray.Count; i++)
            {
                for (int j = 0; j < unserializedArray[i][0].Count; j++)
                {
                    result[ix, 0] = unserializedArray[i][0][j][0];
                    result[ix++, 1] = unserializedArray[i][0][j][1];
                }
            }
        }

        return result;
    }
}

[Serializable]
public class Country
{
    public int index;
    public CountryInfo info;
    public Vector2[] geometry;
    public Bounds2D bounds;
    
    public Country(int index, CountryInfo info, Vector2[] geometry)
    {
        this.index = index;
        this.info = info;
        
        this.geometry = new Vector2[geometry.Length];
        Vector2[] worldPos = new Vector2[geometry.Length];
        for (int i = 0; i < geometry.Length; i++)
        {
            Coordinate coordinate = new Coordinate(geometry[i].y, geometry[i].x);
            this.geometry[i] = geometry[i];
            Vector3 point = Coordinate.CoordinateToPoint(coordinate) * 10;
            worldPos[i] = new Vector2(point.x, point.z);
        }

        this.bounds = new Bounds2D(worldPos);
    }
}

[Serializable]
public class CountryInfo
{
    public string name;
    
    public CountryInfo(string name)
    {
        this.name = name;
    }

    public void Print()
    {
        Debug.Log("Name: " + name);
    }
}