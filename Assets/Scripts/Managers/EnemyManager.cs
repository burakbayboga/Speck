using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager instance;

    public float SmallFryMinDelay;
    public float SmallFryMaxDelay;
    public float BossDelay;

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
        //StopCoroutine(SpawnSmallFryCoroutine);
        StartCoroutine(InitiateBossCoroutine());
    }

    public void OnBossDefeated()
    {
        GameController.instance.CurrentScore += 20;
        GameController.instance.BossSmallFryCountdownActive = true;
    }

    IEnumerator InitiateBossCoroutine()
    {
        GameController.instance.TriggerBossEncounterIntro();
        yield return new WaitForSeconds(BossDelay);
        GameController.instance.DeactivateBossEncounterIntro();
        BossGenerator.instance.InitiateBoss();
        yield break;

        //while (true)
        //{
        //    if (SmallFryManager.instance.SmallFryExists())
        //    {
        //        yield return null;
        //    }
        //    else
        //    {
        //        GameController.instance.TriggerBossEncounterIntro();
        //        yield return new WaitForSeconds(BossDelay);
        //        GameController.instance.DeactivateBossEncounterIntro();
        //        BossGenerator.instance.InitiateBoss();
        //        yield break;
        //    }
        //}
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
