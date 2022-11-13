using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatData
{
    public int currentReinforceLevel;
    public float increasingAmount;
    public int currentCost;
}

public class StatDataForLoad : ILoader<int, StatData>
{
    // Load�� Json ����Ʈ�� �����
    public List<StatData> statValues = new List<StatData>();

    public Dictionary<int, StatData> MakeDict()
    {
        Dictionary<int, StatData> dict = new Dictionary<int, StatData>();

        foreach (StatData data in statValues)
            dict.Add(data.currentReinforceLevel, data);

        return dict;
    }
}