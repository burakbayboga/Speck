using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveVisualizer : MonoBehaviour
{

    public int PointCount;
    public float PointDensity;
    public float a;
    public float b;
    public float h;
    public float k;

    LineRenderer LineRenderer;
    Vector3[] Points;

    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
        Points = new Vector3[PointCount];
    }

    private void Start()
    {
        for (int i = 0; i < PointCount; i++)
        {
            Points[i] = new Vector3(i / PointDensity, 0f, 0f);
        }

        LineRenderer.positionCount = PointCount;
    }

    private void Update()
    {
        for (int i = 0; i < PointCount; i++)
        {
            Points[i].y = a * Mathf.Sin((Points[i].x - h) / b) + k;
        }

        LineRenderer.SetPositions(Points);
    }

}
