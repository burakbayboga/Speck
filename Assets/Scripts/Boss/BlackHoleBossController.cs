using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct ScreenSection
{
    public int x;
    public int y;
}

public class BlackHoleBossController : Boss
{


    public int BlackHoleCount;
    public float DelayBetweenBlackHoles;
    public GameObject BlackHoleBossPrefab;

    private List<ScreenSection> OccupiedSections;
    private Camera MainCamera;


    public override void Initiate()
    {
        base.Initiate();
        OccupiedSections = new List<ScreenSection>();
        MainCamera = Camera.main;
        StartCoroutine(SpawnBlackHoles());
    }

    private IEnumerator SpawnBlackHoles()
    {
        WaitForSeconds secondsToWait = new WaitForSeconds(DelayBetweenBlackHoles);

        for (int i=0; i < BlackHoleCount; i++)
        {
            Instantiate(BlackHoleBossPrefab, GetSpawnPosition(), Quaternion.identity);
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
                x = Random.Range(1, /*Screen.width / */8),
                y = Random.Range(1, /*Screen.height / */3)
            };
        }
        while (IsSectionOccupied(newSection));

        OccupiedSections.Add(newSection);
        Vector2 screenPoint = new Vector2(newSection.x * (Screen.width / 8), newSection.y * (Screen.height / 3));
        Vector3 worldPoint = MainCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y , 25.0f));
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


}
