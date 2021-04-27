using System.Collections;
using UnityEngine;

public class Leaper : SmallFry
{
    public GameObject InactiveParticle;
    public GameObject ActiveParticle;

    public float LeapDelay;
    public int LeapCount;
    public float LeapSpeed;

	public float TraceInterval;

    private Collider2D Collider;

	private int[] TracePoolIndexes;
	private GameObject[] TraceObjects;


    public override void Init()
    {
        base.Init();
        Collider = GetComponent<Collider2D>();
        DetermineStartTheta();
        StartCoroutine(LeapSequence());
    }

    private Vector3 GetLeapPoint(float leapTheta)
    {
        float xCoord = Mathf.Cos(leapTheta * Mathf.Deg2Rad) * 20.0f;
        float yCoord = Mathf.Sin(leapTheta * Mathf.Deg2Rad) * 10.0f;
        return new Vector3(xCoord, yCoord, 0.0f);
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

		StartCoroutine(DeactivateTrace());
 
        Collider.enabled = true; 
        InactiveParticle.SetActive(false); 
        ActiveParticle.SetActive(true);
		int deactivationTraceIndex = 0;
 
		float movementSinceTrace = 0f;
        for (int i = 0; i < leapSequence.Length; i++) 
        { 
            Vector3 originalPos = i == 0 ? transform.position : leapSequence[i - 1]; 

            while (Vector3.Distance(transform.position, leapSequence[i]) >= 0.5f) 
            { 
                Vector3 movement = (leapSequence[i] - originalPos).normalized * LeapSpeed;
				float movementMagCache = movement.magnitude;
                movement = Vector3.ClampMagnitude(movement, (leapSequence[i] - transform.position).magnitude); 
                transform.position += movement; 
				movementSinceTrace += movement.magnitude;
				if (movementSinceTrace >= TraceInterval)
				{
					deactivationTraceIndex++;
					movementSinceTrace -= TraceInterval;
				}
                yield return null; 
            } 
        } 
 
        HandleDeath(); 
    }

    public override void HandleDeath()
    {
		TraceDeactivationFailsafe();
		TracePool.instance.ReturnTrace(TracePoolIndexes);
        base.HandleDeath();
    }

	private void TraceDeactivationFailsafe()
	{
		for (int i = TraceObjects.Length - 1; i >= 0; i--)
		{
			if (TraceObjects[i].activeSelf)
			{
				TraceObjects[i].SetActive(false);
			}
			else
			{
				return;
			}
		}
	}

	private IEnumerator DeactivateTrace()
	{
		int indexToDeactivate = 0;
		float tracePerFrame = LeapSpeed / TraceInterval;
		float traceCount = 0f;
		bool done = false;
		while (!done)
		{
			yield return null;
			traceCount += tracePerFrame;
			if (traceCount >= TraceObjects.Length)
			{
				traceCount = TraceObjects.Length - 1;
				done = true;
			}
			for (; indexToDeactivate < (int)traceCount; indexToDeactivate++)
			{
				TraceObjects[indexToDeactivate].SetActive(false);
			}
		}
	}

    private IEnumerator DrawSequenceLines(Vector3[] leapSequence) 
    { 
		WaitForSeconds delay = new WaitForSeconds(0.02f);

		float totalDistance = 0f;
		Vector3 leapSrc = transform.position;
		for (int i = 0; i < leapSequence.Length; i++)
		{
			totalDistance += Vector3.Distance(leapSrc, leapSequence[i]);
			leapSrc = leapSequence[i];
		}

		int traceCount = (int)(totalDistance / TraceInterval);
		TraceObjects = TracePool.instance.GetTrace(traceCount, out TracePoolIndexes);
		if (TraceObjects == null)
		{
			// TODO: not enough trace pooled
			Debug.LogError("NOT ENOUGH TRACE");
		}

		Vector3 tracePosition;
		Vector3 traceVector;
		float distanceToLeapPoint = Vector3.Distance(transform.position, leapSequence[0]);
		int currentLeapTargetIndex = 0;
		Vector3 previousTracePosition = transform.position;

		for (int i = 0; i < TraceObjects.Length; i++)
		{
			GameObject trace = TraceObjects[i];
			
			if (currentLeapTargetIndex == 0)
			{
				traceVector = (leapSequence[0] - transform.position).normalized;
			}
			else
			{
				traceVector = (leapSequence[currentLeapTargetIndex] - leapSequence[currentLeapTargetIndex - 1]).normalized;
			}
			if (TraceInterval <= distanceToLeapPoint)
			{
				tracePosition = previousTracePosition + traceVector * TraceInterval;
				distanceToLeapPoint -= TraceInterval;
			}
			else
			{
				float distanceOnNextLine = TraceInterval - distanceToLeapPoint;
				currentLeapTargetIndex++;
				traceVector = (leapSequence[currentLeapTargetIndex] - leapSequence[currentLeapTargetIndex - 1]).normalized;
				tracePosition = leapSequence[currentLeapTargetIndex - 1] + traceVector * distanceOnNextLine;
				distanceToLeapPoint = Vector3.Distance(leapSequence[currentLeapTargetIndex], tracePosition);
			}
			
			previousTracePosition = tracePosition;

			trace.SetActive(true);
			trace.transform.position = tracePosition;

			yield return delay;
		}
    }
}
