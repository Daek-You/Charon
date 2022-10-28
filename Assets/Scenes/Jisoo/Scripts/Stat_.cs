using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public struct Stat_
{
    public StatType statType;
    public int initValue;                 // ��ȭ ���� 0�� ���� �ʱⰪ 
    public float currentValue;            // ���� ����
    public float IncreasingAmount;        // ���� ������
    public int currentReinforceLevel;     // ���� ��ȭ ����
    public int maxReinforceLevel;         // �ִ� ��ȭ ����

    //���⸦ ������ ���� ������
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