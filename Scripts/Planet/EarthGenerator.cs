using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthGenerator : MonoBehaviour
{
    [SerializeField, HideInInspector] private MeshFilter[] filters;
    private TerrainFace[] faces;

    public Texture2D[] heightMaps;
    
    [SerializeField] private int resolution = 10;
    [SerializeField] private float radius = 1;

    public static EarthGenerator instance;
    
    private void Start()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(this);
            return;
        }
        
        Coordinate testCoord = new Coordinate(0, 0);
        Debug.Log(testCoord.GetHeight());
    }
    
    private void FillFaces()
    {
        if (filters == null || filters.Length == 0) filters = new MeshFilter[6];
        faces = new TerrainFace[6];
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
            if (filters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                filters[i] = meshObj.AddComponent<MeshFilter>();
                filters[i].sharedMesh = new Mesh();   
            }

            faces[i] = new TerrainFace(filters[i].sharedMesh, resolution, radius, directions[i]);
        }
    }

    private void GenerateMesh()
    {
        foreach (TerrainFace face in faces) face.ConstructMesh();
    }
    
    private void OnValidate()
    {
        FillFaces();
        GenerateMesh();
    }
}
