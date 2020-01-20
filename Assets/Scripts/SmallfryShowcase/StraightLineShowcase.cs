using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLineShowcase : MonoBehaviour
{

    public GameObject EdgeParticleInitial;
    public GameObject EdgeParticleConstant;

    Rigidbody2D Rigidbody;
    Vector3 TargetPosition;
    Vector3 OriginalPosition;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        OriginalPosition = transform.position;
        TargetPosition = OriginalPosition + Vector3.up * 15f;
    }

    private void Start()
    {
        StartCoroutine(Showcase());
    }

    IEnumerator Showcase()
    {
        yield return new WaitForSeconds(1f);

        float lerpParameter = 0.0f;
        float startTime = Time.time;
        while (lerpParameter < 1.0f)
        {
            lerpParameter = (Time.time - startTime) / 2.0f;
            Rigidbody.angularVelocity = Mathf.Lerp(0.0f, 1440.0f, lerpParameter * lerpParameter);
            yield return null;
        }

        EdgeParticleInitial.SetActive(false);
        EdgeParticleConstant.SetActive(true);

        startTime = Time.time;
        lerpParameter = 0f;

        while (lerpParameter < 1f)
        {
            lerpParameter = Time.time - startTime;
            transform.position = Vector3.Lerp(OriginalPosition, TargetPosition, lerpParameter);
            yield return null;
        }

        ResetShowcase();
    }

    void ResetShowcase()
    {
        EdgeParticleConstant.SetActive(false);
        EdgeParticleInitial.SetActive(true);
        Rigidbody.angularVelocity = 0f;
        transform.position = OriginalPosition;
        transform.rotation = Quaternion.identity;
        StartCoroutine(Showcase());
    }
}
