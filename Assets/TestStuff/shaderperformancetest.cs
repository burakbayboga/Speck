using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shaderperformancetest : MonoBehaviour
{

    public GameObject spawn;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(spawn, new Vector3(i * 2f - 10f, 0f, 0f), Quaternion.identity);
            yield return new WaitForSeconds(1.7f);
        }
    }

}
