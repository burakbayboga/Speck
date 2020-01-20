using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEverythingC : Collectible
{

    public override void OnCollected()
    {
        SmallFryManager.instance.KillAllSmallFry();
        CollectibleSpawner.instance.CollectibleSmallFryCountdownActive = true;
        Destroy(gameObject);
    }



}
