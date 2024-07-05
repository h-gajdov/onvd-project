using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    private Mesh mesh;
    private int resolution;
    private int xOffset;
    private int yOffset;
    private float distance;
    private Vector3 localUp;
    private Vector3 axisA;
    private Vector3 axisB;
    
    public TerrainFace(Mesh mesh, int resolution, int xOffset, int yOffset, float distance, Vector3 localUp)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        this.distance = distance;
        this.xOffset = xOffset;
        this.yOffset = yOffset;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructContries(Vector2[] countryCoords)
    {
        Vector3[] verts = new Vector3[countryCoords.Length];
        int[] tris = new int[81 * 3];
        int trisIndex = 0;
        
        for (int i = 0; i < countryCoords.Length; i++)
        {
            Coordinate coordinate = new Coordinate(countryCoords[i].y, countryCoords[i].x);
            Vector3 point = Coordinate.CoordinateToPoint(coordinate);
            Vector3 pointOnSphere = PointOnCubeToPointOnSphere(point) * distance;
            verts[i] = pointOnSphere;

            if (i > 60) continue;
            
            tris[trisIndex] = i;
            tris[trisIndex + 1] = i + 81 + 1;
            tris[trisIndex + 2] = i + 81;
            tris[trisIndex + 3] = i;
            tris[trisIndex + 4] = i + 1;
            tris[trisIndex + 5] = i + 81 + 1;
            trisIndex += 6;
        }
        
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
    
    public void ConstructMesh()
    {
        Vector3[] verts = new Vector3[resolution * resolution];
        int[] tris = new int[(resolution - 1) * (resolution - 1) * 6];
        int trisIndex = 0;
        
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnCube = localUp + (percent.x - xOffset + 1.5f) / 2.5f * axisA + (percent.y - yOffset + 1.5f) / 2.5f * axisB;
                Vector3 pointOnSphere = PointOnCubeToPointOnSphere(pointOnCube);
                pointOnSphere *= distance + (Coordinate.PointToCoordinate(pointOnSphere).GetHeight() * EarthGenerator.instance.heightMultiplier);
                verts[i] = pointOnSphere;

                if (x != resolution - 1 && y != resolution - 1)
                {
                    tris[trisIndex] = i;
                    tris[trisIndex + 1] = i + resolution + 1;
                    tris[trisIndex + 2] = i + resolution;
                    tris[trisIndex + 3] = i;
                    tris[trisIndex + 4] = i + 1;
                    tris[trisIndex + 5] = i + resolution + 1;
                    trisIndex += 6;
                }
            }
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }

    public static Vector3 PointOnCubeToPointOnSphere(Vector3 p)
    {
        float x2 = p.x * p.x;
        float y2 = p.y * p.y;
        float z2 = p.z * p.z;
        float x = p.x * Mathf.Sqrt(1 - (y2 + z2) / 2 + (y2 * z2) / 3);
        float y = p.y * Mathf.Sqrt(1 - (z2 + x2) / 2 + (z2 * x2) / 3);
        float z = p.z * Mathf.Sqrt(1 - (x2 + y2) / 2 + (x2 * y2) / 3);
        return new Vector3(x, y, z);
    }
}
