using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectible : MonoBehaviour
{

    public float EffectTime;

    [HideInInspector] protected LilB LilB;

    protected bool Collected;

    void Awake()
    {
        LilB = FindObjectOfType<LilB>();
        Collected = false;
    }

    public virtual void OnCollected() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LilB") && !Collected)
        {
            Collected = true;
            OnCollected();
        }
    }
}
