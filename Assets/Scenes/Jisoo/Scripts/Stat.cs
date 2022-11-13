using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public StatType statType;
    public int initValue;                 // ��ȭ ���� 0�� ���� �ʱⰪ 
    public float currentValue;            // ���� ����
    public int currentReinforceLevel;     // ���� ��ȭ ����
    public int maxReinforceLevel;         // �ִ� ��ȭ ����
        
    public Stat(StatType statType, int initValue, int currentReinforceLevel, int maxReinforceLevel)
    {
        this.statType = statType;
        this.initValue = initValue;
        this.currentReinforceLevel = currentReinforceLevel;
        this.maxReinforceLevel = maxReinforceLevel;
    }
}