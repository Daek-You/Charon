using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public struct Stat
{
    public int initValue;                 // ��ȭ ���� 0�� ���� �ʱⰪ  
    public float IncreasingAmount;        // ���� ������
    public int currentReinforceLevel;     // ���� ��ȭ ����
    public int maxReinforceLevel;         // �ִ� ��ȭ ����

    public Stat(int initValue, float IncreasingAmout, int currentReinforceLevel,int maxReinforceLevel)
    {
        this.initValue = initValue;
        this.IncreasingAmount = IncreasingAmout;
        this.currentReinforceLevel = currentReinforceLevel;
        this.maxReinforceLevel = maxReinforceLevel;
    }
}