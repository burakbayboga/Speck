using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureMultipleWaves : MonoBehaviour
{

    Material Material;
    MeshRenderer MeshRenderer;
    Camera MainCamera;

    Vector4[] EffectCenters = new Vector4[4];
    float[] ActiveEffectCenters = new float[4] { 0f, 0f, 0f, 0f };

    private void Awake()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        Material = MeshRenderer.material;
        MainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //print(Input.mousePosition + " " + MainCamera.ScreenToViewportPoint(Input.mousePosition) + " " + MainCamera.ScreenToWorldPoint(Input.mousePosition));
            //Material.SetVector("_TempCenter", MainCamera.ScreenToViewportPoint(Input.mousePosition));
            SetNewEffectCenter(MainCamera.ScreenToViewportPoint(Input.mousePosition));
        }
    }

    private void SetNewEffectCenter(Vector2 viewportPoint)
    {
        EffectCenters[GetAvailableEffectCenterIndex()] = viewportPoint;
        Material.SetFloatArray("_ActiveEffectCenters", ActiveEffectCenters);
        Material.SetVectorArray("_EffectCenters", EffectCenters);
    }

    int GetAvailableEffectCenterIndex()
    {
        int index = 0;
        
        for (int i = 0; i < ActiveEffectCenters.Length; i++)
        {
            if (ActiveEffectCenters[i] == 0f)
            {
                index = i;
                ActiveEffectCenters[i] = 1f;
                break;
            }
        }

        return index;
    }


}
