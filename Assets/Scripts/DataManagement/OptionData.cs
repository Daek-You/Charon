using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OptionData
{
    [SerializeField]
    private int bgmValue = 100;
    [SerializeField]
    private int seValue = 100;

    public int BgmValue { get { return bgmValue; } set { bgmValue = value; } }
    public int SeValue { get { return seValue; } set { seValue = value; } }
}
