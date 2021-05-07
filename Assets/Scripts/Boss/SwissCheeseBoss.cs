using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwissCheeseBoss : Boss
{

    private float Radius;
    private Vector3 ScreenMiddleScreenSpace;
    private List<Vector3> SpawnPositions;
    private AudioSource AudioSource;
    
    public int MobCount;
    public float MobSpawnInterval;
	public float MobSpawnDelay;
    public GameObject MobPrefab;

    public override void Initiate()
    {
        base.Initiate();
        AudioSource = GetComponent<AudioSource>();
        int minScreenSize = Mathf.Min(Screen.height, Screen.width);
        Radius = (minScreenSize / 2.0f) * 3.0f / 4.0f;
        ScreenMiddleScreenSpace = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
        InitSpawnPositions();
        StartCoroutine(SpawnMobCoroutine());
    }

    private void InitSpawnPositions()
    {
        SpawnPositions = new List<Vector3>();
        for (int i=0; i < MobCount; i++)
        {
            float screenX = ScreenMiddleScreenSpace.x + Mathf.Cos(i * (360.0f / MobCount) * Mathf.Deg2Rad) * Radius;
            float screenY = ScreenMiddleScreenSpace.y + Mathf.Sin(i * (360.0f / MobCount) * Mathf.Deg2Rad) * Radius;
            Vector3 screenPos = new Vector3(screenX, screenY, 25.0f);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            SpawnPositions.Add(worldPos);
        }
    }

    private IEnumerator SpawnMobCoroutine()
    {
        yield return new WaitForSeconds(MobSpawnDelay);
        AudioSource.Play();
        for (int i=0; i < MobCount; i++)
        {
            SmallFry newMob = Instantiate(MobPrefab, SpawnPositions[i], Quaternion.identity).GetComponent<SmallFry>();
            newMob.Init();
            yield return new WaitForSeconds(MobSpawnInterval);
        }
    }
}
