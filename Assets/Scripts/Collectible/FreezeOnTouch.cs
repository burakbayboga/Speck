using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeOnTouch : MonoBehaviour
{
    public float PowerUpDuration;
    public float EffectTime;

    LilB LilB;

    void Awake()
    {
        LilB = GameObject.FindWithTag("LilB").GetComponent<LilB>();
        LilB.GetComponent<Collider2D>().enabled = false;
        StartCoroutine(LifeTime(PowerUpDuration));
    }

    IEnumerator LifeTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        GracefulDeath();
    }

    void GracefulDeath()
    {
        CollectibleSpawner.instance.CollectibleSmallFryCountdownActive = true;
        LilB.GetComponent<Collider2D>().enabled = true;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SmallFry"))
        {
            SmallFry smallFry = other.gameObject.GetComponent<SmallFry>();
            FreezeSmallFry(smallFry);
        }
    }

    void FreezeSmallFry(SmallFry smallFry)
    {
        smallFry.ApplyFreeze(EffectTime);
    }
}
