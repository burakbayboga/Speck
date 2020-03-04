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

    
    [Range(0f, 5f)] public float aLowLimit;
    [Range(0f, 5f)] public float aHighLimit;
    [Range(0f, 5f)] public float bLowLimit;
    [Range(0f, 5f)] public float bHighLimit;
    [Range(0f, 5f)] public float kLowLimit;
    [Range(-5f, 5f)] public float kHighLimit;

    public float Speed;


    public Vector3 kTest;
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
            //Points[i].y = a * Mathf.Sin((Points[i].x - h) / b) + k;
            //Points[i].y = (Points[i].x / 15f) * Mathf.Sin((Points[i].x - h) / b) + k;

            Points[i].y = aMapped(Points[i].x) * Mathf.Sin((Points[i].x / bMapped(Points[i].x)) - Time.time) + kMapped(Points[i].x);
        }

        LineRenderer.SetPositions(Points);
    }

    float aMapped(float x)
    {
        return Utility.MapToInterval(0f, 50f, aLowLimit, aHighLimit, x);
    }

    float bMapped(float x)
    {
        return Utility.MapToInterval(0f, 50f, bLowLimit, bHighLimit, x);
    }

    float kMapped(float x)
    {
        return Utility.MapToInterval(0f, 50f, kLowLimit, kHighLimit, x);
    }
}
