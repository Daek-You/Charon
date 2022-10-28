using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public struct _Stat
{
    public int initValue;                 // 강화 레벨 0일 때의 초기값  
    public float IncreasingAmount;        // 스탯 증가량
    public int currentReinforceLevel;     // 현재 강화 레벨
    public int maxReinforceLevel;         // 최대 강화 레벨

    public _Stat(int initValue, float IncreasingAmout, int currentReinforceLevel,int maxReinforceLevel)
    {
        this.initValue = initValue;
        this.IncreasingAmount = IncreasingAmout;
        this.currentReinforceLevel = currentReinforceLevel;
        this.maxReinforceLevel = maxReinforceLevel;
    }
}