using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EarthGenerator : MonoBehaviour
{
    [SerializeField, HideInInspector] private MeshFilter[] filters;
    private TerrainFace[] faces;

    public Material earthMaterial;
    public TextAsset countryJson;
    public TextAsset cityJson;
    public Texture2D[] heightMaps;
    public float heightMultiplier = 2f;
    public int numberOfFacesPerSide = 4;
    
    [Range(0, 256)]
    [SerializeField] private int resolution = 10;
    [SerializeField] public float radius = 1;
    [SerializeField] private bool generate = false;
    
    public static EarthGenerator instance;

    private static Vector2[] macedonia =
    {
       new Vector2(20.964257812499994f, 40.849902343750017f),
       new Vector2(20.95859375f, 40.871533203124997f),
       new Vector2(20.933496093750023f, 40.903125f),
       new Vector2(20.870214843750006f, 40.917919921874997f),
       new Vector2(20.740820312500006f, 40.9052734375f),
       new Vector2(20.709277343750017f, 40.928369140624994f),
       new Vector2(20.656054687500017f, 41.061669921874994f),
       new Vector2(
          20.614453125000011f,
          41.083056640624996f
       ),
       new Vector2(
          20.56787109375f,
          41.127832031249994f
       ),
       new Vector2(
          20.488964843750011f,
          41.272607421874994f
       ),
       new Vector2(
          20.487011718750011f,
          41.336083984374994f
       ),
       new Vector2(
          20.492382812500011f,
          41.39140625f
       ),
       new Vector2(
          20.448632812500023f,
          41.521289062499996f
       ),
       new Vector2(
          20.4755859375f,
          41.554101562499994f
       ),
       new Vector2(
          20.516210937500006f,
          41.574755859374996f
       ),
       new Vector2(
          20.5166015625f,
          41.627050781249999f
       ),
       new Vector2(
          20.505175781250017f,
          41.706494140624997f
       ),
       new Vector2(
          20.553125f,
          41.862353515624996f
       ),
       new Vector2(
          20.566210937500017f,
          41.873681640624994f
       ),
       new Vector2(
          20.578515625000023f,
          41.8662109375f
       ),
       new Vector2(
          20.694921875f,
          41.853808593749996f
       ),
       new Vector2(
          20.725f,
          41.87353515625f
       ),
       new Vector2(
          20.744140625f,
          41.904296875f
       ),
       new Vector2(
          20.750390625000023f,
          42.018359375f
       ),
       new Vector2(
          20.778125f,
          42.071044921875f
       ),
       new Vector2(
          21.059765625000011f,
          42.171289062499994f
       ),
       new Vector2(
          21.142480468750023f,
          42.175f
       ),
       new Vector2(
          21.2060546875f,
          42.128955078124996f
       ),
       new Vector2(
          21.25634765625f,
          42.099511718749994f
       ),
       new Vector2(
          21.28662109375f,
          42.100390625f
       ),
       new Vector2(
          21.297558593750011f,
          42.130078125f
       ),
       new Vector2(
          21.331738281250011f,
          42.187158203124994f
       ),
       new Vector2(
          21.389550781250023f,
          42.219824218749999f
       ),
       new Vector2(
          21.560839843750017f,
          42.24765625f
       ),
       new Vector2(
          21.5625f,
          42.247509765624997f
       ),
       new Vector2(
          21.618261718750006f,
          42.242138671874983f
       ),
       new Vector2(
          21.7392578125f,
          42.267724609374994f
       ),
       new Vector2(
          21.814648437500011f,
          42.303125f
       ),
       new Vector2(
          21.85302734375f,
          42.308398437499996f
       ),
       new Vector2(
          21.904101562500017f,
          42.322070312499996f
       ),
       new Vector2(
          21.9775390625f,
          42.320068359375f
       ),
       new Vector2(
          22.052050781250017f,
          42.304638671875011f
       ),
       new Vector2(
          22.146679687500011f,
          42.325f
       ),
       new Vector2(
          22.23974609375f,
          42.358154296875f
       ),
       new Vector2(
          22.277050781250011f,
          42.349853515625f
       ),
       new Vector2(
          22.317382812499972f,
          42.321728515625011f
       ),
       new Vector2(
          22.344042968749989f,
          42.313964843750028f
       ),
       new Vector2(
          22.498242187500011f,
          42.165087890624996f
       ),
       new Vector2(
          22.582714843750011f,
          42.104833984374999f
       ),
       new Vector2(
          22.682324218750011f,
          42.059130859374996f
       ),
       new Vector2(
          22.796093750000011f,
          42.025683593749996f
       ),
       new Vector2(
          22.836816406250023f,
          41.993603515624997f
       ),
       new Vector2(
          22.9091796875f,
          41.835205078125f
       ),
       new Vector2(
          22.943945312500006f,
          41.775097656249997f
       ),
       new Vector2(
          22.991992187500017f,
          41.757177734374999f
       ),
       new Vector2(
          23.003613281250011f,
          41.73984375f
       ),
       new Vector2(
          23.005664062500017f,
          41.716943359374994f
       ),
       new Vector2(
          22.951464843750017f,
          41.605615234374994f
       ),
       new Vector2(
          22.9296875f,
          41.356103515624994f
       ),
       new Vector2(
          22.916015625f,
          41.336279296874977f
       ),
       new Vector2(
          22.859277343750023f,
          41.337353515624997f
       ),
       new Vector2(
          22.783886718750011f,
          41.331982421874997f
       ),
       new Vector2(
          22.755078125000011f,
          41.312744140625f
       ),
       new Vector2(
          22.724804687500011f,
          41.178515625f
       ),
       new Vector2(
          22.603613281250006f,
          41.140185546874996f
       ),
       new Vector2(
          22.493554687500023f,
          41.118505859374999f
       ),
       new Vector2(
          22.400781250000023f,
          41.123388671874999f
       ),
       new Vector2(
          22.237695312499994f,
          41.155175781250051f
       ),
       new Vector2(
          22.184472656250023f,
          41.158642578124997f
       ),
       new Vector2(
          22.138867187500011f,
          41.140527343749994f
       ),
       new Vector2(
          21.993359375000011f,
          41.130957031249999f
       ),
       new Vector2(
          21.929492187500017f,
          41.107421875f
       ),
       new Vector2(
          21.779492187500011f,
          40.950439453125f
       ),
       new Vector2(
          21.627539062500006f,
          40.896337890624999f
       ),
       new Vector2(
          21.57578125f,
          40.868945312499996f
       ),
       new Vector2(
          21.459667968750011f,
          40.903613281249996f
       ),
       new Vector2(
          21.404101562499989f,
          40.907177734374955f
       ),
       new Vector2(
          21.32373046875f,
          40.867138671874997f
       ),
       new Vector2(
          21.147558593750006f,
          40.863134765624999f
       ),
       new Vector2(
          21.1f,
          40.856152343749997f
       ),
       new Vector2(
          20.964257812499994f,
          40.849902343750017f
       )
    };
    
    private Vector2 ReadContryData()
    {
        return Vector3.zero;
    }
    
    private void FillFaces()
    {
        if(!generate) return;
        
        if(filters == null || filters.Length == 0) filters = new MeshFilter[6 * numberOfFacesPerSide];
        faces = new TerrainFace[6 * numberOfFacesPerSide];
        Vector3[] directions =
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };
        
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < numberOfFacesPerSide; j++)
            {
                int faceIndex = j + i * numberOfFacesPerSide;
                if (filters[faceIndex] == null)
                {
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;
                    meshObj.AddComponent<MeshRenderer>().sharedMaterial = earthMaterial;
                    filters[faceIndex] = meshObj.AddComponent<MeshFilter>();
                    filters[faceIndex].sharedMesh = new Mesh();   
                }
                
                faces[faceIndex] = new TerrainFace(filters[faceIndex].sharedMesh, resolution, j % (int)Mathf.Sqrt(numberOfFacesPerSide), j / (int)Mathf.Sqrt(numberOfFacesPerSide), radius, directions[i]);
            }
        }
    }
    
    private void FillCountries()
    {
       CountryJSONReader.SetJSONFile(countryJson);
       Country[] countries = CountryJSONReader.ReadAllCountries();
       
       faces = new TerrainFace[countries.Length];
       
       for (int i = 0; i < countries.Length; i++)
       {
          foreach (CountryPolygon polygon in countries[i].polygons)
          {
             polygon.DrawLineRenderer(transform);
          }
       }
    }
    
    private void GenerateMesh()
    {
        if (faces == null || !generate) return;
        foreach (TerrainFace face in faces) face.ConstructMesh();
    }
    
    private void OnValidate()
    {
        instance = this;
        FillFaces();
        GenerateMesh();
    }

    private void Start()
    {
       FillCountries();
    }
}
