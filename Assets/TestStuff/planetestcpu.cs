using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetestcpu : MonoBehaviour
{

    public float Strength;
    public float Speed;

    MeshFilter MeshFilter;
    List<Vector3> Vertices;

    void Awake()
    {
        MeshFilter = GetComponent<MeshFilter>();    
    }

    void Start()
    {
        Vertices = new List<Vector3>();
        MeshFilter.mesh.GetVertices(Vertices);    
    }

    void Update()
    {
        ModifyMesh();
        MeshFilter.mesh.SetVertices(Vertices);
    }

    void ModifyMesh()
    {
        for (int i = 0; i < Vertices.Count; i++)
        {
            Vector3 vertex = Vertices[i];
            float distance = Mathf.Sqrt(vertex.x * vertex.x + vertex.z * vertex.z);
            vertex.y = Mathf.Sin(distance * (Mathf.Sin(Time.time * Speed) + 1f)) * Strength;
            Vertices[i] = vertex;
        }
    }


}
