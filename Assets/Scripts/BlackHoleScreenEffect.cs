using System.Collections;
using UnityEngine;

public class BlackHoleScreenEffect : MonoBehaviour
{
    public Material BlackHoleScreenEffectMat;

	public float WaveFrequency;
	public float WaveSpeed;
	public float WaveThickness;

	int Height;
	int Width;
	Material ScreenEffect;

	// x,y = world position    z = waveRadiusStart   w = waveRadiusEnd
	Vector4[] BlackHoleData = new Vector4[2]{ Vector4.zero, Vector4.zero };

	bool[] IndexAvailable = new bool[2]{ true, true };

	int ActiveBlackHoleCount;

	void Awake()
	{
		Height = Screen.height;
		Width = Screen.width;
		ScreenEffect = Instantiate(BlackHoleScreenEffectMat);
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, ScreenEffect);
	}

	public void StartBlackHoleEffect(Vector3 position, float lifeTime, BlackHole blackHole)
	{
		if (ActiveBlackHoleCount == 0)
		{
			this.enabled = true;
		}

		ActiveBlackHoleCount++;
		StartCoroutine(BlackHoleWave(position, lifeTime, blackHole));
	}

	IEnumerator BlackHoleWave(Vector2 position, float lifeTime, BlackHole blackHole)
	{
		int index = GetAvailableIndex();

		BlackHoleData[index].x = position.x;
		BlackHoleData[index].y = position.y;

		float timePassed = 0f;
		float startTime = Time.time;
		float waveTime = 0f;

		while (timePassed <= lifeTime)
		{
			timePassed += Time.deltaTime;
			waveTime = (waveTime + Time.deltaTime) % WaveFrequency;
			float radiusStart = waveTime * WaveSpeed;
			float radiusEnd = radiusStart + WaveThickness;
			BlackHoleData[index].z = radiusStart;
			BlackHoleData[index].w = radiusEnd;
			blackHole.WaveRadiusData = new Vector2(radiusStart, radiusEnd);
			ScreenEffect.SetVectorArray("_BlackHoleData", BlackHoleData);

			yield return null;
		}

		BlackHoleData[index].z = 3000f;
		ScreenEffect.SetVectorArray("_BlackHoleData", BlackHoleData);
		ReleaseIndex(index);

		ActiveBlackHoleCount--;
		CheckEffectDisable();
	}

	void CheckEffectDisable()
	{
		if (ActiveBlackHoleCount == 0)
		{
			this.enabled = false;
		}
	}

	void ReleaseIndex(int index)
	{
		IndexAvailable[index] = true;
	}

	int GetAvailableIndex()
	{
		for (int i = 0; i < IndexAvailable.Length; i++)
		{
			if (IndexAvailable[i] == true)
			{
				IndexAvailable[i] = false;
				return i;
			}
		}

		Debug.LogError("NOT ENOUGH BLACK HOLE SLOTS");
		return -1;
	}



}
