using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public StatType statType;
    public int initValue;                 // ��ȭ ���� 0�� ���� �ʱⰪ 
    public float currentValue;            // ���� ����
    public float increasingAmount;        // ���� ������
    public int currentReinforceLevel;     // ���� ��ȭ ����
    public int maxReinforceLevel;         // �ִ� ��ȭ ����

    public Stat(StatType statType, int initValue, float increasingAmount, int currentReinforceLevel, int maxReinforceLevel)
    {
        this.statType = statType;
        this.initValue = initValue;
        this.increasingAmount = increasingAmount;
        this.currentReinforceLevel = currentReinforceLevel;
        this.maxReinforceLevel = maxReinforceLevel;

    }
}