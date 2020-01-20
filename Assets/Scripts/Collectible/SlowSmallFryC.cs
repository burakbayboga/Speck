using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowSmallFryC : Collectible
{

    public void Effect(SmallFry smallFry)
    {
        smallFry.SpeedMultiplier = 0.1f;
    }

    public void Revert(SmallFry smallFry)
    {
        smallFry.SpeedMultiplier = 1.0f;
    }

    public override void OnCollected()
    {
        GetComponent<MeshRenderer>().enabled = false;
        SmallFryManager.instance.ApplyEffectToAllSmallFry(Effect);
        SmallFryManager.instance.OnSmallFrySpawned += Effect;
        StartCoroutine(EffectTimer(EffectTime));
    }

    private IEnumerator EffectTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        SmallFryManager.instance.ApplyEffectToAllSmallFry(Revert);
        SmallFryManager.instance.OnSmallFrySpawned -= Effect;
        CollectibleSpawner.instance.CollectibleSmallFryCountdownActive = true;
        Destroy(gameObject);
    }

}
