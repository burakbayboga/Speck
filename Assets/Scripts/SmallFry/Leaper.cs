using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaper : SmallFry
{

    public GameObject InactiveParticle;
    public GameObject ActiveParticle;

    public float LeapDelay;
    public int LeapThreshold;

    private int LeapCount;
    private LineRenderer LineRenderer;
    private Collider2D Collider;

    public override void Init()
    {
        base.Init();
        EnemyType = EnemyType.Leaper;
        LeapCount = 0;
        LineRenderer = GetComponent<LineRenderer>();
        Collider = GetComponent<Collider2D>();
        DetermineStartTheta();
        StartCoroutine(Leap());
    }

    private Vector3 GetLeapPoint(float leapTheta)
    {
        float xCoord = Mathf.Cos(leapTheta * Mathf.Deg2Rad) * 20.0f;
        float yCoord = Mathf.Sin(leapTheta * Mathf.Deg2Rad) * 10.0f;
        return new Vector3(xCoord, yCoord, 0.0f);
    }

    private IEnumerator Leap()
    {
        Collider.enabled = false;
        InactiveParticle.SetActive(true);
        ActiveParticle.SetActive(false);

        LeapCount++;

        float leapTheta = (CurrentTheta + 180.0f + Random.Range(-50.0f, 50.0f)) % 360.0f;
        Vector3 originalPos = transform.position;
        Vector3 leapTarget = GetLeapPoint(leapTheta);

        StartCoroutine(DrawLine(leapTarget));

        yield return new WaitForSeconds(LeapDelay);

        Collider.enabled = true;
        InactiveParticle.SetActive(false);
        ActiveParticle.SetActive(true);

        LineRenderer.enabled = false;

        float lerpParameter = 0.0f;
        float startTime = Time.time;
        float timePassed;

        AudioSource.Play();

        while (lerpParameter < 1.0f)
        {
            timePassed = Time.time - startTime;
            lerpParameter = Mathf.Clamp(timePassed * 1.0f, 0.0f, 1.0f);
            transform.position = Vector3.Lerp(originalPos, leapTarget, lerpParameter);
            yield return null;
        }

        AudioSource.Stop();

        CurrentTheta = leapTheta;

        if (LeapCount < LeapThreshold)
        {
            StartCoroutine(Leap());
        }
        else
        {
            HandleDeath();
        }

    }

    public override void HandleDeath()
    {
        base.HandleDeath();
    }

    private IEnumerator DrawLine(Vector3 leapTarget)
    {
        LineRenderer.enabled = true;
        LineRenderer.SetPosition(0, transform.position);

        Vector3 lineEndOffset = leapTarget - transform.position;
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

    }
}
