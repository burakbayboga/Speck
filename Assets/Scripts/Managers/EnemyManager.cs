using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager instance;

    public float SmallFryMinDelay;
    public float SmallFryMaxDelay;

    private void Awake()
    {
        instance = this;
    }

	public void TriggerSmallFryLoop()
	{
		StartCoroutine(SpawnSmallFry());
	}

    public void TriggerBoss()
    {
		BossGenerator.instance.InitiateBoss();
    }

    IEnumerator SpawnSmallFry()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(SmallFryMinDelay, SmallFryMaxDelay));
            SmallFryGenerator.instance.SpawnSmallFry();
        }
    }
}
