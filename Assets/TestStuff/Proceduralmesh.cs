using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proceduralmesh : MonoBehaviour
{

    public int RectSizeX;
    public int RectSizeY;

    Vector3[] Vertices;
    Mesh mesh;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateRectangularMesh();
        }
    }

    void GenerateRectangularMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();

        Vertices = new Vector3[(RectSizeX + 1) * (RectSizeY + 1)];

        for (int i = 0, y = 0; y <= RectSizeY; y++)
        {
            for (int x = 0; x <= RectSizeX; x++, i++)
            {
                Vertices[i] = new Vector3(x, y);
            }
        }

        int[] triangles = new int[RectSizeX * RectSizeY * 6];

        for (int ti = 0, vi = 0, y = 0; y < RectSizeY; y++, vi++)
        {
            for (int x = 0; x < RectSizeX; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + RectSizeX + 1;
                triangles[ti + 5] = vi + RectSizeX + 2;
            }
        }

        mesh.vertices = Vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }


}
