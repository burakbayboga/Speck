using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

	public static Background instance;

	public float WaveSpeed;
	public float WaveFrequency;

	Renderer Renderer;
	
	// sent to shader
	Vector4[] WaveCenter = new Vector4[5];
	float[] WaveRadiusStart = new float[5];
	
	float[] WaveStartTime = new float[5];
	List<bool> IndexStates = new List<bool>() { true, true, true, true, true };

	void Awake()
	{
		instance = this;
		Renderer = GetComponent<Renderer>();
	}

	void Start()
	{
		InitShaderProperties();
	}

	void InitShaderProperties()
	{
		for (int i = 0; i < WaveRadiusStart.Length; i++)
		{
			WaveRadiusStart[i] = 3000f;
		}

		Renderer.material.SetFloatArray("_WaveRadiusStart", WaveRadiusStart);
	}

	public void StartBlackHoleWave(Vector3 position, float lifeTime, BlackHole blackHole)
	{
		StartCoroutine(BlackHoleWave(position, lifeTime, blackHole));
	}

	IEnumerator BlackHoleWave(Vector4 position, float lifeTime, BlackHole blackHole)
	{
		int index = GetAvailableIndex();
		
		WaveCenter[index] = position;
		Renderer.material.SetVectorArray("_WaveCenter", WaveCenter);

		float timePassed = 0f;
		float startTime = Time.time;
		float waveTime = 0f;

		while (timePassed <= lifeTime)
		{
			timePassed += Time.deltaTime;
			waveTime = (waveTime + Time.deltaTime) % WaveFrequency;
			float radiusStart = waveTime * WaveSpeed;
			WaveRadiusStart[index] = radiusStart;
			blackHole.WaveRadiusStart = radiusStart;
			Renderer.material.SetFloatArray("_WaveRadiusStart", WaveRadiusStart);

			yield return null;
		}

		WaveRadiusStart[index] = 3000f;
		Renderer.material.SetFloatArray("_WaveRadiusStart", WaveRadiusStart);
		ReleaseIndex(index);
	}

	void ReleaseIndex(int index)
	{
		IndexStates[index] = true;
	}

	int GetAvailableIndex()
	{
		for (int i = 0; i < IndexStates.Count; i++)
		{
			if (IndexStates[i])
			{
				IndexStates[i] = false;
				return i;
			}
		}

		IndexStates[0] = false;
		return 0;
	}
}
