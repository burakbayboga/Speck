using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fragcirclepulse : MonoBehaviour
{

    MeshRenderer Renderer;

    private void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Renderer.material.SetFloat("_StartTime", Time.time);
        }
    }


}
