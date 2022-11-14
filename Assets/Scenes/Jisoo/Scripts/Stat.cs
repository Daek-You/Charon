using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public StatType statType;
    public int initValue;                 // 강화 레벨 0일 때의 초기값 
    public float currentValue;            // 현재 스탯
    public int currentReinforceLevel;     // 현재 강화 레벨
    public int maxReinforceLevel;         // 최대 강화 레벨
        
    public Stat(StatType statType, int initValue, int currentReinforceLevel, int maxReinforceLevel)
    {
        this.statType = statType;
        this.initValue = initValue;
        this.currentReinforceLevel = currentReinforceLevel;
        this.maxReinforceLevel = maxReinforceLevel;
    }
}