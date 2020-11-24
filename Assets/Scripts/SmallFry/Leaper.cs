using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaper : SmallFry
{
    public GameObject InactiveParticle;
    public GameObject ActiveParticle;

    public LineRenderer LinePrefab;

    public float LeapDelay;
    public int LeapCount;
    public float LeapSpeed;

    private Collider2D Collider;
    private List<LineRenderer> Lines;

    private Coroutine WindupCoroutine;


    public override void Init()
    {
        base.Init();
        EnemyType = EnemyType.Leaper;
        Collider = GetComponent<Collider2D>();
        DetermineStartTheta();
        Lines = new List<LineRenderer>();
        StartCoroutine(LeapSequence());
    }

    private Vector3 GetLeapPoint(float leapTheta)
    {
        float xCoord = Mathf.Cos(leapTheta * Mathf.Deg2Rad) * 20.0f;
        float yCoord = Mathf.Sin(leapTheta * Mathf.Deg2Rad) * 10.0f;
        return new Vector3(xCoord, yCoord, 0.0f);
    }

    private IEnumerator Windup(LineRenderer line, Vector3 forwardDirection) 
    { 
        while (true) 
        { 
            Vector3 movement = (transform.position - forwardDirection).normalized * 0.05f; 
            transform.position += movement; 
            line.SetPosition(0, transform.position); 
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
 
        StartCoroutine(DrawSequenceLines(leapSequence)); 
 
        yield return new WaitForSeconds(LeapDelay); 
 
        StopCoroutine(WindupCoroutine); 
 
        Collider.enabled = true; 
        InactiveParticle.SetActive(false); 
        ActiveParticle.SetActive(true); 
 
        for (int i = 0; i < leapSequence.Length; i++) 
        { 
            Vector3 originalPos = i == 0 ? transform.position : leapSequence[i - 1]; 
			LineRenderer line = Lines[0];

            while (Vector3.Distance(transform.position, leapSequence[i]) >= 0.5f) 
            { 
                Vector3 movement = (leapSequence[i] - originalPos).normalized * LeapSpeed; 
                movement = Vector3.ClampMagnitude(movement, (leapSequence[i] - transform.position).magnitude); 
                transform.position += movement; 
				line.SetPosition(0, transform.position);
                yield return null; 
            } 

			Destroy(line.gameObject);
			Lines.RemoveAt(0);
        } 
 
        HandleDeath(); 
    }

    public override void HandleDeath()
    {
        base.HandleDeath();
    }

    private IEnumerator DrawSequenceLines(Vector3[] leapSequence) 
    { 
 
        for (int i = 0; i < leapSequence.Length; i++) 
        { 
            float lerpParameter = 0f; 
            float startTime = Time.time; 
            Vector3 lineEndPoint = leapSequence[i]; 
            LineRenderer line; 
 
            if (i == 0) 
            { 
                line = Instantiate(LinePrefab, transform.position, Quaternion.identity); 
                WindupCoroutine = StartCoroutine(Windup(line, leapSequence[0])); 
            } 
            else 
            { 
                line = Instantiate(LinePrefab, leapSequence[i - 1], Quaternion.identity); 
            } 
 
            Lines.Add(line); 
 
            line.SetPosition(0, line.transform.position); 
 
            while (lerpParameter < 1f) 
            { 
                lerpParameter = Mathf.Clamp01((Time.time - startTime) * 4f); 
                line.SetPosition(1, line.transform.position + (lineEndPoint - line.transform.position) * lerpParameter); 
                yield return null; 
            } 
        } 
    }
}
