using UnityEngine;

public class SmallFryGenerator : MonoBehaviour
{

    public static SmallFryGenerator instance;

    public GameObject[] SmallFryPrefabs;
    public float[] SmallFrySpawnChances;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnSmallFry()
    {
        int smallFryToSpawnIndex = GetRandomSmallFryIndex();
        Vector3 spawnPosition = SpawnPositionHelper.GetSpawnPositionForEnemy((EnemyType)smallFryToSpawnIndex);

        SmallFry newSmallFry = Instantiate(SmallFryPrefabs[smallFryToSpawnIndex], spawnPosition, Quaternion.identity).GetComponent<SmallFry>();
        
        newSmallFry.Init();
        SmallFryManager.instance.AddToSmallFryList(newSmallFry);
        GameController.instance.CheckBoss();
    }

    int GetRandomSmallFryIndex()
    {

		bool canSpawnBlackHole = SmallFryManager.instance.CurrentBlackHoleCount < 2;
		int spawnIndex = 0;
		do
		{
			float random = Random.Range(0.0f, 1.0f);
			float sum = 0.0f;

			for (int i=0; i < SmallFrySpawnChances.Length; i++)
			{
				sum += SmallFrySpawnChances[i];
				if (random <= sum)
				{
					spawnIndex = i;
					break;
				}
			}
		}
		while (!canSpawnBlackHole && spawnIndex == 3);

		if (spawnIndex == 3)
		{
			SmallFryManager.instance.CurrentBlackHoleCount++;
		}

        return spawnIndex;
    }

    
}
