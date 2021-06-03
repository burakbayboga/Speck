using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwissCheeseBoss : Boss
{
    private float Radius;
    private Vector3 ScreenMiddleScreenSpace;
    private List<Vector3> SpawnPositions;
    
    public int MobCount;
    public float MobSpawnInterval;
    public GameObject MobPrefab;

	private int ActiveMobCount = 0;

    public override void Initiate()
    {
        base.Initiate();
        int minScreenSize = Mathf.Min(Screen.height, Screen.width);
        Radius = (minScreenSize / 2.0f) * 3.0f / 4.0f;
        ScreenMiddleScreenSpace = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
        InitSpawnPositions();
        StartCoroutine(SpawnMobCoroutine());
    }

    private void InitSpawnPositions()
    {
        SpawnPositions = new List<Vector3>();
		float angleInterval = 360f / MobCount;
        for (int i=0; i < MobCount; i++)
        {
            float screenX = ScreenMiddleScreenSpace.x + Mathf.Cos(i * angleInterval * Mathf.Deg2Rad) * Radius;
            float screenY = ScreenMiddleScreenSpace.y + Mathf.Sin(i * angleInterval * Mathf.Deg2Rad) * Radius;
            Vector3 screenPos = new Vector3(screenX, screenY, 25.0f);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            SpawnPositions.Add(worldPos);
        }
    }

    private IEnumerator SpawnMobCoroutine()
    {
        for (int i=0; i < MobCount; i++)
        {
            StraightLine newMob = Instantiate(MobPrefab, SpawnPositions[i], Quaternion.identity).GetComponent<StraightLine>();
            newMob.Init();
			newMob.AssignBoss(this);
			ActiveMobCount++;
            yield return new WaitForSeconds(MobSpawnInterval);
        }
    }

	public void OnMobDestroyed()
	{
		ActiveMobCount--;
		if (ActiveMobCount == 0)
		{
			if (LilB.instance.IsChallenge)
			{
				ChallengeController.instance.OnBossDefeated();
			}
			else
			{
				EnemyManager.instance.OnBossDefeated();
			}
			Destroy(gameObject);
		}
	}
}
