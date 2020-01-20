using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawntest : MonoBehaviour
{

    public GameObject TestObject;
    private List<GameObject> Objects = new List<GameObject>();
    //zdrbnskym
    private float _a;
    private float _b;
    public float a
    {
        get
        {
            return _a;
        }
        set
        {
            _a = value;
            for (int i = 0; i < Objects.Count; i++)
            {
                Destroy(Objects[i]);
            }
            Objects.Clear();
            TestSpawns();
        }
    }

    public float b
    {
        get
        {
            return _b;
        }
        set
        {
            _b = value;
            for (int i = 0; i < Objects.Count; i++)
            {
                Destroy(Objects[i]);
            }
            Objects.Clear();
            TestSpawns();
        }
    }

    private void Start()
    {
        a = 10.0f;
        b = 5.0f;
        TestSpawns();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            a += 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            a -= 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            b += 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            b -= 1.0f;
        }
    }

    private void TestSpawns()
    {
        for (float theta = 0.0f; theta < 2.0f * Mathf.PI; theta += GetThetaIncrement(theta))
        {
            Objects.Add(Instantiate(TestObject, GetSpawnPositionForElectricFence(theta), Quaternion.identity));
        }
    }

    private float GetThetaIncrement(float currentTheta)
    {
        float lerpParameter = Mathf.InverseLerp(0f, 1f, Mathf.Abs(Mathf.Cos(currentTheta)));
        return Mathf.Lerp(0.1f, 0.2f, lerpParameter);
    }

    private Vector3 GetSpawnPositionForElectricFence(float theta)
    {
        float xCoord = Mathf.Cos(theta) * a;
        float yCoord = Mathf.Sin(theta) * b;
        return new Vector3(xCoord, yCoord, 0.0f);
    }

}
