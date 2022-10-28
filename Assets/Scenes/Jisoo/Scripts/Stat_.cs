using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public struct Stat_
{
    public StatType statType;
    public int initValue;                 // 강화 레벨 0일 때의 초기값 
    public float currentValue;            // 현재 스탯
    public float IncreasingAmount;        // 스탯 증가량
    public int currentReinforceLevel;     // 현재 강화 레벨
    public int maxReinforceLevel;         // 최대 강화 레벨

    //무기를 제외한 스탯 생성자
    public Stat_(StatType statType, int initValue, float IncreasingAmout, int currentReinforceLevel, int maxReinforceLevel)
    {
        this.statType = statType;
        this.initValue = initValue;
        this.currentValue = initValue;
        this.IncreasingAmount = IncreasingAmout;
        this.currentReinforceLevel = currentReinforceLevel;
        this.maxReinforceLevel = maxReinforceLevel;
    }
}