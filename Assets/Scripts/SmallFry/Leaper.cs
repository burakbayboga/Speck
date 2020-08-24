using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaper : SmallFry
{

    public GameObject InactiveParticle;
    public GameObject ActiveParticle;

    public float LeapDelay;
    public int LeapCount;
    public float LeapSpeed;

    private LineRenderer LineRenderer;
    private Collider2D Collider;

    private Coroutine WindupCoroutine;

    public override void Init()
    {
        base.Init();
        EnemyType = EnemyType.Leaper;
        LineRenderer = GetComponent<LineRenderer>();
        Collider = GetComponent<Collider2D>();
        DetermineStartTheta();
        //StartCoroutine(Leap());
        StartCoroutine(LeapSequence());
    }

    private Vector3 GetLeapPoint(float leapTheta)
    {
        float xCoord = Mathf.Cos(leapTheta * Mathf.Deg2Rad) * 20.0f;
        float yCoord = Mathf.Sin(leapTheta * Mathf.Deg2Rad) * 10.0f;
        return new Vector3(xCoord, yCoord, 0.0f);
    }

    private IEnumerator Windup(Vector3 target)
    {
        while (true)
        {
            Vector3 movement = (transform.position - target).normalized * 0.05f;
            transform.position += movement;
            LineRenderer.SetPosition(0, transform.position);
            yield return null;
        }
    }

    private IEnumerator LeapSequence()
    {

        Vector3[] leapSequence = new Vector3[LeapCount];

        float thetaTemp = CurrentTheta;

        for (int i = 0; i < LeapCount; i++)
        {
            float leapTheta = (thetaTemp + 180f + Random.Range(-100f, 100f)) % 360f;
            leapSequence[i] = GetLeapPoint(leapTheta);
            thetaTemp = leapTheta;
        }

        StartCoroutine(DrawSequenceLine(leapSequence));
        WindupCoroutine = StartCoroutine(Windup(leapSequence[0]));

        yield return new WaitForSeconds(LeapDelay);

        StopCoroutine(WindupCoroutine);

        Collider.enabled = true;
        LineRenderer.enabled = false;
        InactiveParticle.SetActive(false);
        ActiveParticle.SetActive(true);

        for (int i = 0; i < leapSequence.Length; i++)
        {
            float lerpParameter = 0f;
            float startTime = Time.time;
            float timePassed;
            Vector3 originalPos = i == 0 ? transform.position : leapSequence[i - 1];

            while (Vector3.Distance(transform.position, leapSequence[i]) >= 0.5f)
            {
                Vector3 movement = (leapSequence[i] - originalPos).normalized * LeapSpeed;
                movement = Vector3.ClampMagnitude(movement, (leapSequence[i] - transform.position).magnitude);
                transform.position += movement;
                yield return null;
            }
        }

        HandleDeath();
    }

    private IEnumerator Leap()
    {
        Collider.enabled = false;
        InactiveParticle.SetActive(true);
        ActiveParticle.SetActive(false);


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

        //if (LeapCounttt < LeapCount)
        //{
        //    StartCoroutine(Leap());
        //}
        //else
        //{
        //    HandleDeath();
        //}

    }

    public override void HandleDeath()
    {
        base.HandleDeath();
    }


    private IEnumerator DrawSequenceLine(Vector3[] leapSequence)
    {
        LineRenderer.enabled = true;
        LineRenderer.positionCount = 1;

        LineRenderer.SetPosition(0, transform.position);
        

        for (int i = 1; i < leapSequence.Length + 1; i++)
        {
            Vector3 lineEndOffset = leapSequence[i - 1] - LineRenderer.GetPosition(i - 1);
            float offsetMultiplier = 0f;
            float lerpParameter = 0f;
            float startTime = Time.time;

            LineRenderer.positionCount = i + 1;

            while (lerpParameter < 1f)
            {
                lerpParameter = (Time.time - startTime) * 4f;
                offsetMultiplier = lerpParameter;
                LineRenderer.SetPosition(i, LineRenderer.GetPosition(i - 1) + lineEndOffset * offsetMultiplier);
                yield return null;
            }

        }
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
