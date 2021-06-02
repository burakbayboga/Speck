using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoss : Boss
{

    public GameObject LaserGunPrefab;
    public float DelayBetweenLasers;

    public int LaserCount;

    private List<ScreenSection> OccupiedSections;
    private Camera MainCamera;

    private bool TriggeredFireSound = false;

    public override void Initiate()
    {
        base.Initiate();
        MainCamera = Camera.main;
        OccupiedSections = new List<ScreenSection>();
        StartCoroutine(SpawnLasers());
        StartCoroutine(DeathCountdown());
    }

    public void TriggerFireSound()
    {
        if (TriggeredFireSound)
        {
            return;        
        }

        TriggeredFireSound = true;
    }

    private IEnumerator DeathCountdown()
    {
        yield return new WaitForSeconds(EncounterTime);
        Destroy(gameObject);
    }

    private IEnumerator SpawnLasers()
    {
        WaitForSeconds secondsToWait = new WaitForSeconds(DelayBetweenLasers);

        for (int i=0; i < LaserCount; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition();
            Quaternion spawnRotation = GetSpawnRotation(spawnPosition);

            LaserGun gun = Instantiate(LaserGunPrefab, spawnPosition, spawnRotation).GetComponent<LaserGun>();
            gun.Init(this);
            yield return secondsToWait;
        }
    }

    private Vector3 GetSpawnPosition()
    {
        ScreenSection newSection;

        do
        {
            newSection = new ScreenSection
            {
                x = Random.Range(1, 7),
                y = Random.Range(1, 4)
            };
        }
        while (IsSectionOccupied(newSection));

        OccupiedSections.Add(newSection);
        Vector2 screenPoint = new Vector2(newSection.x * (Screen.width / 7), newSection.y * (Screen.height / 4));
        Vector3 worldPoint = MainCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, 25.0f));
        return worldPoint;
    }

    private bool IsSectionOccupied(ScreenSection section)
    {
        for (int i=0; i < OccupiedSections.Count; i++)
        {
            if (section.x == OccupiedSections[i].x && section.y == OccupiedSections[i].y)
            {
                return true;
            }
        }

        return false;
    }

    private Quaternion GetSpawnRotation(Vector3 spawnPosition)
    {
        float zRotation = Vector3.SignedAngle(spawnPosition, Vector3.up, Vector3.forward * -1.0f);
        float offset = Random.Range(-60.0f, 60.0f);
        zRotation += offset;
        Quaternion spawnRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, zRotation));
        return spawnRotation;
    }
}
