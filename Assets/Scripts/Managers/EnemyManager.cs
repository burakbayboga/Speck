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

    private void Start()
    {
        StartCoroutine(SpawnSmallFry());
    }

    public void TriggerBoss()
    {
		BossGenerator.instance.InitiateBoss();
    }

    public void OnBossDefeated()
    {
        GameController.instance.CurrentScore += 20;
        GameController.instance.BossSmallFryCountdownActive = true;
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
