using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float width = 1f;
    [SerializeField] private int detail = 100;
    
    private Mesh mesh;
    private MeshFilter meshFilter;
    private GameObject crosshairImage;
    private bool endedCoroutine = true;

    private void Start()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();

        crosshairImage = transform.GetChild(0).gameObject;
        crosshairImage.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            crosshairImage.SetActive(true);
            if (!endedCoroutine) return;
            radius = 0f;
            StartCoroutine(ShowAimCircle());
        }
        else
        {
            crosshairImage.SetActive(false);
        }
    }
    
    private void CreateCircle()
    {
        List<Vector3> verticiesList = new List<Vector3> { };
        float x;
        float y;
        for (int i = 0; i < 2 * detail; i++)
        {
            x = radius * Mathf.Sin((2 * Mathf.PI * i) / detail);
            y = radius * Mathf.Cos((2 * Mathf.PI * i) / detail);
            verticiesList.Add(new Vector3(x, y, 0f));
            
            x = (radius - width) * Mathf.Sin((2 * Mathf.PI * i) / detail);
            y = (radius - width) * Mathf.Cos((2 * Mathf.PI * i) / detail);
            verticiesList.Add(new Vector3(x, y, 0f));
        }
        Vector3[] vertices = verticiesList.ToArray();
        
        List<int> trianglesList = new List<int> { };
        for(int i = 0; i < 2 * detail; i+=2)
        {
            trianglesList.Add(i);
            trianglesList.Add(i+1);
            trianglesList.Add(i+2);
            
            trianglesList.Add(i+1);
            trianglesList.Add(i+3);
            trianglesList.Add(i+2);
        }
        int[] triangles = trianglesList.ToArray();
        
        List<Vector3> normalsList = new List<Vector3> { };
        for (int i = 0; i < vertices.Length; i++)
        {
            normalsList.Add(transform.forward);
        }
        Vector3[] normals = normalsList.ToArray();
        
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / (radius*2) + 0.5f, vertices[i].y / (radius*2) + 0.5f);
        }
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        meshFilter.mesh = mesh;
    }

    private IEnumerator ShowAimCircle()
    {
        endedCoroutine = false;
        
        while (radius < 80f)
        {
            radius = Mathf.Lerp(radius, 100f, speed * Time.deltaTime);
            CreateCircle();
            yield return null;
        }

        endedCoroutine = true;
    }
}
