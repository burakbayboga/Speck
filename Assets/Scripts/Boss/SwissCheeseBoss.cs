using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwissCheeseBoss : Boss
{

    private float Radius;
    private Vector3 ScreenMiddleScreenSpace;
    private List<Vector3> SpawnPositions;
    private List<GameObject> VisualCues;
    private AudioSource AudioSource;
    
    public int MobCount;
    public float MobSpawnInterval;
    public GameObject MobPrefab;
    public GameObject VisualCuePrefab;

    public override void Initiate()
    {
        base.Initiate();
        AudioSource = GetComponent<AudioSource>();
        int minScreenSize = Mathf.Min(Screen.height, Screen.width);
        Radius = (minScreenSize / 2.0f) * 3.0f / 4.0f;
        ScreenMiddleScreenSpace = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
        InitSpawnPositions();
        StartCoroutine(SpawnVisualCues());
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

    private IEnumerator SpawnVisualCues()
    {
        VisualCues = new List<GameObject>();
        for (int i=0; i < MobCount; i++)
        {
            VisualCues.Add(Instantiate(VisualCuePrefab, SpawnPositions[i], Quaternion.identity));
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator SpawnMobCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        AudioSource.Play();
        for (int i=0; i < MobCount; i++)
        {
            GameObject tempVisualCue = VisualCues[0];
            VisualCues.RemoveAt(0);
            Destroy(tempVisualCue);
            SmallFry newMob = Instantiate(MobPrefab, SpawnPositions[i], Quaternion.identity).GetComponent<SmallFry>();
            newMob.Init();
            //SpawnMob(i);
            yield return new WaitForSeconds(MobSpawnInterval);
        }
    }

    private void SpawnMob(int index)
    {
        float x = ScreenMiddleScreenSpace.x + Mathf.Cos(index * (360.0f / MobCount) * Mathf.Deg2Rad) * Radius;
        float y = ScreenMiddleScreenSpace.y + Mathf.Sin(index * (360.0f / MobCount) * Mathf.Deg2Rad) * Radius;
        Vector3 screenPos = new Vector3(x, y, 25.0f);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        SmallFry newMob = Instantiate(MobPrefab, worldPos, Quaternion.identity).GetComponent<SmallFry>();
        newMob.Init();
    }



}
