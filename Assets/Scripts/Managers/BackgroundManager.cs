using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    Material Material;
    MeshRenderer MeshRenderer;
    Camera MainCamera;

    // optimize this. send all data in a single vector4
    Vector4[] EffectCenters = new Vector4[4];
    float[] ActiveEffectCenters = new float[4];
    float[] EffectStartTimes = new float[4];

    void Awake()
    {
        instance = this;

        MeshRenderer = GetComponent<MeshRenderer>();
        Material = MeshRenderer.material;
        MainCamera = Camera.main;
    }

    public void OnBlackHoleActivated(Vector3 effectCenter, float effectLife)
    {
        int effectIndex = GetAvailableEffectIndex();
        EffectCenters[effectIndex] = MainCamera.WorldToViewportPoint(effectCenter);
        ActiveEffectCenters[effectIndex] = 1f;
        EffectStartTimes[effectIndex] = Time.time;

        Material.SetVectorArray("_EffectCenters", EffectCenters);
        Material.SetFloatArray("_ActiveEffectCenters", ActiveEffectCenters);
        Material.SetFloatArray("_EffectStartTimes", EffectStartTimes);

        StartCoroutine(EffectLife(effectLife, effectIndex));
    }

    IEnumerator EffectLife(float lifeTime, int effectIndex)
    {
        yield return new WaitForSeconds(lifeTime);
        ActiveEffectCenters[effectIndex] = 0f;
        Material.SetFloatArray("_ActiveEffectCenters", ActiveEffectCenters);
    }

    int GetAvailableEffectIndex()
    {
        int index = 0;

        for (int i = 0; i < ActiveEffectCenters.Length; i++)
        {
            if (ActiveEffectCenters[i] == 0f)
            {
                index = i;
                break;
            }
        }

        return index;
    }

}
