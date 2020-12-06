using System.Collections.Generic;
using UnityEngine;

public class SmallFryManager : MonoBehaviour
{
    public static SmallFryManager instance;

    List<SmallFry> SmallFryList;
    public int CurrentBlackHoleCount = 0;

    void Awake()
    {
        instance = this;
        SmallFryList = new List<SmallFry>();
    }

    public void AddToSmallFryList(SmallFry smallFry)
    {
        SmallFryList.Add(smallFry);
    }

    public bool SmallFryExists()
    {
        return SmallFryList.Count > 0;
    }

    public void RemoveFromSmallFryList(SmallFry smallFry)
    {
        SmallFryList.Remove(smallFry);
    }
}
