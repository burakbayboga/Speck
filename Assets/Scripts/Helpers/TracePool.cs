using UnityEngine;

public class TracePool : MonoBehaviour
{
    public static TracePool instance;
	
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
		TraceInUse = new bool[TraceObjects.Length];
		AvailableIndexCount = TraceInUse.Length;
	}

	// TODO: use database indexing like system to optimize?
	// cache last given index to search for available indexes,
	// instead of starting from beginning every time
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
