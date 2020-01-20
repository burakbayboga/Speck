using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaperShowcase : MonoBehaviour
{

    public GameObject InactiveParticle;
    public GameObject ActiveParticle;

    LineRenderer LineRenderer;

    Vector3 OriginalPosition;
    Vector3 TargetPosition;

    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
        OriginalPosition = transform.position;
        TargetPosition = OriginalPosition + Vector3.up * 15f;
    }

    private void Start()
    {
        StartCoroutine(Showcase());
    }

    IEnumerator Showcase()
    {

        LineRenderer.SetPosition(0, transform.position);
        Vector3 lineEndOffset = TargetPosition - transform.position;
        float offsetMultiplier = 0.0f;

        float lerpParameter = 0.0f;
        float startTime = Time.time;

        while (lerpParameter < 1.0f)
        {
            lerpParameter = (Time.time - startTime) * 2.0f;
            offsetMultiplier = lerpParameter;
            LineRenderer.SetPosition(1, transform.position + lineEndOffset * offsetMultiplier);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        LineRenderer.enabled = false;
        InactiveParticle.SetActive(false);
        ActiveParticle.SetActive(true);

        startTime = Time.time;
        lerpParameter = 0f;

        while (lerpParameter < 1f)
        {
            lerpParameter = (Time.time - startTime);
            transform.position = Vector3.Lerp(OriginalPosition, TargetPosition, lerpParameter);
            yield return null;
        }

        ResetShowcase();
    }

    void ResetShowcase()
    {
        LineRenderer.enabled = true;
        ActiveParticle.SetActive(false);
        InactiveParticle.SetActive(true);
        transform.position = OriginalPosition;
        StartCoroutine(Showcase());
    }

}
