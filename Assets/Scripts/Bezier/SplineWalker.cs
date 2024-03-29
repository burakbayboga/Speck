﻿using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    public BezierSpline spline;
	public float duration;
	public bool lookForward;
	public SplineWalkerMode mode;

	bool goingForward = true;
	float progress;

	void Update()
	{
		if (goingForward)
		{
			progress += Time.deltaTime / duration;
			if (progress > 1f)
			{
				if (mode == SplineWalkerMode.Once)
				{
					progress = 1f;
				}
				else if (mode == SplineWalkerMode.Loop)
				{
					progress -= 1f;
				}
				else
				{
					progress = 2f - progress;
					goingForward = false;
				}
			}
		}
		else
		{
			progress -= Time.deltaTime / duration;
			if (progress < 0f)
			{
				progress = -progress;
				goingForward = true;
			}
		}
		
		Vector3 position = spline.GetPoint(progress);
		transform.position = position;
		if (lookForward)
		{
			if (goingForward)
			{
				transform.LookAt(position + spline.GetDirection(progress));
			}
			else
			{
				transform.LookAt(position - spline.GetDirection(progress));
			}
		}
	}
}

public enum SplineWalkerMode
{
	Once,
	Loop,
	PingPong
}
