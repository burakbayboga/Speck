﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFryManager : MonoBehaviour
{
    public static SmallFryManager instance;

    [HideInInspector] public Action<SmallFry> OnSmallFrySpawned;

    List<SmallFry> SmallFryList;
    

    void Awake()
    {
        instance = this;
        SmallFryList = new List<SmallFry>();
    }

    public void KillAllSmallFry()
    {
        while (SmallFryExists())
        {
            SmallFryList[0].HandleDeath();
        }
    }

    public void AddToSmallFryList(SmallFry smallFry)
    {
        SmallFryList.Add(smallFry);
        if (OnSmallFrySpawned != null)
        {
            OnSmallFrySpawned(smallFry);
        }
    }

    public bool SmallFryExists()
    {
        return SmallFryList.Count > 0;
    }

    public void RemoveFromSmallFryList(SmallFry smallFry)
    {
        SmallFryList.Remove(smallFry);
    }

    public void ApplyEffectToAllSmallFry(Action<SmallFry> effect)
    {
        for (int i=0; i < SmallFryList.Count; i++)
        {
            effect(SmallFryList[i]);
        }
    }
}
