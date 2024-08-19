using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using Random = UnityEngine.Random;

public class CityJSONReader
{
    public static JSONNode root;

    public static void SetJSONFile(TextAsset jsonFile)
    {
        root = JSONNode.Parse(jsonFile.text)["features"];
    }

    public static City[] ReadAllCapitals()
    {
        List<City> capitals = new List<City>();
        for (int i = 0; i < root.Count; i++)
        {
            if(!isCapital(i)) continue;
            
            capitals.Add(ReadCity(i));
        }

        return capitals.ToArray();
    }

    public static City[] ReadEasyCities()
    {
        List<City> cities = new List<City>();
        List<string> names = new List<string>
            { "Paris", "Berlin", "Moscow", "Washington, D.C.", "Ottawa", "Madrid", "London", "Beijing", "Tokyo", "Canberra", "Skopje", "Rome", "Amsterdam", "Istanbul", "Athens", "Warsaw", "Rio de Janeiro", "Seoul", "Mexico City"};
        for (int i = 0; i < root.Count; i++)
        {
            City city = ReadCity(i);
            if (!names.Contains(city.name) || isPopulatedPlace(i)) continue;
            
            cities.Add(city);
        }

        return cities.ToArray();
    }

    public static City[] ReadHardCities()
    {
        List<City> cities = new List<City>();
        for (int i = 0; i < root.Count; i++)
        {
            if (isPopulatedPlace(i)) continue;
            cities.Add(ReadCity(i));
        }
        
        return cities.ToArray();
    }

    public static City[] ReadTutorialCities()
    {
        List<City> cities = new List<City>();
        List<string> names = new List<string>
            { "Skopje", "Paris", "Berlin", "Moscow", "Athens", "Warsaw", "Seoul"};

        for (int i = 0; i < names.Count; i++)
        {
            for (int j = 0; j < root.Count; j++)
            {
                City city = ReadCity(j);
                if (names[i] != city.name || isPopulatedPlace(j)) continue;
            
                cities.Add(city);
            }
        }

        return cities.ToArray();
    }
    
    public static City ReadCity(int index)
    {
        var unserializedCity = root[index]["properties"];
        Vector2 point = new Vector2(unserializedCity["latitude"], unserializedCity["longitude"]);
        string name = unserializedCity["name"];
        string countryName = unserializedCity["sov0name"];
        return new City(name, point, countryName);
    }

    public static City ReadCityByName(string name)
    {
        for (int i = 0; i < root.Count; i++)
        {
            City city = ReadCity(i);
            if (name != city.name) continue;

            return city;
        }

        return null;
    }
    
    public static bool isCapital(int index)
    {
        return root[index]["properties"]["featurecla"].Value.Contains("Admin-0 capital");
    }

    public static bool isPopulatedPlace(int index)
    {
        return root[index]["properties"]["featurecla"].Value == "Populated place";
    }

    //Knuth shuffle algorithm
    public static void ShuffleCities(City[] cities)
    {
        for (int i = 0; i < cities.Length; i++)
        {
            City tmp = cities[i];
            int rng = Random.Range(i, cities.Length);
            cities[i] = cities[rng];
            cities[rng] = tmp;
        }
    }
}

[Serializable]
public class City
{
    public string name;
    public string countryName;
    public Coordinate coordinate;

    public Vector3 worldPosition
    {
        get
        {
            return Coordinate.CoordinateToPoint(coordinate) * (GameManager.planetRadius + coordinate.GetHeight() * 20);
        }
    }
    
    public City(string name, Vector2 coordinate, string countryName)
    {
        this.name = name;
        this.coordinate = new Coordinate(coordinate.x, coordinate.y);
        this.countryName = countryName;
    }

    public void Print()
    {
        string result = "Name: " + name + "\n Coordinate: " + coordinate.latitude + " " + coordinate.longitude;
        Debug.Log(result);
    }
}