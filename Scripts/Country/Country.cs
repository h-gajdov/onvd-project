using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;

public class CountryJSONReader
{
    private static JSONNode root;
    private static JSONNode desc;
    
    public static void SetJSONFile(TextAsset jsonFile)
    {
        root = JSON.Parse(jsonFile.text)["features"];
    }

    public static void SetDescriptionJSONFile(TextAsset jsonFile)
    {
        desc = JSON.Parse(jsonFile.text);
        Debug.Log(desc.Count);
    }
    
    public static Country[] ReadAllCountries()
    {
        List<Country> countries = new List<Country>();

        // double[,] test = ReadGeometry(141);
        // Vector2[] geom = GameMath.ConvertDouble2ToVector2(test);
        // countries.Add(new Country(141, geom));

        for (int i = 0, cx = 0; i < root.Count; i++)
        {
            CountryPolygon[] polygons = ReadGeometry(i);
            CountryInfo info = ReadInfo(i);
            
            countries.Add(new Country(info, polygons));
        }

        return countries.ToArray();
    }

    public static Country ReadCountry(int index)
    {
        CountryPolygon[] polygons = ReadGeometry(index);
        CountryInfo info = ReadInfo(index);

        return new Country(info, polygons);
    }

    public static Country ReadCountry(string name)
    {
        for (int i = 0; i < root.Count; i++)
        {
            CountryInfo info = ReadInfo(i);
            if (info.name != name) continue;

            return ReadCountry(i);
        }

        return null;
    }
    
    public static CountryInfo ReadInfo(int index)
    {
        var unserializedArray = root[index]["properties"];
        string name = unserializedArray["ADMIN"];
        return new CountryInfo(index, name);
    }
    
    public static CountryPolygon[] ReadGeometry(int index)
    {
        CountryPolygon[] result;
        double[,] coordsMatrix;
        var unserializedArray = root[index]["geometry"]["coordinates"];
        
        if (root[index]["geometry"]["type"] == "Polygon")
        {
            result = new CountryPolygon[1];
            unserializedArray = unserializedArray[0];
            coordsMatrix = new double[unserializedArray.Count,2];
            for (int i = 0; i < unserializedArray.Count; i++)
            {
                coordsMatrix[i, 0] = unserializedArray[i][0];
                coordsMatrix[i, 1] = unserializedArray[i][1];
            }

            Vector2[] points = GameMath.ConvertDouble2ToVector2(coordsMatrix);
            result[0] = new CountryPolygon(points);
        }
        else
        {
            result = new CountryPolygon[unserializedArray.Count];

            for (int i = 0; i < unserializedArray.Count; i++)
            {
                int numberOfCoords = unserializedArray[i][0].Count;
                coordsMatrix = new double[numberOfCoords, 2];
                for (int j = 0; j < numberOfCoords; j++)
                {
                    coordsMatrix[j, 0] = unserializedArray[i][0][j][0];
                    coordsMatrix[j, 1] = unserializedArray[i][0][j][1];
                }
                
                Vector2[] points = GameMath.ConvertDouble2ToVector2(coordsMatrix);
                result[i] = new CountryPolygon(points);
            }
        }

        return result;
    }

    public static string ReadDescription(int index)
    {
        return desc[index]["description"];
    }
}

[Serializable]
public class Country
{
    public CountryInfo info;
    // public Vector2[] geometry;
    public CountryPolygon[] polygons;
    
    public Country(CountryInfo info, CountryPolygon[] polygons)
    {
        this.info = info;
        
        this.polygons = new CountryPolygon[polygons.Length];
        for (int i = 0; i < polygons.Length; i++)
            this.polygons[i] = polygons[i];
        
    }
}

[Serializable]
public class CountryInfo
{
    public int index;
    public string name;
    public string description;
    public Sprite flag;
    public City capitalCity;
    
    public CountryInfo(int index, string name)
    {
        this.index = index;
        this.name = name;
        flag = Resources.Load<Sprite>("CountryFlags/" + this.name);
        if (flag == null) flag = Resources.Load<Sprite>("CountryFlags/No Flag");
        capitalCity = CityJSONReader.ReadCapital(index);
        description = CountryJSONReader.ReadDescription(index);
    }

    public void Print()
    {
        Debug.Log("Name: " + name);
    }
}

public class CountryPolygon
{
    public Vector2[] geometry;

    public CountryPolygon(Vector2[] geometry)
    {
        this.geometry = new Vector2[geometry.Length];
        for (int i = 0; i < geometry.Length; i++)
            this.geometry[i] = geometry[i];
    }

    public void DrawLineRenderer(Transform parent = null)
    {
        GameObject meshObj = new GameObject("mesh");
        meshObj.transform.parent = parent;
        LineRenderer lineRenderer = meshObj.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.25f;
        lineRenderer.endWidth = 0.25f;
        lineRenderer.positionCount = geometry.Length;
                
        int j = 0;
        foreach (Vector2 point in geometry)
        {
            Coordinate coord = new Coordinate(point.y, point.x);
            Vector3 pointOnSphere = Coordinate.CoordinateToPoint(coord);
            pointOnSphere *=
                Coordinate.PointToCoordinate(pointOnSphere).GetHeight() * EarthGenerator.instance.heightMultiplier + (EarthGenerator.instance.radius + 0.5f);
            lineRenderer.SetPosition(j++, pointOnSphere);
        }
    }
}