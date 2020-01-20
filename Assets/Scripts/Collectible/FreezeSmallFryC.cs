using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeSmallFryC : Collectible
{

    public GameObject FreezeOnTouchPrefab;

    public override void OnCollected()
    {
        Instantiate(FreezeOnTouchPrefab, LilB.transform);
        Destroy(gameObject);
    }
}
