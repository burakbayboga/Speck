using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracePool : MonoBehaviour
{
    public static TracePool instance;
	
	public Sprite[] TraceSprites;

	public GameObject[] TraceObjects;
	bool[] TraceInUse;
	int AvailableIndexCount;

	void Awake()
	{
		instance = this;
		InitTraceObjects();
	}

	void InitTraceObjects()
	{
		for (int i = 0; i < TraceObjects.Length; i++)
		{
			TraceObjects[i].GetComponent<SpriteRenderer>().sprite = TraceSprites[Random.Range(0, TraceSprites.Length)];
		}

		TraceInUse = new bool[TraceObjects.Length];
		AvailableIndexCount = TraceInUse.Length;
	}

	public GameObject[] GetTrace(int count, out int[] availableIndexes)
	{
		if (count > AvailableIndexCount)
		{
			// TODO: allocate new trace if necessary
			availableIndexes = null;
			return null;
		}

		GameObject[] availableTrace = new GameObject[count];
		availableIndexes = new int[count];
		int j = 0;
		for (int i = 0; i < TraceInUse.Length; i++)
		{
			if (!TraceInUse[i])
			{
				availableIndexes[j] = i;
				TraceInUse[i] = true;
				availableTrace[j] = TraceObjects[i];
				j++;
				if (j == count)
				{
					break;
				}
			}
		}

		return availableTrace;
	}

	public void ReturnTrace(int[] indexesToReturn)
	{
		for (int i = 0; i < indexesToReturn.Length; i++)
		{
			TraceInUse[indexesToReturn[i]] = false;
		}
	}
}
